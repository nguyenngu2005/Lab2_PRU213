using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    const string RootName = "__MusicManager";
    const string MenuMusicPath = "Audio/antipodeanwriter-chilliwave-dome-living-12184";
    const string GameplayMusicPath = "Audio/maksymmalko-gaming-game-minecraft-background-music-372242";
    const string EndGameMusicPath = "Audio/kawaiiwork-lovely-garden-540008";

    static MusicManager instance;

    AudioSource source;
    string currentPath;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Bootstrap()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Ensure().PlayForScene(SceneManager.GetActiveScene().name);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Ensure().PlayForScene(scene.name);
    }

    static MusicManager Ensure()
    {
        if (instance != null)
        {
            return instance;
        }

        GameObject existing = GameObject.Find(RootName);
        if (existing != null && existing.TryGetComponent(out MusicManager existingManager))
        {
            instance = existingManager;
            return instance;
        }

        GameObject managerObject = new GameObject(RootName);
        instance = managerObject.AddComponent<MusicManager>();
        DontDestroyOnLoad(managerObject);
        return instance;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = 0.38f;
    }

    void PlayForScene(string sceneName)
    {
        string targetPath = GetMusicPath(sceneName);
        if (string.IsNullOrEmpty(targetPath))
        {
            StopMusic();
            return;
        }

        if (currentPath == targetPath && source != null && source.isPlaying)
        {
            source.volume = GetVolume(sceneName);
            return;
        }

        AudioClip clip = Resources.Load<AudioClip>(targetPath);
        if (clip == null)
        {
            Debug.LogWarning($"Music clip not found in Resources: {targetPath}");
            StopMusic();
            return;
        }

        currentPath = targetPath;
        source.clip = clip;
        source.volume = GetVolume(sceneName);
        source.Play();
    }

    static string GetMusicPath(string sceneName)
    {
        if (sceneName == SceneFlow.MenuSceneName)
        {
            return MenuMusicPath;
        }

        if (sceneName == SceneFlow.EndGameSceneName)
        {
            return EndGameMusicPath;
        }

        return SceneFlow.IsGameScene(sceneName) ? GameplayMusicPath : string.Empty;
    }

    static float GetVolume(string sceneName)
    {
        if (SceneFlow.IsGameScene(sceneName))
        {
            return 0.34f;
        }

        return sceneName == SceneFlow.EndGameSceneName ? 0.4f : 0.36f;
    }

    void StopMusic()
    {
        currentPath = string.Empty;
        if (source != null)
        {
            source.Stop();
            source.clip = null;
        }
    }
}
