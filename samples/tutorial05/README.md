# ViewModel与View的创建

对`MVVM`框架中，创建`ViewModel`与`View`是基础工作。下面将一步一步讲述如何在`BIM-STAR`框架之下如何创建他们。

> 创建的前提是，你已经懂得了如何在BIM-STAR框架下创建应用；然后在此基础之上进行下面的操作，之后就可以看到效果

- 第一步 添加引用`Mango.dll`和`Mango.Wpf.dll`

  这个在前面的章节已说过，这里就不再详述如何添加这两个引用了。


- 第二步 创建文件夹`ViewModels`、`Views`

  在应用的项目下创建文件夹`ViewModels`、`Views`。其中`ViewModels`为`ViewModel`层，`Views`为`View`层


- 第三步 在`ViewModels`文件夹下创建`xxxViewModel.cs`类

  `xxxViewModel`其中的xxx为用户自定义名称，然后`FirstViewModel`类继承`Mango`命名空间中的`ViewModelBase`类，`ViewModelBase`类提供一些通知属性、一些可重写的方法以及一些框架特性，因此需要继承它，具体代码如下所示。

```c#
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
      //这里做一些界面的业务逻辑处理
    }
}
```

- 第四步 在`Views`文件夹下创建`xxxView.xaml`

创建界面，其中名称应与想要关联 `ViewModel`层中类的名称一致，如该界面要与`FirstViewModel.cs`对应，应命名为`FirstView.xaml`，千万要注意不要写错了。创建之后需要添加引用`Syatem.Xaml.dll`，它支持解析和处理可扩展应用程序标记语言 (XAML)，默认没有添加，需要自己手动添加。再把`UserControl`改为`mango:ViewBase`，然后就可以在界面上添加一些自己想要添加的控件了。`FirstView.xaml`代码如下。如果对`WPF`不怎么了解的建议先去学习一下`WPF`的基础知识之后再看本教程效果会更好。

```html
<mango:ViewBase x:Class="DemoApp.Views.FirstView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:local="clr-namespace:DemoApp.Views"
             xmlns:mango="clr-namespace:Mango;assembly=Mango.Wpf"
             mc:Ignorable="d"
             d:DesignHeight="300" d:DesignWidth="300">
    <StackPanel>
        <Grid Margin="10">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="100" />
                <ColumnDefinition Width="Auto" />
            </Grid.ColumnDefinitions>
            <TextBlock Text="TextBlock控件：" VerticalAlignment="Center" />
            <TextBlock Grid.Column="1" VerticalAlignment="Center">ViewModel及View创建</TextBlock>
        </Grid>
        <Grid Margin="10">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="100" />
                <ColumnDefinition Width="Auto" />
            </Grid.ColumnDefinitions>
            <TextBlock Text="Text控件：" VerticalAlignment="Center" />
            <TextBlock Grid.Column="1" Text="{Binding Text}" />
        </Grid>
        <Grid Margin="10">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="100" />
                <ColumnDefinition Width="100" />
            </Grid.ColumnDefinitions>
            <TextBlock Text="Button控件：" VerticalAlignment="Center" />
            <Button Grid.Column="1" Content="点击" Command="{Binding ClickButton}" />
        </Grid>
    </StackPanel>
</mango:ViewBase>
```

把文件`FirstView.xaml.cs`中的`:UserControl`去掉，重新编译一下，具体代码如下。

```c#
namespace DemoApp.Views
{
    /// <summary>
    /// FirstView.xaml 的交互逻辑
    /// </summary>
    public partial class FirstView
    {
        public FirstView()
        {
            InitializeComponent();
        }
    }
```

- 第六步 效果查看

文件`DemoApp.cs`中的代码如下所示。在主程序添加了一个`我的菜单`，通过点击我的菜单的按钮，文件`FirstView.xaml`所显示的界面就会出现。关于菜单的代码可以去看菜单章节，这里不再详述。

```c#
namespace DemoApp
{
    public class DemoApp : App
    {
        protected override async Task OnStartupAsync()
        {
            //插入我的菜单
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new ViewTabViewModel());
            await Task.Yield();
        }

        protected override void OnExited()
        {
            var myTab = Mg.Get<IMgRibbon>().GetRibbonTab("MyTab");
            if (myTab != null)
                Mg.Get<IMgRibbon>().RemoveRibbonTab(myTab);
        }
    }
}
```

现在，你已经完成了`ViewModel`和`View`的创建、WPF基本控件的使用与显示。继续加油！

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。