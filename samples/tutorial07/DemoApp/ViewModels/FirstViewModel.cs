using System.Threading.Tasks;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            ButtonCmd = new RelayCommand(OnButtonCmd, CanButtonCmd);
            ButtonCmdAsync = new AsyncCommand(OnButtonCmdAsync, CanButtonCmdAsync);
            GlobalCommand = Mg.Get<IMgCommand>().GetCommandInfo("AlertMessage").Command as RelayCommand;
            GlobalCommandAsync = Mg.Get<IMgCommand>().GetCommandInfo("AlertMessageAsync").Command as AsyncCommand;
        }

        public AsyncCommand GlobalCommandAsync { get; private set; }
        public RelayCommand GlobalCommand { get; private set; }

        public RelayCommand ButtonCmd { get; private set; }

        private bool CanButtonCmd()
        {
            return true;
        }

        private void OnButtonCmd()
        {
            IsChecked = !IsChecked;
        }

        public AsyncCommand ButtonCmdAsync { get; private set; }

        private int _index;

        public async Task<bool> CanButtonCmdAsync()
        {
            await Task.Delay(1000);
            _index++;
            return _index % 2 == 0;
        }

        private void OnButtonCmdAsync()
        {
            IsChecked = !IsChecked;
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set { Set("IsChecked", ref _isChecked, value); }
        }
    }
}