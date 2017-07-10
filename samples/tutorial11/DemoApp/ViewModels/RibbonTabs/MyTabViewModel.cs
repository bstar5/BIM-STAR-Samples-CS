using Mango;
using Mango.ViewModels;

namespace DemoApp.ViewModels.RibbonTabs
{
    public class MyTabViewModel : RibbonTabViewModel
    {
        public MyTabViewModel()
            : base("RibbonTabs/MyTab.xml", "Assets")//第一个参数为xml文件的路径，第二个参数为xml文件里图片资源的路径
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("ShowPane", OnShowPane); //注册xml文件里的按钮点击事件，第一个参数为xml文件里的按钮点击事件名，第二个参数为执行方法
        }

        private void OnShowPane()
        {
            Mg.Get<IMgDocking>().TryOpenPane<DemoApp, FirstViewModel>(t =>
            {
                t.Header = "我的面板";
                t.IsDocument = true;
                return new FirstViewModel();
            });
        }
    }
}