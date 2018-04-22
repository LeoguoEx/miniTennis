# miniTennis

打算制作我的第一个游戏，小游戏用于练手。

制作一个小型、2D、天堂视角的网球游戏。

大致玩法为：

1.简单玩法：闯关？ 每次阶段完成会增加发球机个数，并且重新编排发球频率

训练模式，玩家选择喜欢的运动员，控制运动员接发球机射出的球，并计分，当漏接发球机的球则算失败，否则计分，通过最后获得的总分数高低来计算排名。发球机的发射的节奏和数量由回合数控制，开始较慢球数较低，且球的运动轨迹为支线，并且球的角度较小，随着接到的回合数越多，发球数量越多，轨迹越诡异，且角度越大（可能吊角），在发射球的过程中，球场地中会出现小范围光圈，当击回的球击中光圈，则获得一次额外能力（上限三次），超过三次变为击中球后一段时间积分翻倍，通过特定方式（还没想好），释放一段时间能力。



2.PVE（玩家总共有三条命机会）

玩家和角色对打，角色为AI，角色可以释放技能等一些高端的操作，胜利和失败方式与简单玩法一致，当漏球则失败，当击回则计分，结束后比较计分多少。区别在于每个角色有自己独特的技能，每次击回球会增加一部分能量值，当能量值达到上限后可以释放技能。发动的技能有较大概率让玩家无法击回，对应ai也会释放技能，在ai释放技能时需要有提示，让玩家有提前准备。在打球过程中，场地中会有金币等场外道具出现，击回的球击中道具，就会获得该道具在游戏外使用。



玩家操作：

玩家通过手指点击角色来控制当按下除UI以外的屏幕，则表示控制角色，角色开始准备挥拍，当玩家按住拖动屏幕，角色会根据手指一动方向来移动，距离和玩家拖动的距离一致，当玩家松手时角色挥拍，球的飞行方向和玩家的控制方向做处理，玩家对角色移动的方向会影响球击出的方向，但是玩家需要注意移动范围和移动力度以防止出界。界面中额外增加一个按钮用于角色释放技能，玩家可以通过点击按钮来释放或者使用技能。



技能：

1.香蕉球（弧线）   次数限制

2.跳球  （用2d做螺旋线效果）    次数限制

3.停球一段时间后再挥出（玩家击中球后球会停在拍上，玩家可以移动角色到想要击球的区域后进行释放（可以主动挥出，或者等待时间结束自动弹出） 次数限制

4.降低球的移动速度（将整个场景的速度放慢来击球）    一段时间

5.增加击球速度                                                        一段时间

6.增加击球范围                                                       一段时间

7.场外增加防护罩可以主动弹出未击中的球         一段时间

8.绝对领域，对方击球会主动飞向玩家角色         一段时间

9.显示ai的击回球的路径					 一段时间



场外道具：

金币（游戏货币）--- 可以购买解锁角色或者获得一次抽奖机会。



进阶：

增加局域网对战或者蓝牙对战等一些功能。让玩家可以更好的开始玩，随后更新新的游戏玩法。



开始制作，希望这次能够完成。