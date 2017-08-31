# 应用配置页的设置

## 简介

有时候应用需要第一次启动的时候保存一些配置数据，第二次启动的时候可以读取第一次设置的数据，然后可以根据这些配置数据来处理应用。比如每次启动都会默认有项目概况，那么可以在菜单栏->系统->系统设置->项目概况里取消勾选`启动时显示项目概况`，那么再次启动BIM-STAR时就不会显示项目概况了。`Mg.Get<IMgSetting>()`是框架里提供的系统设置管理器。

## 使用说明

1. 然后新建一个类，命名为`DemoAppSetting.cs`，代码如下所示。

   ```c#
   using Mango;
   namespace DemoApp
   {
       public class DemoAppSetting : SettingBase
       {
           /// <summary>
           /// 全局唯一对象
           /// </summary>
           public static DemoAppSetting Ins => Get<DemoAppSetting, DemoApp>();

           private DemoAppSetting(App app) : base(app)
           {
               SetDefaut("AlwaysShow", true);//设置默认值
               SetDefaut("Name", "深圳筑星科技有限公司");//设置默认值
           }

           public bool AlwaysShow
           {
               get { return Get<bool>("AlwaysShow"); }
               set { Set("AlwaysShow", value); }
           }

           public string Name
           {
               get { return Get<string>("Name"); }
               set { Set("Name", value); }
           }
       }
   }
   ```

2. 在`View`文件夹里添加一个用户控件，命名为`SettingView.xaml`， 代码内容如下所示。`CheckBox`控件的`IsChecked`绑定的是`DemoAppSetting`里的属性`AlwaysShow`，`TextBox`控件的`Text`绑定的是`DemoAppSetting`里的属性`Name`。

   ```xml
   <UserControl x:Class="DemoApp.Views.SettingView"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
                xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                xmlns:local="clr-namespace:DemoApp.Views"
                xmlns:demoApp="clr-namespace:DemoApp"
                mc:Ignorable="d"
                d:DesignHeight="300" d:DesignWidth="300">
       <StackPanel>
           <CheckBox IsChecked="{Binding Source={x:Static demoApp:DemoAppSetting.Ins},Path=AlwaysShow,Mode=TwoWay}" Content="启动时显示我的应用面板" />
           <StackPanel Orientation="Horizontal">
               <TextBlock Text="你的名字：" />
               <TextBox Width="100" Text="{Binding Source={x:Static demoApp:DemoAppSetting.Ins},Path=Name,Mode=TwoWay}" />
           </StackPanel>
       </StackPanel>
   </UserControl>
   ```

3. 在APP派生类`DemoApp`中添加如下代码。加载完所有的应用后，会执行`InstalledEvent()`方法，该方法根据`DemoAppSetting.Ins`里的属性`AlwaysShow`进行判断是否显示面板。通过`Mg.Get<IMgSetting>()`的`AddPluginSettingItem`方法往系统设置里添加专属应用的配置界面。

   ```c#
   using System.Threading.Tasks;
   using DemoApp.ViewModels;
   using DemoApp.ViewModels.RibbonTabs;
   using DemoApp.Views;
   using Mango;
   using Mango.Models;

   namespace DemoApp
   {
       public class DemoApp : App
       {
           protected override async Task OnStartupAsync()
           {
               //插入RibbonTab
               Mg.Get<IMgRibbon>().InsertRibbonTab(this, new MyTabViewModel());
               Mg.Get<IMgApp>().AddInstalledEventHandler(InstalledEvent);//应用安装后的处理事件
               await Task.Yield();
           }

           protected override void OnExited()
           {
               Mg.Get<IMgApp>().RemoveInstalledEventHandler(InstalledEvent);
           }

           private void InstalledEvent()
           {
               if (DemoAppSetting.Ins.AlwaysShow)
                   ShowPane();
               //系统设置中应用的设置界面
               Mg.Get<IMgSetting>().AddPluginSettingItem(this, new SettingItem("我的应用", new SettingViewModel()));
           }

           private void ShowPane()
           {
               Mg.Get<IMgDocking>().TryOpenPane<DemoApp, FirstViewModel>(t =>
               {
                   t.Header = "我的第一个面板";
                   t.IsDocument = true;
                   return new FirstViewModel();
               });
           }
       }
   }
   ```

4. 在`View`文件夹中的新建一个`FirstView.xaml`，代码内容如下所示。控件`TextBlock`中的`Text`绑定的是`FirstViewModel.cs`中的通知属性`Name`。`Button`控件的点击命令绑定的是`FirstViewModel.cs`中的`OpenSystemSetting`命令。

   ```xml
   <UserControl x:Class="DemoApp.Views.FirstView"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
                xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                xmlns:local="clr-namespace:DemoApp.Views"
                mc:Ignorable="d"
                d:DesignHeight="300" d:DesignWidth="300">
       <StackPanel>
           <TextBlock Text="{Binding Name}" />
           <Button Content="打开系统设置" Command="{Binding OpenSystemSetting}" Width="100" HorizontalAlignment="Left" />
       </StackPanel>
   </UserControl>
   ```

5. 在`ViewModel`文件夹中新建一个类，命名为`FirstViewModel.cs`，代码如下所示。

   ```c#
   using Mango;

   namespace DemoApp.ViewModels
   {
       public class FirstViewModel : ViewModelBase
       {
           public FirstViewModel()
           {
               if (!string.IsNullOrEmpty(DemoAppSetting.Ins.Name))
                   Name = "您好，" + DemoAppSetting.Ins.Name;
               else
                   Name = "Hello World!";
               OpenSystemSetting = new RelayCommand(OnOpenSystemSetting);
           }

           private string _name;

           /// <summary>
           /// 获取或设置Name属性
           /// </summary>
           public string Name
           {
               get { return _name; }
               set { Set("Name", ref _name, value); }
           }

           public RelayCommand OpenSystemSetting { get; private set; }

           private void OnOpenSystemSetting()
           {
               Mg.Get<IMgSetting>().OpenSettingDialog();
           }
       }
   }
   ```

6. 按F5运行项目，就可以看到我的应用面板会自动显示出来。

7. 在系统设置->我的应用中，取消勾选`启动时显示我的应用面板`,再次打开的时候会发现，面板就不会在之后启动时显示出来了。

   ​​