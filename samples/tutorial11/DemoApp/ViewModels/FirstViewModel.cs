using System.Collections.ObjectModel;
using DemoApp.Models;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            UserSource = new ObservableCollection<User>
            {
                new User{Number = "1",Name="小茗",Remark="-"},
                new User{Number = "2",Name="冷冷",Remark="-"},
                new User{Number = "3",Name="暖暖",Remark="-"}
            };
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

        private string _textBlockTest;

        /// <summary>
        /// 获取或设置TextBlockTest属性
        /// </summary>
        public string TextBlockTest
        {
            get { return _textBlockTest; }
            set { Set("TextBlockTest", ref _textBlockTest, value); }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "SelectedUserItem" && SelectedUserItem != null)
            {
                TextBlockTest = $"您刚刚选择了{SelectedUserItem.Name}!";
            }
        }
    }
}