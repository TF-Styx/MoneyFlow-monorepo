using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;

namespace MoneyFlow.WPF.ViewModels.PageViewModels
{
    internal class AccountTypePageVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountTypeService _accountTypeService;

        private readonly INavigationPages _navigationPages;

        public AccountTypePageVM(IAuthorizationService authorizationService, IAccountTypeService accountTypeService, INavigationPages navigationPages)
        {
            _authorizationService = authorizationService;
            _accountTypeService = accountTypeService;

            _navigationPages = navigationPages;

            CurrentUser = _authorizationService.CurrentUser;

            GetUserAccountTypes();
            GetAccountType();
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is AccountTypeDTO accountType)
            {
                AccountTypeName = accountType.AccountTypeName;
            }
        }

        private UserDTO _currentUser;
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }


        #region Тип счета пользователя

        private string _accountTypeName;
        public string AccountTypeName
        {
            get => _accountTypeName;
            set
            {
                _accountTypeName = value;
                OnPropertyChanged();
            }
        }

        private UserAccountTypesDTO _userAccountTypes;
        public UserAccountTypesDTO UserAccountTypes
        {
            get => _userAccountTypes;
            set
            {
                _userAccountTypes = value;
                OnPropertyChanged();
            }
        }

        private AccountTypeDTO _selectedUserAccountType;
        public AccountTypeDTO SelectedUserAccountType
        {
            get => _selectedUserAccountType;
            set
            {
                _selectedUserAccountType = value;

                AccountTypeName = value.AccountTypeName;

                OnPropertyChanged();
            }
        }

        private async Task GetUserAccountTypes()
        {
            UserAccountTypes = await _accountTypeService.GetByIdUserAsync(CurrentUser.IdUser);
        }

        #endregion


        #region Тип счета

        private AccountTypeDTO _selectedAccountType;
        public AccountTypeDTO SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                _selectedAccountType = value;

                AccountTypeName = value.AccountTypeName;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccountTypeDTO> AccountTypes { get; set; } = [];
        private async void GetAccountType()
        {
            AccountTypes.Clear();

            var list = await _accountTypeService.GetAllAsync();

            foreach (var item in list)
            {
                AccountTypes.Add(item);
                var index = AccountTypes.IndexOf(item);
                item.Index = index + 1;
            }
        }

        #endregion


        #region Навигация

        private RelayCommand _openProfileUserPageCommand;
        public RelayCommand OpenProfileUserPageCommand
        {
            get => _openProfileUserPageCommand ??= new(obj =>
            {
                _navigationPages.OpenPage(PageType.UserPage, FrameType.MainFrame);
            });
        }

        #endregion
    }
}
