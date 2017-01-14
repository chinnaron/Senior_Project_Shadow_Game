"# Senior_Project_Shadow_Game" 

Unity version 5.5.0
Download: https://unity3d.com/get-unity/download/archive?_ga=1.199498279.863946635.1482486599

Github Guides: https://guides.github.com/

Should Know:
- How to use
  - Vector3
  - Ray/Raycast/RaycastHit
- How to call function from other script

Code Explain:
- GridOverlay
  - สร้างกริดเก็บค่าตามชนิดของ obj โดยตรวจจากการส่ง ray จากฟ้าลงพื้น
  - มีฟังก์ชันไว้เรียกหาตำแหน่ง หาทางเดิน เซ็ตกริด
  
- ObjectController
  - เก็บค่าว่า obj นั้นๆ เป็นชนิดไหน
  
- PlayerController(ยังไม่โอเค)
  - คุมผู้เล่น คลิกเพื่อเดิน เมื่ออยู่ใกล้ของที่ขยับได้คลิกของเพื่อจับ
  - แยกส่วนการเคลื่อนที่และอนิเมชัน กับ เรื่องอื่นๆ
- TriggerController
 - เก็บค่า isOn ว่าโดนแสงเหลืองอยู่รึไม่ มีฟังก์ชันใช้เรียกโชว์ตามค่า isOn เรียกจาก YellowLight

- YellowLight
  - สร้างแสงเหลือง ใช้ ray ส่งไป 4 ทิศ หาว่าเจอของที่ trigger ได้ไหมแล้วจะเรียกฟังก์ชันจาก TriggerController
  
- ShadowController(ยังไม่โอเค)
  - เก็บค่าเงา 4 ทิศรอบ obj มีฟังก์ชันไว้เปิดปิดเงาที่เรียกจาก WhiteLight
  
- WhiteLight(ยังไม่โอเค)
  - สร้างแสงขาว ใช้ ray ส่งไป 4 ทิศ หาว่าเจอของที่สร้างเงาได้ไหมแล้วจะเรียกฟังก์ชันจาก ShadowController

- PushController
  
- BlueLight

- DestroyController
  
- RedLight