using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DemoApp.Models;
using Mango;

namespace DemoApp.Views
{
    /// <summary>
    /// FirstView.xaml 的交互逻辑
    /// </summary>
    public partial class FirstView : IHandle<User>
    {
        public FirstView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// string消息接收
        /// </summary>
        /// <param name="message">string</param>
        public override void Handle(Message<string> message)
        {
            base.Handle(message);
            if (message.Name == "ViewModelToView")
            {
                //进行一些操作
                this.ShowMessage($"View：刚刚收到ViewModel发过来的string类型的通知！\r\n消息内容为:{message.Value}");
            }
        }

        /// <summary>
        /// 实体数据消息接收
        /// </summary>
        /// <param name="message">User实体</param>
        public void Handle(Message<User> message)
        {
            if (message.Name == "ViewModelToView")
            {
                this.ShowMessage($"View：刚刚收到ViewModel发过来的User类型的通知！\r\n消息内容为:{message.Value.Name}");
            }
        }

        private void ButtonThree_OnClick(object sender, RoutedEventArgs e)
        {
            //消息发送
            Messager.Send(this, new Message<string>("ViewToViewModel", "你好！"));
        }

        private void ButtonFour_OnClick(object sender, RoutedEventArgs e)
        {
            //消息发送
            var user = new User { Number = "5", Name = "小灰", Remark = "View发送给ViewModel" };
            Messager.Send(this, new Message<User>("ViewToViewModel", user));
        }
    }
}