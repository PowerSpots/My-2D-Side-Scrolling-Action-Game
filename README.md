Gnome's Precious: My Side-Scrolling Action Game

关于游戏的说明

游戏预览：

![img](https://github.com/PowerSpots/Readme/blob/master/My-2D-Side-Scrolling-Action-Game/Preview_Side_Scrolling.gif)


---------------------------------------------------------------<b>游戏设计：</b>---------------------------------------------------------------


游戏简介

    玩家控制一个矮人园丁，通过一根绳子进出井中。井底有一堆宝藏，但是井里充满了各种陷阱。如果矮人接触了陷阱，就会被杀死。

![img](https://github.com/PowerSpots/Readme/blob/master/My-2D-Side-Scrolling-Action-Game/PrototypeFirstVersion.png)

    这个游戏的实现依赖与Unity的2D物理和图形系统，还有内置的新UI系统。

    矮人实际上是一个玩偶：通过弹簧关节SringJoint2D连接的一堆对象，每个对象都是独立模拟的刚体。

![img](https://github.com/PowerSpots/Readme/blob/master/My-2D-Side-Scrolling-Action-Game/ConfigureTheSpringJoint2D-ConnectedAnchorAndTheAnchor.png) ![img](https://github.com/PowerSpots/Readme/blob/master/My-2D-Side-Scrolling-Action-Game/ConfigureTheSpringJoint2D-ConnectTheLegToTheRope.png)

    绳索的制作方式类似：它由一组刚体组成，全部用关节相互连接。

    游戏相机上有一个脚本，用来保持相机位置与矮人的垂直位置相关联，同时也能防止相机显示井顶或井底以下的任何物体。


游戏规则

    游戏规则通过直接的碰撞检测来处理：

        如果矮人的任何部分触及一个陷阱对象，那么矮人就会死掉，并且会重新生成一个新的矮人。

        如果宝藏被触碰，矮人的精灵图片将被更新以显示他握有宝藏。

        如果矮人到达了井的顶部（不可见对象），此时如果矮人拿着宝藏，那么玩家将赢得游戏。


游戏草图

![img](https://github.com/PowerSpots/Readme/blob/master/My-2D-Side-Scrolling-Action-Game/ConceptSketch.bmp)


UI系统

![img](https://github.com/PowerSpots/Readme/blob/master/My-2D-Side-Scrolling-Action-Game/Menu_Side_Scrolling.gif)


游戏成品

![img](https://github.com/PowerSpots/Readme/blob/master/My-2D-Side-Scrolling-Action-Game/TheFinishedProduct.png)



----------------------------------------------------------English Document------------------------------------------------------------



Gnome's Precious: My Side-Scrolling Action Game

This is an 'About The Game' more than a Readme.md document

-------------------------------------------------------------<b>Game Design</b>--------------------------------------------------------------


Introduction    

    The player con‐ trols a garden gnome, who’s being lowered by an attached rope into a well. At the bottom of the well there’s some treasure. The catch is that the well is filled with traps that kill the gnome if he touches them.

    This game relies on the 2D graphics and physics features of Unity, in addition to fairly heavy use of the UI system.

    The gnome is a “ragdoll”—a collection of pieces that are connected via joints, with each piece an independently simulated rigid body.

    The rope is made in a similar way: it’s a collection of rigid bodies, all connected to each other with joints.

    The game’s camera has a script running that keeps its position linked to the vertical position of the gnome, but also keeps the camera from showing anything above the top of the well or below the bottom of the well. 


Rules   

    The rule of the gameplay is handled through very straightforward collision detection: 

        ·If any part of the gnome touches a trap object, the gnome is dead, and a new gnome is created.

        ·If the treasure is touched, the gnome’s sprites are updated to show that he’s holding the treasure.

        ·If the top of the well (an invisible object) is touched, and if the gnome is holding the treasure, the player wins the game.







    