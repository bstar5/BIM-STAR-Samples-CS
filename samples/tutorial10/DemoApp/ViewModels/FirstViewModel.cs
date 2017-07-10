using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DemoApp.Models;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            YearList = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("2017", "2017年"),
                new Tuple<string, string>("2018", "2018年"),
                new Tuple<string, string>("2019", "2019年")
            };
            UserSource = new ObservableCollection<User>
            {
                new User{Number = "1",Name="小茗",Remark="-"},
                new User{Number = "2",Name="冷冷",Remark="-"},
                new User{Number = "3",Name="暖暖",Remark="-"}
            };
            ButtonClick = new RelayCommand(OnButtonClick, CanButtonClick);
        }

        private List<Tuple<string, string>> _yearList;

        /// <summary>
        /// 年份列表
        /// </summary>
        public List<Tuple<string, string>> YearList
        {
            get { return _yearList; }
            set { Set("YearList", ref _yearList, value); }
        }

        private ObservableCollection<User> _userSource;

        /// <summary>
        /// RadGridView的数据源
        /// </summary>
        public ObservableCollection<User> UserSource
        {
            get { return _userSource; }
            set { Set("UserSource", ref _userSource, value); }
        }

        private User _selectedUserItem;

        /// <summary>
        /// 获取或设置SelectedUserItem属性
        /// </summary>
        public User SelectedUserItem
        {
            get { return _selectedUserItem; }
            set { Set("SelectedUserItem", ref _selectedUserItem, value); }
        }

        private string _textBoxText;

        /// <summary>
        /// 获取或设置TextBoxText属性
        /// </summary>
        public string TextBoxText
        {
            get { return _textBoxText; }
            set { Set("TextBoxText", ref _textBoxText, value); }
        }

        public RelayCommand ButtonClick { get; private set; }

        private bool CanButtonClick()
        {
            return true;
        }

        private void OnButtonClick()
        {
            var mess = string.IsNullOrEmpty(TextBoxText) ? "您什么都没输入" : $"您刚刚输入了：{TextBoxText}";
            this.ShowMessage(mess);
        }
    }
}