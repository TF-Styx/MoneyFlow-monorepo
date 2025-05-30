using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCases.Abstraction.GenderUseCases;
using MoneyFlow.Domain.Results;
using MoneyFlow.WPF.Client.Command;
using MoneyFlow.WPF.Client.Enums;
using MoneyFlow.WPF.Client.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.Client.ViewModels.WindowViewModels
{
    public class AuthWindowVM : BaseViewModel
    {
        private readonly INavigationWindow _navigationWindow;

        private readonly IAuthorizationService _authorizationService;
        private readonly IGetAllGenderUseCase _getAllGenderUseCase;

        public AuthWindowVM(INavigationWindow navigationWindow, IAuthorizationService authorizationService, IGetAllGenderUseCase getAllGenderUseCase)
        {
            SettingValuesInCommands();
            CurrentControl = Controls.AuthUserControl;

            _navigationWindow = navigationWindow;

            _authorizationService = authorizationService;
            _getAllGenderUseCase = getAllGenderUseCase;
        }

        #region --- Текущий отображаемый UserControl ---
        private Controls _currentControl;
        public Controls CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;

                AuthCurrentControlVisibilityCommand?.RaiseCanExecuteChanged();
                RegistrationCurrentControlVisibilityCommand?.RaiseCanExecuteChanged();

                AuthenticateUserCommand?.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }
        #endregion -------------------------------------

        #region --- Вход ---

        private string? _authLogin;
        public string? AuthLogin
        {
            get => _authLogin;
            set
            {
                _authLogin = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string? _authPassword;
        public string? AuthPassword
        {
            get => _authPassword;
            set
            {
                _authPassword = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        #endregion ---------

        #region --- Регистрация ---

        private string? _registrationUserName;
        public string? RegistrationUserName
        {
            get => _registrationUserName;
            set
            {
                _registrationUserName = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string? _registrationEmail;
        public string? RegistrationEmail
        {
            get => _registrationEmail;
            set
            {
                _registrationEmail = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string? _registrationLogin;
        public string? RegistrationLogin
        {
            get => _registrationLogin;
            set
            {
                _registrationLogin = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string? _registrationPassword;
        public string? RegistrationPassword
        {
            get => _registrationPassword;
            set
            {
                _registrationPassword = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string? _RegistrationPhone;
        public string? RegistrationPhone
        {
            get => _RegistrationPhone;
            set
            {
                _RegistrationPhone = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private GenderDTO _selectedGender;
        public GenderDTO SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GenderDTO> Genders { get; set; } = [];

        private async Task GetAllGenders()
        {
            Genders.Clear();

            var genders = await _getAllGenderUseCase.GetAllAsync();

            foreach (var item in genders.Value!)
            {
                Genders.Add(item);
            }
        }

        #endregion ----------------

        #region --- Востановление доступа / Смена пароля ---

        private string? _recoveryAccessEmail;
        public string? RecoveryAccessEmail
        {
            get => _recoveryAccessEmail;
            set
            {
                _recoveryAccessEmail = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string? _recoveryAccessLogin;
        public string? RecoveryAccessLogin
        {
            get => _recoveryAccessLogin;
            set
            {
                _recoveryAccessLogin = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string? _recoveryAccessNewPassword;
        public string? RecoveryAccessNewPassword
        {
            get => _recoveryAccessNewPassword;
            set
            {
                _recoveryAccessNewPassword = value;
                AuthenticateUserCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        #endregion -----------------------------------------

        #region --- Установка значений в команды ---

        private void SettingValuesInCommands()
        {
            AuthCurrentControlVisibilityCommand = new RelayCommand<Controls>(Execute_AuthCurrentCurrentControlVisibilityCommand, CanExecute_AuthCurrentControlVisibilityCommand);
            RegistrationCurrentControlVisibilityCommand = new RelayCommand<Controls>(Execute_RegistrationCurrentControlVisibilityCommand, CanExecute_RegistrationCurrentControlVisibilityCommand);
            RecoveryAccessCurrentControlVisibilityCommand = new RelayCommand<Controls>(Execute_RecoveryAccessCurrentCurrentControlVisibilityCommand, CanExecute_RecoveryAccessCurrentControlVisibilityCommand);

            AuthenticateUserCommand = new RelayCommand(Execute_AuthenticateUserCommand, CanExecute_AuthenticateUserCommand);
        }

        #endregion ---------------------------------

        #region --- Команда отображения контрола авторизации ---

        public RelayCommand<Controls>? AuthCurrentControlVisibilityCommand { get; private set; }
        private void Execute_AuthCurrentCurrentControlVisibilityCommand(Controls controls) => CurrentControl = controls;
        private bool CanExecute_AuthCurrentControlVisibilityCommand(Controls controls) => CurrentControl != Controls.AuthUserControl;

        #endregion ---------------------------------------------

        #region --- Команда отображения контрола регистрации ---

        public RelayCommand<Controls>? RegistrationCurrentControlVisibilityCommand { get; private set; }
        private async void Execute_RegistrationCurrentControlVisibilityCommand(Controls controls)
        {
            CurrentControl = controls;

            if (Genders.Count() == 0)
                await GetAllGenders();
        }
        private bool CanExecute_RegistrationCurrentControlVisibilityCommand(Controls controls) => CurrentControl != Controls.RegistrationUserControl;

        #endregion ---------------------------------------------

        #region --- Команда отображения контрола востановление пароля ---

        public RelayCommand<Controls>? RecoveryAccessCurrentControlVisibilityCommand { get; private set; }
        private void Execute_RecoveryAccessCurrentCurrentControlVisibilityCommand(Controls controls) => CurrentControl = controls;
        private bool CanExecute_RecoveryAccessCurrentControlVisibilityCommand(Controls controls) => CurrentControl != Controls.RecoveryAccessUserControl;

        #endregion ------------------------------------------------------

        #region --- Команды авторизации пользователя ---

        public RelayCommand AuthenticateUserCommand { get; private set; }
        private async void Execute_AuthenticateUserCommand()
        {
            Action action = CurrentControl switch
            {
                Controls.AuthUserControl => async () =>
                {
                    var result = await _authorizationService.AuthenticateAsync(AuthLogin!, AuthPassword!);

                    if (result.Success)
                    {
                        _navigationWindow.OpenWindow(WindowName.MainWindow);
                        _navigationWindow.CloseWindow(WindowName.AuthWindow);
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка: {string.Join(';', result.ErrorDetails)}");
                    }
                },

                Controls.RegistrationUserControl => async () =>
                {
                    Result<(string Login, string Password)?> result = await _authorizationService.RegistrationAsync(RegistrationUserName, RegistrationEmail, RegistrationLogin, RegistrationPassword, SelectedGender.IdGender, RegistrationPhone);

                    if (result.Success)
                    {
                        CurrentControl = Controls.AuthUserControl;

                        if (AuthLogin != result.Value!.Value.Login)
                            AuthLogin = result.Value!.Value.Login;
                        if (AuthPassword != result.Value!.Value.Password)
                            AuthPassword = result.Value!.Value.Password;
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка: {string.Join(';', result.ErrorDetails)}");
                    }
                },

                Controls.RecoveryAccessUserControl => async () =>
                {
                    Result<(string Login, string NewPassword)?> result = await _authorizationService.RecoveryAccessAsync(RecoveryAccessEmail, RecoveryAccessLogin, RecoveryAccessNewPassword);

                    if (result.Success)
                    {
                        CurrentControl = Controls.AuthUserControl;

                        if (AuthLogin != result.Value!.Value.Login)
                            AuthLogin = result.Value!.Value.Login;
                        if (AuthPassword != result.Value!.Value.NewPassword)
                            AuthPassword = result.Value!.Value.NewPassword;
                    }
                },

                _ => () => throw new Exception()
            };
            action.Invoke();
        }
        private bool CanExecute_AuthenticateUserCommand()
        {
            return CurrentControl switch
            {
                Controls.AuthUserControl => !string.IsNullOrWhiteSpace(AuthLogin) &&
                                            !string.IsNullOrWhiteSpace(AuthPassword),

                Controls.RegistrationUserControl => !string.IsNullOrWhiteSpace(RegistrationUserName) &&
                                                    !string.IsNullOrWhiteSpace(RegistrationEmail) &&
                                                    !string.IsNullOrWhiteSpace(RegistrationLogin) &&
                                                    !string.IsNullOrWhiteSpace(RegistrationPassword) &&
                                                    SelectedGender != null,

                Controls.RecoveryAccessUserControl => !string.IsNullOrWhiteSpace(RecoveryAccessEmail) &&
                                                      !string.IsNullOrWhiteSpace(RecoveryAccessLogin) &&
                                                      !string.IsNullOrWhiteSpace(RecoveryAccessNewPassword),

                _ => throw new Exception()
            };
        }

        #endregion -------------------------------------
    }
}
