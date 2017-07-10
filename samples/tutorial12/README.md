# 右键菜单

示例中的右键菜单采用控件是`telerik`中的`RadContextMenu` 控件，关于此控件的用法见`telerik控件的使用`章节。

- 创建`MenuHub`对象

  ```C#
   var menuHub = new MenuHub(menuNames);
   var menuHub = new MenuHub(false,menuNames);
  ```

  **参数解释**：`MenuHub`有两个构造函数：

  - 带一个参数的构造函数：该参数是类型为`string[]`的可变参数，包含一组菜单的唯一标识名。

  - 带两个参数的构造函数：第一个参数用于控制该菜单是否允许其他的应用在其中扩展命令，`false`为不允许。

    ​                                           第二个参数为`string`类型的可变参数，表示菜单的唯一标识名。

## 右键菜单中添加菜单项

- 在应用的内部添加，直接使用`MenuHub` 对象的`Register`方法，如下：

  ```C#
  var menuHub = new MenuHub("menuName");
  var mItemInfo= new MenuItemInfo("菜单项名", "command") { GroupName = "GroupName" };
  menuHub.Register("menuName", mItemInfo, 0);
  ```

  **说明**：`menuName`为要插入的菜单名，初始化一个`MenuHub`对象。`MenuItemInfo`对象构造函数包含两个参数，其中第一个参数为菜单项显示的名称，第二的参数可以是全局命令的唯一标识名（什么是全局命令请看`命令章节`），或者直接使用`ICommand`对象（具体如何使用请看`命令章节`）。 `GroupName` 为菜单项的组名，一个菜单里不同组之间有分割线进行分割。

  ​	 然后再通过 `MenuHub`对象 的 `Register`方法往指定的菜单里插入一个菜单项。 `Register`方法包含三个参数，`MenuName`为已知的要插入的菜单名，第二个参数为`MenuItemInfo`对象，第三个为菜单项要插入的位置。

- 往其它应用的菜单里添加，需要使用`Mg.Get<IMgMenu>()`中的`Register`方法 。注意：如果其他应用在注册菜单时就已经指定不让别的应用的菜单项插入到自己的菜单里时候，就无法插入菜单项。

  ```c#
  var mItemInfo= new MenuItemInfo("菜单项名", "command") { GroupName = "GroupName" };
  Mg.Get<IMgMenu>().Register("menuName", mItemInfo);
  ```

  **说明**：`menuName`为目标应用的菜单标识名。

## 卸载右键菜单中的菜单项

一般应用内部的菜单都可以自己卸载，但是如果想把插入到别的应用的菜单项卸载掉，那么就需要如下所示的代码。

```c#
var mItemInfo= new MenuItemInfo("DisplayName", "command") { GroupName = "GroupName" }; 
Mg.Get<IMgMenu>().Unregister("menuName", mItemInfo);
```

**说明**： 上述实例会卸载`menuName`菜单中的显示名称为`DisplayName`的菜单项。

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。