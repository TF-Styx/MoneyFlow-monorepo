using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;

namespace MoneyFlow.WPF.ViewModels.PageViewModels
{
    internal class BankPageVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IBankService _bankService;

        private readonly INavigationPages _navigationPages;

        public BankPageVM(IAuthorizationService authorizationService, IBankService bankService, INavigationPages navigationPages)
        {
            _authorizationService = authorizationService;
            _bankService = bankService;

            _navigationPages = navigationPages;

            CurrentUser = _authorizationService.CurrentUser;

            GetBank();
            GetUserBanks();
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is BankDTO bank)
            {
                BankName = bank.BankName;
            }

            if (parameter is BankDTO bankAdd && typeParameter is ParameterType.Add)
            {
                Banks.Add(bankAdd);
            }
            if (parameter is BankDTO bankUpdate && typeParameter is ParameterType.Update)
            {
                var itemForDelete = Banks.FirstOrDefault(x => x.IdBank == bankUpdate.IdBank);
                var index = Banks.IndexOf(itemForDelete);

                Banks.Remove(itemForDelete);
                Banks.Insert(index, itemForDelete);
            }
            if (parameter is BankDTO bankDelete && typeParameter is ParameterType.Delete)
            {
                Banks.Remove(Banks.FirstOrDefault(x => x.IdBank == bankDelete.IdBank));
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

        private string _bankName;
        public string BankName
        {
            get => _bankName;
            set
            {
                _bankName = value;
                OnPropertyChanged();
            }
        }

        private UserBanksDTO _userBanks;
        public UserBanksDTO UserBanks
        {
            get => _userBanks;
            set
            {
                _userBanks = value;
                OnPropertyChanged();
            }
        }

        private BankDTO _selectedUserBank;
        public BankDTO SelectedUserBank
        {
            get => _selectedUserBank;
            set
            {
                _selectedUserBank = value;

                BankName = value.BankName;

                OnPropertyChanged();
            }
        }

        private async Task GetUserBanks()
        {
            UserBanks = await _bankService.GetByIdUserAsync(CurrentUser.IdUser);
        }


        private BankDTO _selectedBank;
        public BankDTO SelectedBank
        {
            get => _selectedBank;
            set
            {
                _selectedBank = value;

                BankName = value.BankName;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<BankDTO> Banks { get; set; } = [];
        private async void GetBank()
        {
            Banks.Clear();

            var list = await _bankService.GetAllAsync();

            foreach (var item in list)
            {
                Banks.Add(item);
                var index = Banks.IndexOf(item);
                item.Index = index + 1;
            }
        }


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
