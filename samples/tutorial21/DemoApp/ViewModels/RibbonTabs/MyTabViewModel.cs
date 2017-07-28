using Mango;
using Mango.ViewModels;

namespace DemoApp.ViewModels.RibbonTabs
{
    public class MyTabViewModel : RibbonTabViewModel
    {
        public MyTabViewModel()
            : base(@"RibbonTabs\MyTab.xml", "Assets")//对应菜单配置文件和图片的文件夹名称
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("ShowPane", OnShowPane);
        }

        private void OnShowPane()
        {
            Mg.Get<IMgDocking>().TryOpenPane<DemoApp, FirstViewModel>(t =>
            {
                t.Header = "调用图形平台服务的面板";
                t.InitialPosition = DockState.DockedRight;
                return new FirstViewModel();
            });
        }
    }
}