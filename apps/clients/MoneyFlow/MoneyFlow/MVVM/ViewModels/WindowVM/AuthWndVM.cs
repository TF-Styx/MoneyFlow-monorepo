using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.MVVM.Models.DB_MSSQL;
using MoneyFlow.MVVM.ViewModels.BaseVM;
using MoneyFlow.Utils.Commands;
using MoneyFlow.Utils.Services.AuthorizationVerificationServices;
using MoneyFlow.Utils.Services.DataBaseServices;
using MoneyFlow.Utils.Services.NavigationServices;
using MoneyFlow.Utils.Services.NavigationServices.WindowNavigationsService;
using System.Windows;

namespace MoneyFlow.MVVM.ViewModels.WindowVM
{
    public class AuthWndVM : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataBaseService _dataBaseService;
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IAuthorizationVerificationService _authorizationVerificationService;

        public AuthWndVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dataBaseService = _serviceProvider.GetService<IDataBaseService>();
            _windowNavigationService = _serviceProvider.GetService<IWindowNavigationService>();
            _authorizationVerificationService = _serviceProvider.GetService<IAuthorizationVerificationService>();
        }
        public void Update(object parameter)
        {
            
        }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _isAuthenticated = true;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _authCommand;
        public RelayCommand AuthCommand { get => _authCommand ??= new(obj => { Auth(); }); }

        private RelayCommand _registrationCommand;
        public RelayCommand RegistrationCommand { get => _registrationCommand ??= new(obj => { Registration(); }); }

        private async void Auth()
        {
            if (await _dataBaseService.ExistsAsync<User>(x => x.Login == Login))
            {
                _authorizationVerificationService.CreateJsonUser(await _dataBaseService.FirstOrDefaultAsync<User>(x => x.Login == Login));

                _windowNavigationService.NavigateTo("MainWindow", _dataBaseService.FirstOrDefaultAsync<User>(x => x.Login == Login));
                _windowNavigationService.CloseWindow("AuthWnd");
            }
            else
            {
                IsAuthenticated = false;
            }
        }

        private async void Registration()
        {
            if (string.IsNullOrEmpty(Login) && string.IsNullOrEmpty(Password))
            {
                return; // TODO : Добавить визуальное отображение неверно введенных данных
            }
            if (!await _dataBaseService.ExistsAsync<User>(x => x.Login.ToLower() == Login.ToLower()))
            {
                User user = new User()
                {
                    Login = Login.ToLower(),
                    Password = Password.ToLower()
                };

                await _dataBaseService.AddAsync(user);

                _authorizationVerificationService.CreateJsonUser(await _dataBaseService.FirstOrDefaultAsync<User>(x => x.Login == Login));

                _windowNavigationService.NavigateTo("MainWindow", _dataBaseService.FirstOrDefaultAsync<User>(x => x.Login.ToLower() == Login.ToLower()));
                _windowNavigationService.CloseWindow("AuthWnd");
            }
            else
            {
                IsAuthenticated = false;
                // TODO : Добавить уведомление о занятости «Login»
            }
        }
    }
}
