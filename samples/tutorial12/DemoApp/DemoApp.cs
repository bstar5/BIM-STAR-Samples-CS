using System.Threading.Tasks;
using DemoApp.ViewModels.RibbonTabs;
using Mango;

namespace DemoApp
{
    public class DemoApp : App
    {
        protected override async Task OnStartupAsync()
        {
            //插入Ribbon菜单
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new MyTabViewModel());
            //var menuName = "别的应用已存在的菜单名";
            //var mItemInfo = new MenuItemInfo("我是别的地方插入的菜单","已存在的全局命令名") { GroupName = "菜单组名" };
            //Mg.Get<IMgMenu>().Register(this, menuName, mItemInfo);//往别的应用的菜单添加菜单项
            await Task.Yield();
        }

        protected override void OnExited()
        {
            //移除Tab
            var tab = Mg.Get<IMgRibbon>().GetRibbonTab("MyTab");
            if (tab != null)
                Mg.Get<IMgRibbon>().RemoveRibbonTab(tab);
        }

        private void ShowWindow()
        {
            this.ShowMessage("我是一个全局命令。");
        }
    }
}