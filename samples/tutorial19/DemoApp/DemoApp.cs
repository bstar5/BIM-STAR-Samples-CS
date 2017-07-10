using System.Threading.Tasks;
using DemoApp.ViewModels.RibbonTabs;
using Mango;

namespace DemoApp
{
    public class DemoApp : App
    {
        protected override async Task OnStartupAsync()
        {
            //插入我的菜单
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new ViewTabViewModel());
            await Task.Yield();
        }

        protected override void OnExited()
        {
            var myTab = Mg.Get<IMgRibbon>().GetRibbonTab("MyTab");
            if (myTab != null)
                Mg.Get<IMgRibbon>().RemoveRibbonTab(myTab);
        }
    }
}