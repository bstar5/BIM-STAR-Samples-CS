using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using DemoApp.Models;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private readonly List<User> _bakUserSource;

        public FirstViewModel()
        {
            _bakUserSource = new List<User>();
            UserSource = new ObservableCollection<User>();
            ButtonClick = new RelayCommand(OnButtonClick, CanButtonClick);
            Search = OnSearchAsync;
        }

        private ObservableCollection<User> _userSource;

        /// <summary>
        /// RadGridView绑定的数据源
        /// </summary>
        public ObservableCollection<User> UserSource
        {
            get { return _userSource; }
            set { Set("UserSource", ref _userSource, value); }
        }

        private User _selectedUserItem;

        /// <summary>
        /// RadGridView绑定的选择项
        /// </summary>
        public User SelectedUserItem
        {
            get { return _selectedUserItem; }
            set { Set("SelectedUserItem", ref _selectedUserItem, value); }
        }

        private string _yourName;

        /// <summary>
        /// PromptBox绑定的文本
        /// </summary>
        public string YourName
        {
            get { return _yourName; }
            set { Set("YourName", ref _yourName, value); }
        }

        /// <summary>
        /// 按钮的点击事件
        /// </summary>
        public RelayCommand ButtonClick { get; private set; }

        private bool CanButtonClick()
        {
            return !IsBusy;
        }

        private async void OnButtonClick()
        {
            IsBusy = true;
            await Task.Delay(5000);//延迟5秒
            UserSource.AddRange(new[] {
                new User { Number = "1", Name = "小茗", Remark = "-" },
                new User { Number = "2", Name = "冷冷", Remark = "-" },
                new User { Number = "3", Name = "暖暖", Remark = "-" }
            });
            _bakUserSource.AddRange(UserSource);
            IsBusy = false;
        }

        protected override string GetErrorFor(string propertyName)
        {
            if (propertyName == "YourName")
            {
                if (string.IsNullOrEmpty(YourName))
                    return "填写的名字不能空！";
            }
            return base.GetErrorFor(propertyName);
        }

        #region SearchBox

        public Func<string, Task> Search { get; private set; }

        private async Task OnSearchAsync(string searchStr)
        {
            if (string.IsNullOrEmpty(searchStr))
            {
                UserSource = new ObservableCollection<User>();
                UserSource.AddRange(_bakUserSource);
            }
            var searchItems = await Task.Run(() =>
            {
                var items = new ObservableCollection<User>();
                if (searchStr == null) return items;
                searchStr = searchStr.ToLower().Trim();
                foreach (var item in _bakUserSource)
                {
                    if (ContainsString(item.Number + "", searchStr)
                        || ContainsString(item.Name, searchStr)
                        || ContainsString(item.Remark, searchStr)
                        )
                    {
                        items.Add(item);
                    }
                }
                return items;
            });
            UserSource = new ObservableCollection<User>(searchItems);
        }

        private static bool ContainsString(string str, string searchStr)
        {
            return str != null && str.Contains(searchStr, CompareOptions.IgnoreCase);
        }

        #endregion SearchBox
    }
}