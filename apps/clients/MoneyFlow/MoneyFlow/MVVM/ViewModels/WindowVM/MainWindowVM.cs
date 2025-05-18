using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.MVVM.Models.DB_MSSQL;
using MoneyFlow.MVVM.ViewModels.BaseVM;
using MoneyFlow.Utils.Commands;
using MoneyFlow.Utils.Services.AuthorizationVerificationServices;
using MoneyFlow.Utils.Services.NavigationServices;
using MoneyFlow.Utils.Services.NavigationServices.PageNavigationsService;

namespace MoneyFlow.MVVM.ViewModels.WindowVM
{
    public class MainWindowVM : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IPageNavigationService _pageNavigationService;
        private readonly IAuthorizationVerificationService _authorizationVerificationService;

        private bool _isCheckedProfileButton;
        public bool IsCheckedProfileButton 
        { 
            get => _isCheckedProfileButton; 
            set
            {
                _isCheckedProfileButton = value;
                CheckToggleButtonActive(nameof(IsCheckedProfileButton));
                OnPropertyChanged();
            }
        }

        private bool _isCheckedFinancialJournalButton;
        public bool IsCheckedFinancialJournalButton
        {
            get => _isCheckedFinancialJournalButton;
            set
            {
                _isCheckedFinancialJournalButton = value;
                CheckToggleButtonActive(nameof(IsCheckedFinancialJournalButton));
                OnPropertyChanged();
            }
        }

        public MainWindowVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();
            _authorizationVerificationService = _serviceProvider.GetService<IAuthorizationVerificationService>();

            CurrentUser = _authorizationVerificationService.CurrentUser;
        }

        public void Update(object parameter)
        {
            
        }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _openProfilePageCommand;
        public RelayCommand OpenProfilePageCommand { get => _openProfilePageCommand ??= new(obj => { OpenProfilePage(); }); }

        private RelayCommand _openFinancialJournalPageCommand;
        public RelayCommand OpenFinancialJournalPageCommand { get => _openFinancialJournalPageCommand ??= new(obj => { OpenFinancialJournalPage(); }); }

        private void OpenProfilePage()
        {
            _pageNavigationService.NavigateTo("Profile", CurrentUser);
        }

        private void OpenFinancialJournalPage()
        {
            _pageNavigationService.NavigateTo("FinanceJournal", CurrentUser);
        }

        private void CheckToggleButtonActive(string key)
        {
            var properties = new Dictionary<string, Action<bool>>
            {
                { nameof(IsCheckedProfileButton), value => _isCheckedProfileButton = value },
                { nameof(IsCheckedFinancialJournalButton), value => _isCheckedFinancialJournalButton = value }
            };

            foreach (var propertyKey in properties.Keys)
            {
                properties[propertyKey](propertyKey == key);
            }

            // Уведомляем о смене значений
            OnPropertyChanged(nameof(IsCheckedProfileButton));
            OnPropertyChanged(nameof(IsCheckedFinancialJournalButton));
        }
    }
}
