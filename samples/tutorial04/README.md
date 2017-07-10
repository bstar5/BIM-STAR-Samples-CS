# Ribbon菜单的使用

![1](Pictures/1.png)



## Ribbon菜单对应的相关类简介

- `Mg.Get<IMgRibbon>() `：`Ribbon`菜单管理类，在`Mango`的命名空间中；提供操作管理`Ribbon`菜单的相关的方法。
- `RibbonTabViewModel`：对应`UI`中`RibbonTab` 。
- `RibbonGroupViewModel` ：对应`UI`中`RibbonGroup`。
- `RibbonButtonViewModel/RibbonDropDownButtonViewModel`：对应`UI`中 `RibbonButton`。

## 创建Ribbon菜单（两种方式）

### Xml方式 创建Ribbon 菜单

1.`Xml`配置文件的方式 创建`Ribbon` 菜单。示例源码中`ViewTab.xml`文件中的完整代码如下所示。

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Tab Header="XmlTab" Name = "XmlTab">
  <Group Header="Group1" Name="Group1">
    <Button Text="Button" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
    <Button Text="Button" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
  </Group>
  <Group Header="Group2" Name="Group2">
    <Panel GroupType="Collapsible">
      <Button Text="Button" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
      <Button Text="Button" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
    </Panel>
  </Group>
  <Group Header="Group3" Name="Group3">
    <DropDownButton Text="方向" ButtonSize="Medium"  LargeImage="测试图片.png" SmallImage="测试图片16.png">
      <ContextMenu>
        <MenuGroupItem />
        <ContextMenuItem Header="上" Icon="测试图片16.png" Click="ShowWindow" />
        <MenuGroupItem />
        <ContextMenuItem Header="下" Icon="测试图片16.png" Click="ShowWindow" />
        <MenuGroupItem />
        <ContextMenuItem Header="左" Icon="测试图片16.png" Click="ShowWindow" />
        <MenuGroupItem />
        <ContextMenuItem Header="右" Icon="测试图片16.png" Click="ShowWindow" />
      </ContextMenu>
    </DropDownButton>
    <DropDownButton Text="速度" ButtonSize="Medium"  SmallImage="测试图片16.png">
      <ContextMenu>
        <MenuGroupItem />
        <ContextMenuItem Header="快" Icon="测试图片16.png" Click="ShowWindow" />
        <MenuGroupItem />
        <ContextMenuItem Header="慢" Icon="测试图片16.png" Click="ShowWindow" />
      </ContextMenu>
    </DropDownButton>
  </Group>
</Tab>
```

- `Tab` 标签对应`RibbonTab` ，其属性：`Header`的值为显示的名称，`Name`为该`Tab`唯一的标识符（通过此值获取对应的`RibbonTabViewModel`对象。
- `Group` 标签对应`RibbonGroup`，其属性：`Header` 的值为显示的名称，`Name`为该`Group`唯一的标识符（通过此值获取对应的`RibbonGroupViewModel `对象。
- `Button` 标签对应`RibbonButton`，其属性：`Text`的值为显示的名称，`Click `为点击事件的方法名称，`LargeImage`为大图标的名称， `SmallImage`为小图标的名称， `ButtonSize` 为按钮的尺寸，其值为`Large`，`Medium`，`Small`。 对应的对象为`RibbonButtonViewModel`。
- `DropDownButton` 标签对应的`DropDownButton`，可选；`Text`的值为显示名称，`Click `为点击事件的方法名称，`LargeImage`为大图标的名称， `SmallImage`为小图标的名称， `ButtonSize` 为按钮的尺寸，其值为`Large`，`Medium`，`Small`。对应的对象为`RibbonButtonViewModel`。
- `ContextMenu `标签对应下拉的菜单。
- `MenuGroupItem`标签对应下拉菜单项的分组。
- `ContextMenuItem`标签对应具体的下拉菜单项，`Header`的值为显示名称，`Click `为点击事件的方法名称，`Icon`为图标的名称 。

2.创建`ViewTab.xml`文件对应的`ViewModel` ，并继承`ViewTab.xml`中根元素对应的`VM`类，由于`ViewTab.xml`中的根元素是`Tab`，因此该`ViewTabViewModel`继承`RibbonTabViewModel`。`ViewTabViewModel.cs`完整代码如下。`ViewTabViewModel`将会解析`ViewTab.xml`中的内容。由于`ViewTab.xml`中的菜单按钮使用到了一些图片，这些图片在项目的`Assets`文件夹下，因此需要把`Assets`传入，程序才会定位到图片所在的位置。由于`ViewTab.xml`中的菜单按钮使用到了一些命令，因此需要把这些命令注册好，否则点击按钮就不会有事件发生。

```c#
using Mango;
using Mango.ViewModels;

