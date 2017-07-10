using System.Windows;
using System.Windows.Input;

namespace PostilApp.Views
{
    /// <summary>
    /// AddPostilView.xaml 的交互逻辑
    /// </summary>
    public partial class AddPostilView
    {
        public AddPostilView()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };
            (sender as UIElement)?.RaiseEvent(eventArg);
        }
    }
}