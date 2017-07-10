using System;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            ClickButton = new RelayCommand(OnClickButton, CanClickButton);
        }

        private string _text;

        /// <summary>
        /// 获取或设置Text属性
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { Set("Text", ref _text, value); }
        }

        public RelayCommand ClickButton { get; private set; }

        private bool CanClickButton()
        {
            return true;
        }

        private void OnClickButton()
        {
            Text = $"您于{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}点击了按钮！";
        }
    }
}