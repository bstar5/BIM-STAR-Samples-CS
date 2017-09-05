using System;
using System.Windows.Media;
using Mango;
using Mango.ViewModels;

namespace DemoApp.ViewModels.RibbonTabs
{
    public class RibbonTypeTabViewModel : RibbonTabViewModel
    {
        public RibbonTypeTabViewModel()
            : base("RibbonTabs/RibbonTypeTab.xml", "Assets")//第一个参数为xml文件的路径，第二个参数为xml文件里图片资源的路径
        {
        }

        protected override void OnLoadingLayout()
        {
            base.OnLoadingLayout();
            RegisterCommand("ShowWindow", OnShowWindow); //注册xml文件里的按钮点击事件，第一个参数为xml文件里的按钮点击事件名，第二个参数为执行方法
            RegisterCommand("ChangeRadioButton", OnChangeRadioButton);
            RegisterCommand("ChangeContextMenuItem", OnChangeContextMenuItem);
            RegisterCommand("SpeedChanged", OnSpeedChanged, CanChangeSpeed);
            RegisterCommand("ToggleButtonClick", OnToggleButtonClick);
            RegisterCommand("ColorChanged", OnColorChanged);
            RegisterCommand("DateTimeChanged", OnDateTimeChanged);
            RegisterCommand("GallerySelectedItemChanged", OnGallerySelectedItemChanged);
            RegisterCommand("ValueChanged", OnValueChanged);
            RegisterCommand("TextChanged", OnTextChanged);
            RegisterCommand("OnComboBoxSelectedItemChanged", OnComboBoxSelectedItemChanged);
        }

        private void OnShowWindow()
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

        private void OnChangeRadioButton(object vm)
        {
            var name = (vm as RibbonRadioButtonViewModel)?.Name;
            if (name == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"选中了{name}");
        }

        private void OnChangeContextMenuItem(object vm)
        {
            var header = (vm as RibbonContextMenuItemViewModel)?.ToString();
            if (header == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"选中了{header}");
        }

        private void OnSpeedChanged(object vm)
        {
            var speed = (float)(vm as RibbonNumericUpDownViewModel).Value;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"当前速度为：{speed}");
        }

        private bool CanChangeSpeed()
        {
            return true;//可增加状态判断
        }

        private void OnToggleButtonClick(object vm)
        {
            var isChecked = (vm as RibbonToggleButtonViewModel)?.IsChecked;
            if (isChecked == null)
                return;
            var mes = (bool)isChecked ? "选中状态" : "未选中状态";
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", mes);
        }

        private void OnColorChanged(object vm)
        {
            var color = (vm as RibbonColorPickerViewModel)?.Color;
            if (color == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"选中了{color}");
        }

        private void OnDateTimeChanged(object vm)
        {
            var time = (vm as RibbonDatePickerViewModel)?.DateTime;
            if (time == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"选择的日期为：{((DateTime)time).ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        private void OnGallerySelectedItemChanged(object vm)
        {
            var selectedItem = (vm as RibbonGalleryViewModel)?.SelectedItem;
            if (selectedItem == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"选中了{selectedItem.Name}");
        }

        private void OnValueChanged(object vm)
        {
            var value = (vm as RibbonSlideViewModel)?.Value;
            if (value == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"选取的值为：{value}");
        }

        private void OnTextChanged(object vm)
        {
            var value = (vm as RibbonTextBoxViewModel)?.Text;
            if (value == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"文本框的文本为：{value}");
        }

        private void OnComboBoxSelectedItemChanged(object vm)
        {
            var selectedItem = (vm as RibbonComboBoxViewModel)?.SelectedItem;
            if (selectedItem == null)
                return;
            Mg.Get<IMgDialog>().ShowDesktopAlert("提示", $"选择的字体为：{selectedItem}");
        }
    }
}