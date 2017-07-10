# 图片素材的绑定

## `Image`控件绑定图片

- 直接在`View`代码中绑定图片

`View`中添加控件`Image`，赋值`Source`，代码如下所示。需要添加引用`Mango.dll`，将`View`中的`UserControl`改为`mango:ViewBase`，才可以使用这种方式。

```xml
<mango:ViewBase x:Class="DemoApp.Views.FirstView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:local="clr-namespace:DemoApp.Views"
             xmlns:mango="clr-namespace:Mango;assembly=Mango.Wpf"
             mc:Ignorable="d"
             d:DesignHeight="500" d:DesignWidth="500">
	<Image Source="{mango:AppResource ImagePath=Assets/测试图片.png}" Width="100" Height="100"/>
</mango:ViewBase>
```

其中`Assets/测试图片.png`为图片的相对路径。

- 通过`ViewModel`层绑定图片

`ViewModel`中添加一个通知属性，类型为`ImageSource`，对该属性赋值，然后对应的界面`View`代码中绑定该属性。

`ViewModel`代码如下所示，通过`this.GetAppImageSource("图片相对路径")`获取图片的ImageSource。

```C#
using System.Windows.Media;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            ImageSource = this.GetAppImageSource("Assets\\测试图片.png");
		}

        private ImageSource _imageSource;

        /// <summary>
        /// 获取或设置ImageSource属性
        /// </summary>
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { Set("ImageSource", ref _imageSource, value); }
        }
    }
}
```

对应的`View`关键代码如下所示。

```Html
<Image Source="{Binding ImageSource}" Width="100" Height="100"  Margin="5" />
```

- 通过URL绑定图片

  当我们向系统上传一张图片的时候，系统会在API返回该图片的下载链接。因此系统还提供了绑定URL的方式，需要添加引用`Mango.dll`，才可以使用这种方式。当然了，网络上各种各样的图片的下载链接都可以绑定，而不只是系统返回的图片下载链接。示例源码中绑定的链接就是百度图片中某一张图片的下载链接。

  `ViewBox`代码如下所示。

  ```C#
  using System.Windows.Media;
  using Mango;

  namespace DemoApp.ViewModels
  {
      public class FirstViewModel : ViewModelBase
      {
          public FirstViewModel()
          {
              ImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=2300258792,2484561302&fm=23&gp=0.jpg";
  		}

          private ImageSource _imageSource;

          private string _imageUrl;

          /// <summary>
          /// 获取或设置ImageUrl属性
          /// </summary>
          public string ImageUrl
          {
              get { return _imageUrl; }
              set { Set("ImageUrl", ref _imageUrl, value); }
          }
      }
  }
  ```

  对应的`View`关键代码如下所示。

  ```Html
  <Image mango:RemoteImageEx.UriSource="{Binding ImageUrl}" Width="100" Height="100" Margin="5" />
  ```

## 控件背景绑定图片

以控件`Button`为例，把按钮替换成我们想要的图片按钮。

- `View`的关键代码如下所示，`Image`的绑定方式取上面的任意一种即可。

  ```html
  <Button Width="100" Height="30" Margin="5">
       <Image Source="{Binding ImageSource}" />
  </Button>
  ```


- 还有一种方式是通过`Metro Studio`该软件获得对应的图标`Code`里的`Data`数据，不同图标有不同的`Data`，具体详情请百度`Metro Studio`的使用，关键代码如下所示。

  ```html
  <Button Width="100" Height="30" Margin="5">
        <Viewbox>
             <Path Data="M299.152955025434,0L496.426178902388,0 268.890046089888,280.247772216797 198.190201729536,367.407897949219 199.87503144145,369.495361328125 271.502137154341,457.706451416016 498.999970406294,738 301.717621773481,738 101.234124153852,491.008697509766 99.5304498374462,488.936553955078 98.6409082114697,490.003173828125 0,368.488952636719 98.6409082114697,246.945526123047z"
                Stretch="Uniform" Fill="{Binding Source={x:Static mango:M.ThemeManager},Path=AccentBrush}"/>
         </Viewbox>
  </Button>
  ```

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。