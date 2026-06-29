using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObstacleHazard : MonoBehaviour
{
    [SerializeField] int crashPenalty = 50;
    [SerializeField] bool costsLife;
    [SerializeField] float hitCooldown = 0.75f;
    [SerializeField] Color bumpEffectColor = new Color(0.9f, 0.96f, 1f, 0.95f);
    [SerializeField] int bumpParticleCount = 18;
    [SerializeField] float bumpEffectLifetime = 1.15f;

    float lastHitTime = -999f;

    public bool CostsLife => costsLife;

    public void Configure(bool shouldCostLife, int penalty)
    {
        costsLife = shouldCostLife;
        crashPenalty = Mathf.Max(0, penalty);
    }

    void Reset()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.isTrigger = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
        {
            return;
        }

        Driver driver = collision.collider.GetComponentInParent<Driver>();
        if (driver == null)
        {
            return;
        }

        Vector2 impactPoint = collision.contactCount > 0
            ? collision.GetContact(0).point
            : (Vector2)transform.position;

        HandleHit(driver, impactPoint);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
        {
            return;
        }

        Driver driver = other.GetComponentInParent<Driver>();
        if (driver == null)
        {
            return;
        }

        HandleHit(driver, transform.position);
    }

    void HandleHit(Driver driver, Vector2 impactPoint)
    {
        if (driver == null || Time.time - lastHitTime < hitCooldown)
        {
            return;
        }

        lastHitTime = Time.time;

        bool wasInvincible = GameManager.Instance.IsInvincible;
        if (costsLife)
        {
            GameManager.Instance.ApplyCrash(impactPoint, gameObject.name);
            if (wasInvincible)
            {
                GameManager.Instance.CompleteTrick("Obstacle smash", crashPenalty);
            }

            return;
        }

        PlayBumpEffect(impactPoint);

        if (wasInvincible)
        {
            GameManager.Instance.CompleteTrick("Rock smash", Mathf.Max(75, crashPenalty));
            return;
        }

        GameManager.Instance.ApplyObstacleBump(impactPoint, gameObject.name);
    }

    void PlayBumpEffect(Vector2 impactPoint)
    {
        ParticleSystem generatedEffect = CreateDefaultBumpEffect(impactPoint);
        generatedEffect.Play();
        Destroy(generatedEffect.gameObject, bumpEffectLifetime);
    }

    ParticleSystem CreateDefaultBumpEffect(Vector2 impactPoint)
    {
        GameObject effectObject = new GameObject("Rock Bump Effect");
        effectObject.transform.position = new Vector3(impactPoint.x, impactPoint.y, transform.position.z - 0.1f);

        ParticleSystem particles = effectObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.duration = 0.35f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.35f, 0.75f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(2.2f, 4.6f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);
        main.startColor = bumpEffectColor;
        main.gravityModifier = 0.25f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(0f, (short)Mathf.Max(1, bumpParticleCount))
        });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.18f;
        shape.arc = 180f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(bumpEffectColor, 0f),
                new GradientColorKey(Color.white, 0.55f),
                new GradientColorKey(Color.clear, 1f)
            },
            new[]
            {
                new GradientAlphaKey(0.95f, 0f),
                new GradientAlphaKey(0.65f, 0.65f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = 40;

        return particles;
    }
}
