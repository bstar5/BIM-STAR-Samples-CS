using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            if (!string.IsNullOrEmpty(DemoAppSetting.Ins.Name))
                Name = "您好，" + DemoAppSetting.Ins.Name;
            else
                Name = "Hello World!";
            OpenSystemSetting = new RelayCommand(OnOpenSystemSetting);
        }

        private string _name;

        /// <summary>
        /// 获取或设置Name属性
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        public RelayCommand OpenSystemSetting { get; private set; }

        private void OnOpenSystemSetting()
        {
            Mg.Get<IMgSetting>().OpenSettingDialog();
        }
    }
}