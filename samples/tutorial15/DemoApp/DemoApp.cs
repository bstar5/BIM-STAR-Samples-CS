using System.Threading.Tasks;
using DemoApp.ViewModels;
using DemoApp.ViewModels.RibbonTabs;
using DemoApp.Views;
using Mango;
using Mango.Models;

namespace DemoApp
{
    public class DemoApp : App
    {
        protected override async Task OnStartupAsync()
        {
            //插入RibbonTab
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new MyTabViewModel());
            Mg.Get<IMgApp>().AddInstalledEventHandler(InstalledEvent);
            await Task.Yield();
        }

        protected override void OnExited()
        {
            Mg.Get<IMgApp>().RemoveInstalledEventHandler(InstalledEvent);
        }

        private void InstalledEvent()
        {
            if (DemoAppSetting.Ins.AlwaysShow)
                ShowPane();
            //系统设置中应用的设置界面
            Mg.Get<IMgSetting>().AddPluginSettingItem(this, new SettingItem("我的应用", new SettingViewModel()));
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