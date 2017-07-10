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
        private RibbonGroupViewModel _groupVm;

        protected override async Task OnStartupAsync()
        {
            //插入RibbonTab
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new MyTabViewModel());
            //在已有的菜单组中插入菜单按钮
            _groupVm = Mg.Get<IMgRibbon>().GetRibbonGroup(LocalConfig.InsertGroupName);
            if (_groupVm != null)
            {
                _viewBtn = new RibbonButtonViewModel
                {
                    ButtonSize = ButtonSize.Large,
                    Text = "我的按钮",
                    LargeImage = this.GetAppResPath("Assets\\logo.jpg"),
                    SmallImage = this.GetAppResPath("Assets\\logo.jpg"),
                    Click = new RelayCommand(ShowPane)//按钮命令
                };
                _groupVm.Items.Add(_viewBtn);
            }
            else
            {
                Mg.Get<IMgLog>().Warn($"没有找到名称为{LocalConfig.InsertGroupName}的菜单组,无法插入菜单按钮！");
            }
            //注册全局命令，全局命令可以任何地方使用
            Mg.Get<IMgCommand>().Register(this, new CommandInfo("AlertMessage", new RelayCommand(OnAlertMessage)));
            Mg.Get<IMgCommand>().Register(this, new CommandInfo("AlertMessageAsync", new AsyncCommand(OnAlertMessageAsync, CanAlertMessageAsync)));
            await Task.Yield();
        }

        protected override void OnExited()
        {
            _groupVm?.Items.Remove(_viewBtn);
            //移除Tab
            var tab = Mg.Get<IMgRibbon>().GetRibbonTab("MyTab");
            if (tab != null)
            {
                tab.Groups.Remove(_groupVm);
                Mg.Get<IMgRibbon>().RemoveRibbonTab(tab);
            }
        }

        private void ShowPane()
        {
            Mg.Get<IMgDocking>().TryOpenPane<DemoApp, FirstViewModel>(t =>
            {
                t.Header = "我的面板";
                t.IsDocument = true;
                return new FirstViewModel();
            });
        }

        private void OnAlertMessage()
        {
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "这个一个全局的命令（非异步）");
        }

        private void OnAlertMessageAsync()
        {
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "这个一个全局的命令（异步）");
        }

        private int _index;

        private async Task<bool> CanAlertMessageAsync()
        {
            await Task.Delay(1000);
            _index++;
            return _index % 2 == 0;
        }
    }
}