namespace DemoApp.ViewModels.RibbonTabs
{
    public class ViewTabViewModel : RibbonTabViewModel
    {
        public ViewTabViewModel()
            : base("RibbonTabs/ViewTab.xml", "Assets")//第一个参数为xml文件的路径，第二个参数为xml文件里图片资源的路径
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("ShowWindow", OnShowWindow);//注册xml文件里的按钮点击事件，第一个参数为xml文件里的按钮点击事件名，第二个参数为执行方法
        }

        private void OnShowWindow()
        {
            var firstVm = new FirstViewModel();
            var vm = new DialogViewModel(firstVm)
            {
                Title = "窗口标题",
                Width = 500,
                Height = 500
            };
            var result = Mg.Get<IMgDialog>().ShowDialog(vm);
            if (result == CloseResult.Ok)
            {
                //点击窗口的确定按钮后需要执行的代码
            }
        }
    }
}
```

3.然后在`DemoApp`文件的应用启动方法`OnStartupAsync`中，添加如下代码，就可以把`ViewTab.xml`文件中配置的菜单显示出来了。其中的第三个参数是 插入的`Tab`所在的位置，如果为0，则显示在第一位。`ViewTabViewModel`会解析`ViewTab.xml`里的菜单数据，然后把得到的菜单往程序的菜单栏里插入。

```C#
        protected override async Task OnStartupAsync()
        {
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new ViewTabViewModel(), 0);
            await Task.Yield();
        }
```

4.运行项目，登录之后进入任意项目，可以发现`ViewTab.xml`配置的菜单全部都显示在了第一个菜单项中，并且按钮都具有相同的点击事件（这是因为我们只注册了一个事件命令），点击菜单按钮就会弹出一个窗体。效果如下图所示。

![2](Pictures/2.png)

5.如果要往已知一个已存在的`Tab`插入`Group`，也是同样的道理。首先先配置好一个`xml`文件，即创建一个`ViewGroup.xml`，它的代码如下所示。`Group`下包含需要加入的`Button`组数据。

```html
<?xml version="1.0" encoding="utf-8" ?>
<Group Header="向已有Tab加入的Group4" Name="Group4">
  <Button Text="Group4原有的Button" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
</Group>
```

6.然后创建`ViewGroup.xml`文件对应的`ViewModel` ，并继承`ViewGroup.xml`中根元素对应的`VM`类，由于`ViewGroup.xml`中的根元素是`Group`，因此该`ViewGroupViewModel`继承`RibbonGroupViewModel`。`ViewGroupViewModel.cs`完整代码如下。

```C#
using Mango;
using Mango.ViewModels;

namespace DemoApp.ViewModels.RibbonTabs
{
    public class ViewTabViewModel : RibbonTabViewModel
    {
        public ViewTabViewModel()
            : base("RibbonTabs/ViewTab.xml", "Assets")//第一个参数为xml文件的路径，第二个参数为xml文件里图片资源的路径
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("ShowWindow", OnShowWindow);//注册xml文件里的按钮点击事件
        }

        private void OnShowWindow()
        {
            var firstVm = new FirstViewModel();
            var vm = new DialogViewModel(firstVm)
            {
                Title = "窗口标题",
                Width = 500,
                Height = 500
            };
            var result = Mg.Get<IMgDialog>().ShowDialog(vm);
            if (result == CloseResult.Ok)
            {
                //点击窗口的确定按钮后需要执行的代码
            }
        }
    }
```

7.接着在`DemoApp`文件的应用启动方法`OnStartupAsync`中，添加如下代码，就可以把`ViewGroup.xml`文件中配置的按钮组显示在菜单栏里了。因为之前已经插入了一个`Tab`，所以知道它的属性`Name`为`XmlTab`，所以直接获取这个`Tab`的对象，通过`ViewGroupViewModel`解析`ViewGroup.xml`里的`Group`数据，然后再加入到`Tab`里。有时候可以将一些已知的`tab`的`Name`的值保存在`_config.xml`配置文件中，然后通过`LocalConfig.cs`去解析获取需要的`tab`的`Name`属性。示例源码就是这样的，而不是写死。

```C#
        protected override async Task OnStartupAsync()
        {
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new ViewTabViewModel(), 0);
            //已有的RibbonTab中加入RibbonGroup
            _xmlTab = Mg.Get<IMgRibbon>().GetRibbonTab("XmlTab");
            _xmlGroup = new ViewGroupViewModel();
            _xmlTab.Groups.Add(_xmlGroup);
            await Task.Yield();
        }
