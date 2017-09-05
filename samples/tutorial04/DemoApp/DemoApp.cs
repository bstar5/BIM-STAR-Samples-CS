using System;
using System.Threading.Tasks;
using DemoApp.ViewModels;
using DemoApp.ViewModels.RibbonTabs;
using Mango;
using Mango.ViewModels;

namespace DemoApp
{
    public class DemoApp : App
    {
        private RibbonTabViewModel _tab;
        private RibbonTabViewModel _xmlTab;
        private RibbonGroupViewModel _groupOne;
        private RibbonGroupViewModel _groupTwo;
        private RibbonGroupViewModel _xmlGroup;
        private RibbonGroupViewModel _xmlGroupTwo;
        private RibbonButtonViewModel _buttonOne;
        private RibbonButtonViewModel _buttonTwo;

        protected override async Task OnStartupAsync()
        {
            /*******************采用代码方式创建************************/
            //创建RibbonTab标签
            _tab = RibbonExample.CreateRibbonTab(this, "CodeCreateTab", "代码Tab");
            //创建RibbonGroup 并加入Tab
            _groupOne = RibbonExample.CreateRibbonGroup("CodeCreateGroupOne", "代码Group");
            _tab.Groups.Add(_groupOne);//_tab.Groups.Insert(0,_group);
            //创建RibbonButton 在上述创建的RibbonGroup中加入RibbonButton
            _buttonOne = RibbonExample.CreateRibbonButton(this, "CodeCreateButton", "代码Button", ShowWindow);
            _buttonOne.Click = new RelayCommand(ShowWindow);//button点击的事件
            _groupOne.Items.Add(_buttonOne);
            //已有的RibbonTab中加入新的RibbonGroup
            var existTab = Mg.Get<IMgRibbon>().GetRibbonTab(_tab.Name);//传入已存在的tabName
            _groupTwo = RibbonExample.CreateRibbonGroup("CodeCreateGroupTwo", "向已有Tab加入的Group");
            existTab.Groups.Add(_groupTwo);//注意如果该Group中没有Item则在界面上不能显示
            //已有的RibbonGroup中加入新的RibbonButton
            var existGroup = Mg.Get<IMgRibbon>().GetRibbonGroup(_groupTwo.Name);//传入已存在的groupName
            _buttonTwo = RibbonExample.CreateRibbonButton(this, "CodeCreateButtonTwo", "向已有Group加入的Button", ShowWindow);
            existGroup.Items.Add(_buttonTwo);

            /*******************采用Xml方式创建************************/
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new ViewTabViewModel(), 1);
            //已有的RibbonTab中加入RibbonGroup
            _xmlTab = Mg.Get<IMgRibbon>().GetRibbonTab(LocalConfig.InsertTabName);//可以通过将一些已存在的TabName存在_config.xml里
            _xmlGroup = new ViewGroupViewModel();
            _xmlTab.Groups.Add(_xmlGroup);
            //已有的RibbonGroup中加入RibbonButton
            _xmlGroupTwo = new ViewButtonViewModel();
            //注意:ViewButtonViewModel并不是继承RibbonButtonViewModel，因为RibbonButtonViewModel不支持直接解析XML文件里的<Button>标签
            //因此只能通过外层加上<Group>标签,通过继承RibbonGroupViewModel去解析XML，然后得到Group里的Button组
            _xmlGroup = Mg.Get<IMgRibbon>().GetRibbonGroup(_xmlGroup.Name);
            _xmlGroup.Items.AddRange(_xmlGroupTwo.Items);

            /*******************采用Xml方式展示所有的Ribbon菜单基本控件************************/
            Mg.Get<IMgRibbon>().InsertRibbonTab(this, new RibbonTypeTabViewModel(), 2);
            //插入返回页的项
            Mg.Get<IMgRibbon>().InsertBackstageItem(this, new RibbonBackstageItemViewModel
            {
                Header = "新添加的项",
                Content = new FirstViewModel(),
                IsSelectable = true,
            });
            await Task.Yield();
        }

        protected override void OnExited()
        {
            //如果是在别人的Tab、Group里加入菜单按钮的，就不要移除别人的,只需移除自己加入的即可
            if (_groupOne != null)
            {
                //移除RibbonButton
                _groupOne.Items.Remove(_buttonOne);
                _groupOne.Items.Remove(_buttonTwo);
            }
            if (_tab != null)
            {
                //移除RibbonGroup
                _tab.Groups.Remove(_groupOne);
                _tab.Groups.Remove(_groupTwo);
            }
            Mg.Get<IMgRibbon>().RemoveRibbonTab(_tab);
            if (_xmlTab != null)
            {
                //移除RibbonGroup
                _xmlTab.Groups.Remove(_xmlGroup);
                _xmlTab.Groups.Remove(_xmlGroupTwo);
            }
            Mg.Get<IMgRibbon>().RemoveRibbonTab(_xmlTab);
        }

        public void ShowWindow()
        {
            var firstVm = new FirstViewModel();
            var vm = new DialogViewModel(firstVm)
            {
                Title = "窗口标题",
                Width = 500,
                Height = 500
            };
            var result = Mg.Get<IMgDialog>().ShowDialog(vm);
            if (result == CloseResult.Ok)
            {
                //点击窗口的确定按钮后需要执行的代码
            }
        }
    }
}