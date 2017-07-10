using System.Collections.ObjectModel;
using DemoApp.Models;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase, IHandle<User>, IHandle<string>
    {
        public FirstViewModel()
        {
            UserSource = new ObservableCollection<User>
            {
                new User{Number = "1",Name="小茗",Remark="-"},
                new User{Number = "2",Name="冷冷",Remark="-"},
                new User{Number = "3",Name="暖暖",Remark="-"}
            };
            ButtonOneClick = new RelayCommand(OnButtonOneClick, CanButtonOneClick);
            ButtonTwoClick = new RelayCommand(OnButtonTwoClick, CanButtonTwoClick);
            MouseToButtonText = "鼠标请移进来";
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

        private string _mouseToButtonText;

        /// <summary>
        /// 获取或设置MouseToButtonText属性
        /// </summary>
        public string MouseToButtonText
        {
            get { return _mouseToButtonText; }
            set { Set("MouseToButtonText", ref _mouseToButtonText, value); }
        }

        /// <summary>
        /// 按钮一的点击事件
        /// </summary>
        public RelayCommand ButtonOneClick { get; private set; }

        private bool CanButtonOneClick()
        {
            return true;
        }

        private void OnButtonOneClick()
        {
            Messager.Send(this, new Message<string>("ViewModelToView", "你好！"));
        }

        /// <summary>
        /// 按钮二的点击事件
        /// </summary>
        public RelayCommand ButtonTwoClick { get; private set; }

        private bool CanButtonTwoClick()
        {
            return true;
        }

        private void OnButtonTwoClick()
        {
            var user = new User { Number = "4", Name = "小黑", Remark = "ViewModel发送给View" };
            Messager.Send(this, new Message<User>("ViewModelToView", user));
        }

        #region 消息通信

        /// <summary>
        /// string消息接收
        /// </summary>
        /// <param name="message"></param>
        public void Handle(Message<string> message)
        {
            if (message.Name == "ViewToViewModel")
            {
                //进行一些操作
                this.ShowMessage($"ViewModel：刚刚收到View发过来的string类型的通知！\r\n消息内容为:{message.Value}");
            }
        }

        /// <summary>
        /// 实体数据消息接收
        /// </summary>
        /// <param name="message"></param>
        public void Handle(Message<User> message)
        {
            if (message.Name == "ViewToViewModel")
            {
                //进行一些操作
                UserSource.Add(message.Value);
                this.ShowMessage($"ViewModel：刚刚收到View发过来的User类型的通知！\r\n消息内容为:{message.Value.Name}，并添加到了GridView里");
            }
        }

        #endregion 消息通信

        public void MouseEnter()
        {
            MouseToButtonText = "鼠标移进了按钮";
        }

        public void MouseLeave()
        {
            MouseToButtonText = "鼠标离开了按钮";
        }
    }
}