# UniversalAdmin
通用版后台管理框架

----------

重新编写此项目是为了解决以往开发的痛点:

- 需要权限控制的路由手动把路由地址添加到Route表中
- 编辑修改等参数的拼接很痛苦，容易出错
- UI丑的一B

对应解决思路：

- 使用自定义属性，在最终使用反射获取要权限控制的Action
- 添加/修改全部采用强类型的From表单，解放了拼接参数的步骤
- 使用[H+](http://www.zi-han.net/theme/hplus/?v=4.1 "H+ UI")集合框架

开发工具:VS2015

开发框架:MVC5,EF6.1

主要功能模块：

1. 后台用户模块
2. 系统日志功能
3. 权限管理
4. Demo模块来演示相册的处理和强类型的动态表单



懒得放到服务器上了，直接上图片:
![登录](http://i.imgur.com/uiO2PBx.jpg)

![首页](http://i.imgur.com/ztZqBvj.jpg)

![列表](http://i.imgur.com/lLXGFJ3.jpg)

![编辑用户](http://i.imgur.com/fx0TYme.jpg)

![提示消息](http://i.imgur.com/ujcxcjg.jpg)

![组权限](http://i.imgur.com/s5Gofv9.jpg)

![用户组列表](http://i.imgur.com/5NPEIat.jpg)

![系统操作日志](http://i.imgur.com/AlXGMgJ.jpg)

![系统异常日志](http://i.imgur.com/6auNQ47.jpg)

![强类型动态表单](http://i.imgur.com/C1r1KBq.jpg)

![出现异常的提示](http://i.imgur.com/yCQW3eq.jpg)

