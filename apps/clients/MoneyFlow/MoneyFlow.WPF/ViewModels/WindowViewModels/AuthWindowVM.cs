using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.WindowViewModels
{
    internal class AuthWindowVM : BaseViewModel ,IUpdatable
    {
        private readonly Lazy<INavigationWindows> _navigationWindows;

        private readonly IAuthorizationService _authorizationService;
        private readonly IRegistrationService _registrationService;
        private readonly IRecoveryService _recoveryService;

        public AuthWindowVM(Lazy<INavigationWindows> navigationWindows, 
                            IAuthorizationService authorizationService,
                            IRegistrationService registrationService,
                            IRecoveryService recoveryService)
        {
            _navigationWindows = navigationWindows;

            _authorizationService = authorizationService;
            _registrationService = registrationService;
            _recoveryService = recoveryService;

            //Auth();
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            
        }

// -----------------------------------------------------------------------------------------------------------------------

        #region Текущее отображение

        private AuthenticationType _currentAuthorizationType;
        public AuthenticationType CurrentAuthorizationType
        {
            get => _currentAuthorizationType;
            set
            {
                _currentAuthorizationType = value;
                OnPropertyChanged();
            }
        }

        #endregion

// -----------------------------------------------------------------------------------------------------------------------

        #region Вход

        private string _loginAuth = "styx";
        public string LoginAuth
        {
            get => _loginAuth;
            set
            {
                _loginAuth = value;
                OnPropertyChanged();
            }
        }

        private string _passwordAuth = "styx";
        public string PasswordAuth
        {
            get => _passwordAuth;
            set
            {
                _passwordAuth = value;
                OnPropertyChanged();
            }
        }

        private bool _isRememberLoginPassword;
        public bool IsRememberLoginPassword
        {
            get => _isRememberLoginPassword;
            set
            {
                _isRememberLoginPassword = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _authCommand;
        public RelayCommand AuthCommand { get => _authCommand ??= new(obj => { Auth(); }); }

        private async void Auth()
        {
            Action action = CurrentAuthorizationType == AuthenticationType.Auth ?
                async () =>
                {
                    var (UserDTO, Message) = await _authorizationService.AuthAsync(LoginAuth, PasswordAuth);

                    if (UserDTO == null)
                    {
                        MessageBox.Show(Message);
                        return;
                    }

                    _navigationWindows.Value.OpenWindow(WindowType.MainWindow);
                    _navigationWindows.Value.CloseWindow(WindowType.AuthWindow);
                }
            : () => CurrentAuthorizationType = AuthenticationType.Auth;

            action.Invoke();
        }

        #endregion

// -----------------------------------------------------------------------------------------------------------------------

        #region Регистрация

        //private string _emailRegistration;
        //public string EmailRegistration
        //{
        //    get => _emailRegistration;
        //    set
        //    {
        //        _emailRegistration = value;
        //        OnPropertyChanged();
        //    }
        //}

        private string _userNameRegistration;
        public string UserNameRegistration
        {
            get => _userNameRegistration;
            set
            {
                _userNameRegistration = value;
                OnPropertyChanged();
            }
        }

        private string _loginRegistration;
        public string LoginRegistration
        {
            get => _loginRegistration;
            set
            {
                _loginRegistration = value;
                OnPropertyChanged();
            }
        }

        private string _passwordRegistration;
        public string PasswordRegistration
        {
            get => _passwordRegistration;
            set
            {
                _passwordRegistration = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _registrationCommand;
        public RelayCommand RegistrationCommand { get => _registrationCommand ??= new(async obj => { await Registration(); }); }

        private async Task Registration()
        {
            Action action = CurrentAuthorizationType == AuthenticationType.Registration ?
                async () =>
                {
                    var (UserDTO, Message) = await _registrationService.RegistrationAsync(UserNameRegistration, LoginRegistration, PasswordRegistration);

                    if (UserDTO == null)
                    {
                        MessageBox.Show(Message);
                        return;
                    }

                    await _authorizationService.AuthAsync(UserDTO.Login, UserDTO.Password);
                    _navigationWindows.Value.OpenWindow(WindowType.MainWindow);
                    _navigationWindows.Value.CloseWindow(WindowType.AuthWindow);
                }
            : () => CurrentAuthorizationType = AuthenticationType.Registration;

            action.Invoke();
        }

        #endregion

// -----------------------------------------------------------------------------------------------------------------------

        #region Восстановление пароля

        private string _loginRecovery;
        public string LoginRecovery
        {
            get => _loginRecovery;
            set
            {
                _loginRecovery = value;
                OnPropertyChanged();
            }
        }

        private string _codeVerificationRecovery;
        public string CodeVerificationRecovery
        {
            get => _codeVerificationRecovery;
            set
            {
                _codeVerificationRecovery = value;
                OnPropertyChanged();
            }
        }

        private string _newPasswordRecovery;
        public string NewPasswordRecovery
        {
            get => _newPasswordRecovery;
            set
            {
                _newPasswordRecovery = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _recoveryCommand;
        public RelayCommand RecoveryCommand { get => _recoveryCommand ??= new(obj => { Recovery(); }); }

        private async void Recovery()
        {
            Action action = CurrentAuthorizationType == AuthenticationType.RecoverPassword ?
                async () =>
                {
                    var (UserDTO, Message) = await _recoveryService.RecoveryAsync(LoginRecovery, NewPasswordRecovery);

                    if (UserDTO == null)
                    {
                        MessageBox.Show(Message);
                        return;
                    }

                    MessageBox.Show(Message);

                    LoginRecovery = string.Empty;
                    CodeVerificationRecovery = string.Empty;
                    NewPasswordRecovery = string.Empty;
                }
                : () => CurrentAuthorizationType = AuthenticationType.RecoverPassword;

            action.Invoke();
        }

        #endregion

// -----------------------------------------------------------------------------------------------------------------------

        #region Test

        private RelayCommand _open;
        public RelayCommand Open 
        { 
            get => _open ??= new(obj => 
            { 
                _navigationWindows.Value.OpenWindow(WindowType.MainWindow);
                _navigationWindows.Value.CloseWindow(WindowType.AuthWindow);
            });
        }

        #endregion
    }
}
