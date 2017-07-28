using System.Threading.Tasks;
using DemoApp.ViewModels.RibbonTabs;
using Mango;

namespace DemoApp
{
    public class DemoApp : App
    {
        protected override async Task OnStartupAsync()
        {
            //插入RibbonTab
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new MyTabViewModel());
            await Task.Yield();
        }

        protected override void OnExited()
        {
        }
    }
}