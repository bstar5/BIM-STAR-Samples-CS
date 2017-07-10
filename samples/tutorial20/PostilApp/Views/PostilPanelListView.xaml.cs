using System.Windows.Input;

namespace PostilApp.Views
{
    /// <summary>
    /// PostilPanelListView.xaml 的交互逻辑
    /// </summary>
    public partial class PostilPanelListView
    {
        public PostilPanelListView()
        {
            InitializeComponent();
        }

        private void Grid_OnPreviewMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ListBox.SelectedItem = null;
        }
    }
}