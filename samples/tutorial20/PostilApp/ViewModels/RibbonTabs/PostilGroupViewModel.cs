using System.Windows;
using Mango;
using Mango.ViewModels;

namespace PostilApp.ViewModels.RibbonTabs
{
    public class PostilGroupViewModel : RibbonGroupViewModel
    {
        public PostilGroupViewModel()
            : base(@"RibbonTabs\PostilGroup.xml", "Assets")
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("OpenAddPostilView", OnOpenAddPostilView);
            RegisterCommand("OpenCheckPostilView", OnOpenCheckPostilView);
            RegisterCommand("OpenPostilListView", OnOpenPostilListView);
        }

        public async void OnOpenAddPostilView()
        {
            var privilege = await Mg.Get<IMgContext>().GetPrivilegeAsync(ModuleKeys.OpenAddPostilView);
            if (!privilege)
            {
                Mg.Get<IMgLog>().Error("您没有添加批注的权限！");
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "您没有添加批注的权限！");
                return;
            }
            var addPostil = new AddPostilViewModel();
            var vm = new DialogViewModel(addPostil)
            {
                Title = "添加批注",
                Width = 1400,
                Height = 800,
                CancelButtonVisibility = Visibility.Visible,
                OkButtonVisibility = Visibility.Visible,
                Icon = this.GetAppImageSource("Assets/添加批注.png"),
                ResizeMode = ResizeMode.CanResize
            };
            var result = Mg.Get<IMgDialog>().ShowDialog(vm);
            if (result != CloseResult.Ok)
                return;
            addPostil.OnAddPostil();
        }

        public async void OnOpenCheckPostilView()
        {
            var privilege = await Mg.Get<IMgContext>().GetPrivilegeAsync(ModuleKeys.OpenCheckPostilView);
            if (!privilege)
            {
                Mg.Get<IMgLog>().Error("您没有查看批注详情的权限！");
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "您没有查看批注详情的权限！");
                return;
            }
            var checkPostils = new CheckPostilsViewModel();
            var vm = new DialogViewModel(checkPostils)
            {
                Title = "查看批注",
                Width = 1200,
                Height = 700,
                CancelButtonVisibility = Visibility.Collapsed,
                OkButtonVisibility = Visibility.Collapsed,
                Icon = this.GetAppImageSource("Assets/查看批注.png")
            };
            Mg.Get<IMgDialog>().ShowDialog(vm);
        }

        public async void OnOpenPostilListView()
        {
            var privilege = await Mg.Get<IMgContext>().GetPrivilegeAsync(ModuleKeys.OpenPostilListView);
            if (!privilege)
            {
                Mg.Get<IMgLog>().Error("您没有查看批注列表的权限！");
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", "您没有查看批注列表的权限！");
                return;
            }
            Mg.Get<IMgDocking>().TryOpenPane<PostilApp, PostilPanelListViewModel>(t =>
            {
                t.Header = "批注列表";
                t.InitialPosition = DockState.DockedRight;
                return new PostilPanelListViewModel();
            });
        }
    }
}