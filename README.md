# SnowBoarder - Lab 2 PRU213

## 1. Thông tin project

- Tên game: SnowBoarder
- Thể loại: 2D arcade snowboarding
- Engine: Unity 6
- Scene chính: `Menu`, `Level1`, `Level2`, `Level3`, `EndGame`
- Thư mục code chính: `Assets/LabAssets/Scripts`
- Thư mục scene chính: `Assets/LabAssets/Scenes`

SnowBoarder là game trượt tuyết 2D. Người chơi điều khiển nhân vật đi xuống dốc, giữ thăng bằng, tăng tốc bằng năng lượng boost, ghi điểm bằng tốc độ, trick, powerup và hoàn thành 3 màn chơi.

## 2. Cách chạy project

1. Mở project bằng Unity 6.
2. Mở scene `Assets/LabAssets/Scenes/Menu.unity`.
3. Bấm Play trong Unity Editor.
4. Chọn Start để vào chuỗi màn chơi.

Thứ tự scene trong Build Settings:

1. `Assets/LabAssets/Scenes/Menu.unity`
2. `Assets/LabAssets/Scenes/Level1.unity`
3. `Assets/LabAssets/Scenes/Level2.unity`
4. `Assets/LabAssets/Scenes/Level3.unity`
5. `Assets/LabAssets/Scenes/EndGame.unity`

## 3. Điều khiển

| Phím | Chức năng |
| --- | --- |
| `A/D` hoặc mũi tên trái/phải | Nghiêng, điều hướng và lộn người khi ở trên không |
| `W` hoặc mũi tên lên | Tăng tốc cơ bản |
| `S` hoặc mũi tên xuống | Phanh khi ở trên mặt đất, chủ động rơi xuống khi đang trên không |
| `Space` | Giữ để boost, tiêu hao thanh năng lượng |
| `P` hoặc `Esc` | Tạm dừng |
| `R` | Chơi lại màn hiện tại |

## 4. Gameplay chính

- Người chơi di chuyển bằng physics 2D với `Rigidbody2D`, trọng lực và mặt tuyết có độ trượt.
- Camera tự động đi theo nhân vật.
- HUD hiển thị điểm, tốc độ, số mạng, combo, multiplier và thanh năng lượng boost.
- Thanh năng lượng boost giảm khi giữ `Space` và tự hồi lại khi không sử dụng.
- Rơi ra ngoài giới hạn map sẽ tính là thua.
- Chạm đầu hoặc va chạm nặng với mặt đất sẽ mất mạng.
- Chạm đá chỉ làm chậm lại và reset combo, không trừ mạng.
- Cây là vật trang trí nền, không gây va chạm và không cản trở gameplay.
- Game có 3 level liên tiếp, sau Level3 sẽ sang màn EndGame.

## 5. Hệ thống điểm

Điểm được cộng từ nhiều nguồn:

- Di chuyển với tốc độ cao trong thời gian chơi.
- Ăn powerup.
- Thực hiện trick/lộn vòng thành công.
- Về đích qua finish line.
- Giữ combo để tăng multiplier.

Combo sẽ tăng khi người chơi thực hiện hành động ghi điểm liên tiếp. Nếu bị va chạm hoặc hết thời gian combo, combo sẽ reset.

## 6. Powerup và chướng ngại vật

Powerup được tạo theo runtime trên track guide để vị trí hợp lý hơn với địa hình. Các loại powerup:

- Speed boost: tăng tốc trong thời gian ngắn.
- Invincibility: bảo vệ người chơi tạm thời.
- Shortcut impulse: đẩy nhân vật tiến về phía trước.

Chướng ngại vật:

- Rock: vật cản nhẹ, làm chậm người chơi nhưng không trừ mạng.
- Tree: dùng làm scenery/background, không có collider gây cản trở.

## 7. UI và scene flow

### Menu

- Có background full màn hình.
- Có nút Start, Options và Quit.
- Quit hoạt động trong Unity Editor bằng cách dừng Play Mode, và thoát game khi build.

### Gameplay

- HUD nằm trên màn hình cho biết trạng thái người chơi.
- Có thanh năng lượng boost trực quan, không hiện thêm chữ trạng thái boost để giao diện gọn hơn.
- Có hiệu ứng tuyết rơi tạo không khí mùa đông.

### EndGame