```

8.运行项目，登录之后进入任意项目，可以发现`ViewGroup.xml`配置的菜单按钮组显示在了第一列的菜单里。效果如下图所示。

![3](Pictures/3.png)

9.类似的，如果要往已知一个已存在的`Group`插入`Button`，首先先要配置`xml`文件，先创建一个`ViewButton.xml`，它的代码如下所示。

**注意**：由于 `RibbonButtonViewModel`没有`RibbonButtonViewModel("RibbonTabs/ViewTab.xml", "Assets")`方法，所以如果要通过`xml`文件获取`RibbonButton`组数据，只能使用如下的配置文件，外层加上`Group`标签。

```html
<?xml version="1.0" encoding="utf-8" ?>
<Group  Name="Group4">
  <Button Text="向已有Group4加入的Button1" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
  <Button Text="向已有Group4加入的Button2" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
  <Button Text="向已有Group4加入的Button3" ButtonSize="Large" CollapseToMedium="Never" LargeImage="测试图片.png" SmallImage="测试图片16.png" Click="ShowWindow" />
</Group>
```

10.然后通过`ViewButtonViewModel`继承`RibbonGroupViewModel`去解析`XML`配置文件，然后得到`Group`里的`Button`组，再将这`Button`组插入指定的`Group`。`ViewButtonViewModel`文件代码如下所示。

```C#
using Mango;
using Mango.ViewModels;

namespace DemoApp.ViewModels.RibbonTabs
{
    public class ViewButtonViewModel : RibbonGroupViewModel
    {
        public ViewButtonViewModel()
            : base("RibbonTabs/ViewButton.xml", "Assets")//第一个参数为xml文件的路径，第二个参数为xml文件里图片资源的路径
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("ShowWindow", OnShowWindow);//注册xml文件里的按钮点击事件
        }

        private void OnShowWindow()
        {
            var firstVm = new FirstViewModel();
            var vm = new DialogViewModel(firstVm)
            {
                Title = "窗口标题",
                Width = 500,
                Height = 500
            };
            var result = Mg.Get<IMgDialog>().ShowDialog(vm);
            if (result == CloseResult.Ok)
            {
                //点击窗口的确定按钮后需要执行的代码
            }
        }
    }
}
```

11.接着在`DemoApp`文件的应用启动方法`OnStartupAsync`中，添加如下代码，就可以把`ViewButton.xml`文件中配置的按钮组显示在菜单栏里了。因为之前已经插入了一个`Group`，所以知道它的属性`Name`为`Group4`，所以直接获取这个`Group`的对象，通过`ViewButtonViewModel`解析`ViewButton.xml`里的数据，然后再加入到`Group4`里。

```C#
        protected override async Task OnStartupAsync()
        {
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new ViewTabViewModel(), 0);
            //已有的RibbonTab中加入RibbonGroup
            _xmlTab = Mg.Get<IMgRibbon>().GetRibbonTab("XmlTab");
            _xmlGroupTwo = new ViewButtonViewModel();
            _xmlGroup = Mg.Get<IMgRibbon>().GetRibbonGroup("Group4");
            _xmlGroup.Items.AddRange(_xmlGroupTwo.Items);
            await Task.Yield();
        }
