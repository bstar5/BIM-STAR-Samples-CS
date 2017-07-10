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
    }
}