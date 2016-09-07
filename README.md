# Live-2D-with-Unity
Import Live2D and read mtn file In Unity3D

## Prepare
  Unity 5.4.0f3<br />
  Live2D for Unity SDK 2.1<br />
  you can find SDK [here](http://sites.cybernoids.jp/cubism-sdk2_e/unity_2-1)  
  Extract file you will see content like below
  ![SDK](http://imgur.com/1dJEvxm.jpg)
  In Unity project new five folder in Assets ：Editor, Live2D, Resources, Scene and Scripts.  
  like below  
  ![five folder](http://imgur.com/Nr5uWMe.jpg)  
##Import Live2D  
將Live2D SDK中的  
1、ramework
2、lib/Live2DUnity.dll
3、lib/ Resources
拖曳至Assets/Live2D下。  
![Import Live2D](http://imgur.com/0rJPSQO.jpg)  
再來一樣由Live2D SDK中：  
1、sample  
2、utils  
兩個資料夾放入Assets/Scripts中  
![Import Live2D](http://imgur.com/HwjolU7.jpg)  
接著把CreateCanvas.cs與Live2DImporter.cs放置在Assets/Editor下
![Import Live2D]( http://imgur.com/hTqSgVe.jpg) 

##Import Live2D model  
將要使用的Live2D model放置在Assets/Resources/live2d下  
並將Live2D_Canvas拖曳至Assets/Resources/下方  
這邊使用Live2D 中 Sample Material的haru
![Import Live2D](http://imgur.com/QSdhctH.jpg)  
![Import Live2D](http://imgur.com/yngxpgM.jpg)  
完成後會在menu bar中看到Live2D的選項，  
有這個選項就可以在場景中建置一個放置Model的Canvas。
![Menubar Live2D](http://imgur.com/0mugZhB.jpg)  

#呼叫mtn檔的方法 1
  
