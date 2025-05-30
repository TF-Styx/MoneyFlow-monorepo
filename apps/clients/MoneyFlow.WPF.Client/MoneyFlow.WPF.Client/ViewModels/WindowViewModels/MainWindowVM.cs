using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.WPF.Client.Enums;
using MoneyFlow.WPF.Client.Services.Interfaces;

namespace MoneyFlow.WPF.Client.ViewModels.WindowViewModels
{
    public class MainWindowVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly INavigationWindow _navigationWindow;

        public MainWindowVM(IAuthorizationService authorizationService, INavigationWindow navigationWindow)
        {
            _authorizationService = authorizationService;
            CurrentUser = _authorizationService.CurrentUser!;

            _navigationWindow = navigationWindow;
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            
        }

        private UserDTO _currentUser { get; set; } = null!;
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }
    }
}
