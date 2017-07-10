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
                    Text = "通过菜单组名插入的按钮",
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
            await Task.Yield();
        }

        protected override void OnExited()
        {
            //移除按钮
            _groupVm?.Items.Remove(_viewBtn);
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