- Có background riêng.
- Hiển thị kết quả thắng/thua, điểm cuối, combo tốt nhất.
- Có nút Retry, Main Menu và Quit.

## 8. Script chính

| Script | Vai trò |
| --- | --- |
| `Driver.cs` | Xử lý di chuyển, nghiêng, boost energy, tốc độ, out-of-bounds |
| `SnowboardInput.cs` | Gom input bàn phím cho gameplay |
| `GameManager.cs` | Quản lý điểm, mạng, combo, trạng thái thắng/thua |
| `GameHud.cs` | Cập nhật UI điểm, tốc độ, mạng, combo, thanh năng lượng |
| `CrashDetector.cs` | Xử lý va chạm mất mạng và phân biệt hazard không gây mất mạng |
| `ObstacleHazard.cs` | Cấu hình chướng ngại vật như đá/cây |
| `PowerUp.cs` | Xử lý powerup và phần thưởng |
| `FinishLine.cs` | Xử lý về đích và chuyển màn |
| `SceneFlow.cs` | Quản lý luồng scene trong campaign |
| `MenuSceneController.cs` | Tạo và điều khiển UI menu |
| `EndGameSceneController.cs` | Tạo và điều khiển UI endgame |
| `MusicManager.cs` | Phát nhạc theo từng scene |
| `SnowboardBootstrap.cs` | Thiết lập gameplay object, powerup, camera, giới hạn map |

## 9. Đối chiếu yêu cầu Lab 2

| Yêu cầu | Trạng thái |
| --- | --- |
| Player có thể điều khiển nhân vật | Hoàn thành |
| Có physics 2D, gravity, slope/snow movement | Hoàn thành |
| Có UI hiển thị thông tin gameplay | Hoàn thành |
| Có điểm số và cơ chế tính điểm | Hoàn thành |
| Có vật cản/chướng ngại vật | Hoàn thành |
| Có powerup | Hoàn thành |
| Có âm thanh/nhạc nền | Hoàn thành |
| Có menu riêng | Hoàn thành |
| Có màn kết thúc riêng | Hoàn thành |
| Có nhiều level/map | Hoàn thành |
| Có điều kiện thắng/thua | Hoàn thành |
| Có tài liệu giải thích project | Hoàn thành |

## 10. Điều chỉnh sau playtest

Trong quá trình test, một số nội dung runtime generated đã được tắt để project ổn định hơn:

- Checkpoint generated bị tắt vì dễ bị lệch/chìm theo địa hình.
- Snowflake collectible generated bị tắt vì vị trí hiển thị không ổn định.
- Jump ramp generated bị tắt vì không khớp với scene layout.
- Tree collider bị tắt vì cây quá lớn và gây cản trở người chơi.
- Rock được đổi thành bump hazard không trừ mạng để game công bằng hơn.

Những thay đổi này giúp gameplay ổn định, dễ chơi và dễ demo hơn.

## 11. Hướng dẫn test nhanh

1. Từ `Menu`, bấm Start để vào `Level1`.
2. Dùng `A/D` để điều khiển và nghiêng nhân vật.
3. Giữ `Space` để kiểm tra thanh năng lượng giảm dần.
4. Thả `Space` để kiểm tra thanh năng lượng hồi lại.
5. Chạm đá để xác nhận không bị trừ mạng.
6. Rơi khỏi map để xác nhận game tính thua.
7. Qua finish line để chuyển sang level tiếp theo.
8. Hoàn thành `Level3` để vào `EndGame`.
9. Thử các nút Retry, Main Menu và Quit.

## 12. Hạn chế và hướng phát triển

- Có thể thêm lại collectible/checkpoint bằng cách đặt thủ công trong scene thay vì tạo runtime.
- Có thể thêm pause menu đầy đủ hơn.
- Có thể thêm leaderboard thời gian hoàn thành.
- Có thể thêm nhiều âm thanh và hiệu ứng khi boost, va chạm, ăn powerup.
- Có thể thêm nhiều nhân vật/ván trượt với chỉ số khác nhau.

## 13. Kết luận

Project đã hoàn thành phần gameplay cốt lõi của Lab 2: điều khiển nhân vật, physics trượt tuyết, score/combo, powerup, vật cản, UI, âm thanh, menu, endgame và nhiều level. Các phần dễ gây lỗi khi demo đã được tinh gọn để game chạy ổn định hơn.
