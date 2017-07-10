using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DemoApp.Models;
using Mango;
using Mango.Models;

namespace DemoApp.ViewModels
{
    //界面会使用到一些telerik的控件，项目原先添加好的引用可能已无效，请重新添加。
    public class FirstViewModel : ViewModelBase
    {
        private MenuHub menuHub;
        private string menuName = "ExampleMunuName";

        public FirstViewModel()
        {
            DataSource = new List<Model>
            {
                new Model {Colums1 = "a1",Colums2 = "b1",Colums3 = "c1"},
                new Model {Colums1 = "a2",Colums2 = "b2",Colums3 = "c2"},
                new Model {Colums1 = "a3",Colums2 = "b3",Colums3 = "c3"},
                new Model {Colums1 = "a4",Colums2 = "b4",Colums3 = "c4"}
            };
            AddMenuItem = new RelayCommand(OnAddMenuItem, CanAddMenuItem);//绑定界面的添加菜单项的命令
            RemoveMenuItem = new RelayCommand(OnRemoveMenuItem, CanRemoveMenuItem);//绑定界面的删除菜单项的命令
            InitMenu();
        }

        private List<Model> _dataSource;

        public List<Model> DataSource
        {
            get { return _dataSource; }
            set { Set("DataSource", ref _dataSource, value); }
        }

        private Model _selectItem;

        public Model SelectItem
        {
            get { return _selectItem; }
            set { Set("SelectItem", ref _selectItem, value); }
        }

        private ObservableCollection<MenuItemInfo> _menuSource;

        public ObservableCollection<MenuItemInfo> MenuSource
        {
            get { return _menuSource; }
            set { Set("MenuSource", ref _menuSource, value); }
        }

        #region 添加菜单项

        public RelayCommand AddMenuItem { get; private set; }

        private bool CanAddMenuItem()
        {
            return true;
        }

        private void OnAddMenuItem()
        {
            if (MenuSource.Count == 0)
                return;
            var menucount = MenuSource.Count(t => !t.IsSeparator);
            var menu = new MenuItemInfo("菜单" + (menucount + 1), new RelayCommand(ShowWindow)) { GroupName = "Group" };
            menuHub.Register(menuName, menu);
            MenuSource.Add(menu);
        }

        #region 移除菜单项

        public RelayCommand RemoveMenuItem { get; private set; }

        private bool CanRemoveMenuItem()
        {
            return true;
        }

        private void OnRemoveMenuItem()
        {
            if (MenuSource.Count == 0)
                return;
            var menucount = MenuSource.Count(t => !t.IsSeparator);//不把菜单项分隔符计算在内
            var menuItemName = "菜单" + menucount;
            var index = MenuSource.IndexOf(MenuSource.FirstOrDefault(t => t.Text == menuItemName));
            if (index == -1)
                return;
            var menu = MenuSource.ElementAtOrDefault(index);
            menuHub.Unregister(menuName, menu);
            MenuSource.RemoveAt(index);
        }

        #endregion 移除菜单项

        #endregion 添加菜单项

        #region Private/Public Method

        private void InitMenu()
        {
            menuHub = new MenuHub(menuName);
            var menu1 = new MenuItemInfo("菜单1", new RelayCommand(ShowWindow)) { GroupName = "Group1" };
            var menu2 = new MenuItemInfo("菜单2", new RelayCommand(ShowWindow)) { GroupName = "Group1" };
            var menu3 = new MenuItemInfo("菜单3", new RelayCommand(ShowWindow)) { GroupName = "Group2" };
            var menu4 = new MenuItemInfo("菜单4", new RelayCommand(ShowWindow)) { GroupName = "Group2" };
            menu1.SubItems.AddRange(new[] { menu2, menu3 });
            //向全局注册菜单
            menuHub.Register(menuName, menu1, 1);
            menuHub.Register(menuName, menu2, 2);
            menuHub.Register(menuName, menu3, 3);
            menuHub.Register(menuName, menu4, 4);
            MenuSource = new ObservableCollection<MenuItemInfo>();//绑定界面的RadContextMenu的菜单集合
            var list = menuHub.GetMenuItemInfos(new List<string>() { menuName });//GetMenuItemInfos 为获取合并后的右键菜单
            if (list != null)
                MenuSource.AddRange(list);
        }

        private void ShowWindow()
        {
            if (SelectItem == null)
                this.ShowMessage("没有选择一项数据!");
            else
                this.ShowMessage(SelectItem.Colums1 + "\t" + SelectItem.Colums2 + "\t" + SelectItem.Colums3);
        }

        #endregion Private/Public Method
    }
}