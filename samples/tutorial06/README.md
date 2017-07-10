# Button 命令绑定

`View`层的按钮怎么相应`ViewModel`层的命令呢，下面我们举个小例子，`View`层代码如下所示。`Button`的属性`Command`是负责绑定`ViewModel`层中的命令对象，`ButtonOneClick`和`ButtonTwoClick`对应于`ViewModel`层的`RelayCommand`对象（非异步命令），`ButtonThreeClick`对应于`ViewModel`层的`AsyncCommand`对象（异步命令）。

```xml
<Grid VerticalAlignment="Top">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <Button Content="可点击按钮" Command="{Binding ButtonOneClick}" Margin="5" />
    <Button Grid.Column="1" Content="不可点击按钮" Command="{Binding ButtonTwoClick}" Margin="5" />
    <Button Grid.Column="2" Content="异步命令按钮，5秒后才可点击" Command="{Binding ButtonThreeClick}" Margin="5" />
</Grid>
```

`ViewModel`层的代码，如下所示。

```c#
using System.Threading.Tasks;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private bool _buttonTwoCanClick;

        public FirstViewModel()
        {
            ButtonOneClick = new RelayCommand(OnButtonOneClick, CanButtonOneClick);
            ButtonTwoClick = new RelayCommand(OnButtonTwoClick, CanButtonTwoClick);
            ButtonThreeClick = new AsyncCommand(OnButtonThreeClick, CanButtonThreeClick);
        }

        public RelayCommand ButtonOneClick { get; private set; }

        private bool CanButtonOneClick()
        {
            return true;
        }

        private void OnButtonOneClick()
        {
            this.ShowMessage("您点击了第一个按钮！同时您赋予了第二个按钮点击的能力！");
            _buttonTwoCanClick = true;
        }

        public RelayCommand ButtonTwoClick { get; private set; }

        private bool CanButtonTwoClick()
        {
            return _buttonTwoCanClick;
        }

        private void OnButtonTwoClick()
        {
            this.ShowMessage("您点击了第二个按钮！同时第二个按钮丧失了被点击的能力！");
            _buttonTwoCanClick = false;
        }

        public AsyncCommand ButtonThreeClick { get; private set; }

        private async Task<bool> CanButtonThreeClick()
        {
            await Task.Delay(5000);
            return true;
        }

        private void OnButtonThreeClick()
        {
        }
    }
}
```

一定要记得在`ViewModel`层的构造函数`FirstViewModel()`中对命令进行初始化，否则命令会无效。`new RelayCommand(agr1,agr2)`的第一个参数是按钮点击之后需要执行的方法，第二个参数是控制按钮的状态的方法。可以根据自己的需要对按钮的状态进行修改，`AsyncCommand`也是如此。

**注意：**运行示例代码进入项目后，菜单栏->点击`我的菜单`->点击`我的按钮`，`FirstView.xaml`所显示的界面就会出现。