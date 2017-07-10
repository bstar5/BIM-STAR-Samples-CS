using System.Threading.Tasks;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            ButtonClick = new RelayCommand(OnButtonClick, CanButtonClick);
        }

        public RelayCommand ButtonClick { get; private set; }

        private bool CanButtonClick()
        {
            return true;
        }

        private async void OnButtonClick()
        {
            Hide();//隐藏界面
            await Task.Delay(3000);//等待3秒钟
            Show();//显示界面
        }
    }
}