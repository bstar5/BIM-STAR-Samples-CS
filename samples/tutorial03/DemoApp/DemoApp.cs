using System.Threading.Tasks;
using DemoApp.ViewModels;
using DemoApp.ViewModels.RibbonTabs;
using Mango;
using Mango.ViewModels;

namespace DemoApp
{
    public class DemoApp : App
    {
        private RibbonButtonViewModel _viewBtn;
        private RibbonButtonViewModel _viewBtnAsync;
        private RibbonGroupViewModel _groupVm;

        protected override async Task OnStartupAsync()
        {
            //插入菜单组
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new MyTabViewModel());
            //在已有的菜单组中插入菜单按钮
            _groupVm = Mg.Get<IMgRibbon>().GetRibbonGroup(LocalConfig.InsertGroupName);
            if (_groupVm != null)
            {
                _viewBtn = new RibbonButtonViewModel
                {
                    ButtonSize = ButtonSize.Large,
                    Text = "打开非异步窗口",
                    LargeImage = this.GetAppResPath("Assets\\logo.jpg"),
                    SmallImage = this.GetAppResPath("Assets\\logo.jpg"),
                    Click = new RelayCommand(ShowWindow)//按钮命令
                };
                _groupVm.Items.Add(_viewBtn);//添加到菜单组里
                _viewBtnAsync = new RibbonButtonViewModel
                {
                    ButtonSize = ButtonSize.Large,
                    Text = "打开异步窗口",
                    LargeImage = this.GetAppResPath("Assets\\logo.jpg"),
                    SmallImage = this.GetAppResPath("Assets\\logo.jpg"),
                    Click = new RelayCommand(ShowWindowAsync)//按钮命令
                };
                _groupVm.Items.Add(_viewBtnAsync);//添加到菜单组里
            }
            else
            {
                Mg.Get<IMgLog>().Warn($"没有找到名称为{LocalConfig.InsertGroupName}的菜单组,无法插入菜单按钮！");
            }
            await Task.Yield();
        }

        protected override void OnExited()
        {
            //移除按钮
            _groupVm?.Items.Remove(_viewBtn);
            _groupVm?.Items.Remove(_viewBtnAsync);
        }

        private void ShowWindow()
        {
            var firstVm = new FirstViewModel();
            var vm = new DialogViewModel(firstVm)
            {
                Title = "不是异步的窗口",
                Width = 500,
                Height = 500
            };
            var result = Mg.Get<IMgDialog>().ShowDialog(vm);//非异步窗口
            this.ShowMessage("窗口返回结果了！");
            if (result == CloseResult.Ok)
            {
                //点击窗口的确定按钮后需要执行的代码
            }
        }

        private async void ShowWindowAsync()
        {
            var firstVm = new FirstViewModel();
            var vm = new DialogViewModel(firstVm)
            {
                Title = "异步的窗口",
                Width = 500,
                Height = 500
            };
            var result = await Mg.Get<IMgDialog>().ShowDialogAsync(vm);//异步窗口
            this.ShowMessage("窗口返回结果了！");
            if (result == CloseResult.Ok)
            {
                //点击窗口的确定按钮后需要执行的代码
            }
        }
    }
}