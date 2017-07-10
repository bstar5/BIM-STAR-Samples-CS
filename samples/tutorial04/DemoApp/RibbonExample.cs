using System;
using Mango;
using Mango.ViewModels;

namespace DemoApp
{
    public class RibbonExample
    {
        /// <summary>
        /// 创建Tab
        /// </summary>
        public static RibbonTabViewModel CreateRibbonTab(IApp iApp, string tabName, string header, int index = 0)
        {
            var tab = new RibbonTabViewModel
            {
                Name = tabName,
                Header = header,
            };
            Mg.Get<IMgRibbon>().InsertRibbonTab(iApp, tab, index);//index为插入的位置
            return tab;
        }

        /// <summary>
        /// 创建Group
        /// </summary>
        public static RibbonGroupViewModel CreateRibbonGroup(string groupName, string header)
        {
            var group = new RibbonGroupViewModel
            {
                Name = groupName,
                Header = header,
            };
            return group;
        }

        /// <summary>
        /// 创建Button
        /// </summary>
        public static RibbonButtonViewModel CreateRibbonButton(IApp iApp, string buttonName, string text, Action action)
        {
            var buttonVm = new RibbonButtonViewModel()
            {
                Name = buttonName,
                Text = text,
                LargeImage = iApp.GetAppResPath("Assets\\测试图片.png"),
                SmallImage = iApp.GetAppResPath("Assets\\测试图片.png"),
                ButtonSize = ButtonSize.Large,
                Click = new RelayCommand(action)
            };
            return buttonVm;
        }
    }
}