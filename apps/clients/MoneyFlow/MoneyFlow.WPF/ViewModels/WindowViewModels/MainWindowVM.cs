using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;

namespace MoneyFlow.WPF.ViewModels.WindowViewModels
{
    internal class MainWindowVM : BaseViewModel ,IUpdatable
    {
        private readonly INavigationPages _navigationPages;
        private readonly INavigationWindows _navigationWindows;

        public MainWindowVM(INavigationPages navigationPages, INavigationWindows navigationWindows)
        {
            _navigationPages = navigationPages;
            _navigationWindows = navigationWindows;
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            
        }


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Навигация

        private RelayCommand _openAddBaseInformationWindowCommand;
        public RelayCommand OpenAddBaseInformationWindowCommand
        {
            get => _openAddBaseInformationWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.AddBaseInformationWindow);
            });
        }

        private RelayCommand _openAccountWindowCommand;
        public RelayCommand OpenAccountWindowCommand
        {
            get => _openAccountWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.AccountWindow);
            });
        }

        private RelayCommand _openAccountTypeWindowCommand;
        public RelayCommand OpenAccountTypeWindowCommand
        {
            get => _openAccountTypeWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.AccountTypeWindow);
            });
        }

        private RelayCommand _openBankWindowCommand;
        public RelayCommand OpenBankWindowCommand
        {
            get => _openBankWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.BankWindow);
            });
        }

        private RelayCommand _openCatAndSubWindowCommand;
        public RelayCommand OpenCatAndSubWindowCommand
        {
            get => _openCatAndSubWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.CatAndSubWindow);
            });
        }

        private RelayCommand _openFinancialRecordWindowCommand;
        public RelayCommand OpenFinancialRecordWindowCommand
        {
            get => _openFinancialRecordWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.FinancialRecordWindow);
            });
        }

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------
    }
}
