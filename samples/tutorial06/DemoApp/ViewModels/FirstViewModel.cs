using System.Threading.Tasks;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private bool _buttonTwoCanClick;

        public FirstViewModel()
        {
            ButtonOneClick = new RelayCommand(OnButtonOneClick, CanButtonOneClick);
            ButtonTwoClick = new RelayCommand(OnButtonTwoClick, CanButtonTwoClick);
            ButtonThreeClick = new AsyncCommand(OnButtonThreeClick, CanButtonThreeClick);
        }

        public RelayCommand ButtonOneClick { get; private set; }

        private bool CanButtonOneClick()
        {
            return true;
        }

        private void OnButtonOneClick()
        {
            this.ShowMessage("您点击了第一个按钮！同时您赋予了第二个按钮点击的能力！");
            _buttonTwoCanClick = true;
        }

        public RelayCommand ButtonTwoClick { get; private set; }

        private bool CanButtonTwoClick()
        {
            return _buttonTwoCanClick;
        }

        private void OnButtonTwoClick()
        {
            this.ShowMessage("您点击了第二个按钮！同时第二个按钮丧失了被点击的能力！");
            _buttonTwoCanClick = false;
        }

        public AsyncCommand ButtonThreeClick { get; private set; }

        private async Task<bool> CanButtonThreeClick()
        {
            await Task.Delay(5000);
            return true;
        }

        private void OnButtonThreeClick()
        {
        }
    }
}