```

12.运行项目，登录之后进入任意项目，可以发现`ViewButton.xml`配置的菜单按钮组显示在了第一列的菜单里。效果如下图所示。

![4](Pictures/4.png)

### 代码方式创建Ribbon菜单

- 创建`RibbonTab`：

  ```C#
  M.RibbonManager.InsertRibbonTab(new RibbonTabViewModel()
  {
         Name = "Tab",//唯一的标识符
         Header = "开始",//显示的名称          
  },index);//Index为顺序，默认为最后一个
  ```


- 创建`RibbonGroup`：

  ```C#
  RibbonGroupViewModel group=new RibbonGroupViewModel()
  {
         Name = "Name",//唯一的标识符
         Header = "Header",//显示的名称                 
  };
  ```


- 将`RibbonGroup`加入`RibbonTab`

  ```C#
   var tab = M.RibbonManager.GetRibbonTab("TabName");
   tab.Groups.Insert(index,new RibbonGroupViewModel()//index为插入的位置
   {
        Name = "Name",//唯一的标识符
        Header = "Header",//显示的名称
   });
  ```


- 创建`RibbonButton`

  ```C#
  RibbonButtonViewModel buttonVm = new RibbonButtonViewModel()
  {
       Name = "Name",//唯一的标识符
       Text = "Text",//显示的名称
       LargeImage = "图标路径",
       ButtonSize = ButtonSize.Large,
       Click = new RelayCommand(OnClick)//按钮点击事件
  };
  ```


- 将`RibbonButton`加入到`RibbonGroup`

  ```c#
  var tab = M.RibbonManager.GetRibbonGroup("GroupName");
  tab.Items.Insert(index, new RibbonButtonViewModel()//index为插入的位置
  {
      Name = "Name",//唯一的标识符
      Text = "Text",//显示的名称
      LargeImage = "图标路径",
      ButtonSize = ButtonSize.Large,
      Click = new RelayCommand(OnClick)
  });
  ```

## 移除Ribbon菜单

- 移除`RibbonTab`

  ```C#
  var tab =  Mg.Get<IMgRibbon>().GetRibbonTab("TabName");
  Mg.Get<IMgRibbon>().RemoveRibbonTab(tab);
  ```

- 移除`RibbonGroup`

  ```C#
  var tab = Mg.Get<IMgRibbon>().GetRibbonTab("TabName");//要移除的Group所在的Tab
  var group = Mg.Get<IMgRibbon>().GetRibbonGroup("GroupName");
  tab.Groups.Remove(group);
  ```

- 移除`RibbonButton`

  ```C#
  var group = Mg.Get<IMgRibbon>().GetRibbonGroup("GroupName");
  group.Items.Remove(buttonVm);
  ```

  注意：`buttonVm`为上述创建`RibbonButton`所得到的`VM`类。

## 获取已知的Ribbon菜单对象

1.创建`tab`的配置文件，写入已知的`RibbonTab`的`Name`的值 ，如下（文件名为**`_config.xml`**）：

```xml
<Root InsertTabName="TabName/GroupName">//填写已知的TabName或这GroupName
</Root>
或者
<Root>
  <InsertTabName>TabName/GroupName</InsertTabName>
</Root>
```

2.创建配置文件解析类，解析上述的配置文件 如下（文件名及类名为**`LocalConfig.cs`**）：

```C#
public static class LocalConfig
{
    static LocalConfig()
    {
       //解析xml配置文件
        var root = XDocument.Load(typeof(LocalConfig).GetPluginResPath("_config.xml")).Root;
       //根据配置文件获取对应的节点或属性的值
        InsertTabName = root.Attribute("InsertTabName").Value;
       //InsertTabName = root.Element("InsertTabName").Value;
    }
    public static string InsertTabName { get; }
}
```

3.通过解析类`LocalConfig`可以得到当前的`Ribbon` 的`Name`的值，然后通过`GetRibbonXXX`就可以获取到菜单对象了，然后就可以对他们进行操作。当然了，你可以跳过前面的两个步骤，直接写死，不过不建议这么做。

```c#
var tab = Mg.Get<IMgRibbon>().GetRibbonTab(LocalConfig.InsertTabName);//获取指定的tab对象
或
var group= Mg.Get<IMgRibbon>().GetRibbonGroup(LocalConfig.InsertTabName);//获取指定的group对象
```

**注意**： 在应用的卸载方法中需要移除此应用中定义的相关菜单。

## Q/A

------

**Q**：`RibbonButton`样式怎么改变？

**A**：最常用的`RibbonButton`有两种一种是普通的`Button`，另一种为`DropDownButton`  

​	都有两种显示方式（通过`ButtonSize`属性控制)。

------

**Q**：已经按照步骤配置 但还是没有在`Ribbon`菜单中显示 

**A**：请先检查所在的已有`Tab`或者`Group`的应用是否有加载；若没有，则需要把对应的应用先加载出来；若还是没有显示新建的`Ribbon`菜单，右键xml文件选择属性，检查是否如下图所示。如果是通过xml方式创建的菜单，还有一种可能就是对应的xml文件路径不对，或者命名不对，请仔细核对。

![5](Pictures/5.png)

![6](Pictures/6.png)

------

**Q**：菜单读取不到图片

**A**：右键图片文件，选择属性，检查图片文件输出方式是否正确。

​      若正确检查`ViewModel`中的图片路径是否正确，如下图所示。

![6](Pictures/6.png)

`Assets`为`ViewGroup.xml`中项目中，图片资源所在的文件夹，

------

**Q**：应用卸载时移除菜单，却将整个`RibbonGroup`中的`RibbonButton`移除

**A**：移除菜单是要注意只要移除当前应用所创建的`Ribbon`菜单，所以需要将创建的`RibbonButton`设为全局的以便在卸载的时候使用，代码如下所示。当我们往别人的应用所创建的`Tab`里加入按钮组，则我们只需要卸载按钮组即可。

```C#
private RibbonButtonViewModel buttonVm = CreateRibbonButton();

public void RemoveRibbonButton()
{
    var group = Mg.Get<IMgRibbon>().GetRibbonGroup("GroupName");
    group.Items.Remove(buttonVm);
}
```