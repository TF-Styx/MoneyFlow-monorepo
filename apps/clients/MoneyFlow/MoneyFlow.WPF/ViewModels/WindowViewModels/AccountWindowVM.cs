using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.WindowViewModels
{
    class AccountWindowVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountService _accountService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IBankService _bankService;

        private readonly INavigationPages _navigationPages;

        public AccountWindowVM(IAuthorizationService authorizationService,
                               IAccountService accountService,
                               IAccountTypeService accountTypeService,
                               IBankService bankService,
                               INavigationPages navigationPages)
        {
            _authorizationService = authorizationService;
            _accountService = accountService;
            _accountTypeService = accountTypeService;
            _bankService = bankService;

            _navigationPages = navigationPages;

            CurrentUser = _authorizationService.CurrentUser;

            GetAccount();
            GetBank();
            GetAccountType();
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is AccountDTO account)
            {
                SelectedAccount = account;

                NumberAccount = account.NumberAccount;
                Balance = account.Balance;
                SelectedBank = Banks.FirstOrDefault(x => x.IdBank == account.Bank.IdBank);
                SelectedAccountType = AccountTypes.FirstOrDefault(x => x.IdAccountType == account.AccountType.IdAccountType);
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


        #region Счета пользователя

        private int? _numberAccount;
        public int? NumberAccount
        {
            get => _numberAccount;
            set
            {
                _numberAccount = value;
                OnPropertyChanged();
            }
        }

        private decimal? _balance;
        public decimal? Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        private AccountDTO _selectedAccount;
        public AccountDTO SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;

                if (value == null) { return; }

                NumberAccount = value.NumberAccount;
                Balance = value.Balance;
                SelectedBank = Banks.FirstOrDefault(x => x.IdBank == value.Bank.IdBank);
                SelectedAccountType = AccountTypes.FirstOrDefault(x => x.IdAccountType == value.AccountType.IdAccountType);

                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccountDTO> Accounts { get; set; } = [];
        private async void GetAccount()
        {
            Accounts.Clear();

            var list = await _accountService.GetAllAsync(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Accounts.Add(item);
                var index = Accounts.IndexOf(item);
                item.Index = index + 1;
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Банки

        private BankDTO _selectedBank;
        public BankDTO SelectedBank
        {
            get => _selectedBank;
            set
            {
                _selectedBank = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BankDTO> Banks { get; set; } = [];
        private void GetBank()
        {
            Banks.Clear();

            var list = _bankService.GetAll();

            foreach (var item in list)
            {
                Banks.Add(item);
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Тип счета

        private AccountTypeDTO _selectedAccountType;
        public AccountTypeDTO SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                _selectedAccountType = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccountTypeDTO> AccountTypes { get; set; } = [];
        private void GetAccountType()
        {
            AccountTypes.Clear();

            var list = _accountTypeService.GetAll();

            foreach (var item in list)
            {
                AccountTypes.Add(item);
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Команды

        private RelayCommand _accountAddCommand;
        public RelayCommand AccountAddCommand
        {
            get => _accountAddCommand ??= new(async obj =>
            {
                var newAccount = await _accountService.CreateAsync(NumberAccount, CurrentUser.IdUser, SelectedBank, SelectedAccountType, Balance);

                if (newAccount.Message != string.Empty)
                {
                    MessageBox.Show(newAccount.Message);
                    return;
                }

                Accounts.Add(newAccount.AccountDTO);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (newAccount.AccountDTO, SelectedBank), ParameterType.Add);
            });
        }

        private RelayCommand _accountUpdateCommand;
        public RelayCommand AccountUpdateCommand
        {
            get => _accountUpdateCommand ??= new(async obj =>
            {
                var idUpdateAccount = await _accountService.UpdateAsync
                    (
                        SelectedAccount.IdAccount,
                        NumberAccount,
                        SelectedBank,
                        SelectedAccountType,
                        Balance
                    );

                var updateAccount = Accounts
                    .FirstOrDefault(x => x.IdAccount == SelectedAccount.IdAccount)
                        .SetProperty(x =>
                        {
                            x.IdAccount = idUpdateAccount;
                            x.NumberAccount = NumberAccount;
                            x.Bank = SelectedBank;
                            x.AccountType = SelectedAccountType;
                            x.Balance = Balance;
                        });

                var index = Accounts.IndexOf(updateAccount);

                Accounts.RemoveAt(index);
                Accounts.Insert(index, updateAccount);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (updateAccount, SelectedBank), ParameterType.Update);
            });
        }

        private RelayCommand _accountDeleteCommand;
        public RelayCommand AccountDeleteCommand
        {
            get => _accountDeleteCommand ??= new(async obj =>
            {
                await _accountService.DeleteAsync(SelectedAccount.IdAccount);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (SelectedAccount, SelectedBank), ParameterType.Delete);

                NumberAccount = 0;
                SelectedBank = null;
                SelectedAccountType = null;
                Balance = 0;

                Accounts.Remove(SelectedAccount);
            });
        }

        #endregion
    }
}
