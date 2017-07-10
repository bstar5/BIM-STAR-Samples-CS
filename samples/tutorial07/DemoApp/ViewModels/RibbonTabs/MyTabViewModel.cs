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
            RegisterCommand("OpenMyFirstPane", OnOpenMyFirstPane);//注册xml文件中的按钮点击事件
        }

        private void OnOpenMyFirstPane()
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