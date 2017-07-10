using System;
using System.Windows;

namespace DemoApp.Views
{
    /// <summary>
    /// FirstView.xaml 的交互逻辑
    /// </summary>
    public partial class FirstView
    {
        public FirstView()
        {
            InitializeComponent();
        }

        protected override void OnRefreshTheme()
        {
            base.OnRefreshTheme();
            Resources = new ResourceDictionary { Source = new Uri("/DemoApp;component/Views/FirstViewRes.xaml", UriKind.RelativeOrAbsolute) };
        }
    }
}