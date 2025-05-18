using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.WindowViewModels
{
    internal class AddBaseInformationVM : BaseViewModel, IUpdatable
    {
        private readonly IBankService _bankService;
        private readonly IGenderService _genderService;

        private readonly INavigationPages _navigationPages;

        public AddBaseInformationVM(IBankService bankService, IGenderService genderService, INavigationPages navigationPages)
        {
            _bankService = bankService;
            _genderService = genderService;
            GetBanks();
            GetGenders();
            _navigationPages = navigationPages;
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            
        }


        #region Банк || string:BankName || Observable:Banks || BankDTO:SeletedBank

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


        public ObservableCollection<BankDTO> Banks { get; set; } = [];

        private async void GetBanks()
        {
            Banks.Clear();

            var list = await _bankService.GetAllAsync();

            foreach (var item in list)
            {
                Banks.Add(item);
            }
        }

        private BankDTO _selectedBank;
        public BankDTO SelectedBank
        {
            get => _selectedBank;
            set
            {
                _selectedBank = value;

                if (value == null) { BankName = string.Empty; return; }

                BankName = value.BankName;

                OnPropertyChanged();
            }
        }


        private RelayCommand _bankAddCommand;
        public RelayCommand BankAddCommand
        {
            get => _bankAddCommand ??= new(async obj =>
            {
                var newBank = await _bankService.CreateAsync(BankName);
                if (newBank.Message != string.Empty)
                {
                    MessageBox.Show(newBank.Message);
                    return;
                }

                Banks.Add(newBank.BankDTO);

                _navigationPages.TransitObject(PageType.BankPage, FrameType.MainFrame, newBank.BankDTO, ParameterType.Add);
            });
        }

        private RelayCommand _bankUpdateCommand;
        public RelayCommand BankUpdateCommand
        {
            get => _bankUpdateCommand ??= new(async obj =>
            {
                var idUpdatableBank = await _bankService.UpdateAsync(SelectedBank.IdBank, BankName);
                var updatableBank = Banks.FirstOrDefault(x => x.IdBank == SelectedBank.IdBank)
                                         .SetProperty(x => { x.IdBank = idUpdatableBank; x.BankName = BankName; });
                var index = Banks.IndexOf(updatableBank);

                Banks.RemoveAt(index);
                Banks.Insert(index, updatableBank);

                _navigationPages.TransitObject(PageType.BankPage, FrameType.MainFrame, updatableBank, ParameterType.Update);
            });
        }

        private RelayCommand _bankDeleteCommand;
        public RelayCommand BankDeleteCommand
        {
            get => _bankDeleteCommand ??= new(async obj =>
            {
                await _bankService.DeleteAsync(SelectedBank.IdBank);

                _navigationPages.TransitObject(PageType.BankPage, FrameType.MainFrame, SelectedBank, ParameterType.Delete);

                BankName = string.Empty;

                Banks.Remove(SelectedBank);
            });
        }

        #endregion


        #region Gender || string:GenderName || Observable:Genders || GenderDTO:SeletedGender

        private string _genderName;
        public string GenderName
        {
            get => _genderName;
            set
            {
                _genderName = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<GenderDTO> Genders { get; set; } = [];

        private async void GetGenders()
        {
            Genders.Clear();

            var list = await _genderService.GetAllAsyncGender();

            foreach (var item in list)
            {
                if (item == null)
                {
                    continue;
                }

                Genders.Add(item);
            }
        }

        private GenderDTO _selectedGender;
        public GenderDTO SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;

                if (value == null) { GenderName = string.Empty; return; }

                GenderName = value.GenderName;

                OnPropertyChanged();
            }
        }


        private RelayCommand _GenderAddCommand;
        public RelayCommand GenderAddCommand
        {
            get => _GenderAddCommand ??= new(async obj =>
            {
                var newGender = await _genderService.CreateAsyncGender(GenderName);
                if (newGender.Message != string.Empty)
                {
                    MessageBox.Show(newGender.Message);
                    return;
                }

                Genders.Add(newGender.GenderDTO);
            });
        }

        private RelayCommand _genderUpdateCommand;
        public RelayCommand GenderUpdateCommand
        {
            get => _genderUpdateCommand ??= new(async obj =>
            {
                var idUpdatableGender = await _genderService.UpdateAsyncGender(SelectedGender.IdGender, GenderName);
                var updatableGender = Genders.FirstOrDefault(x => x.IdGender == SelectedGender.IdGender)
                                             .SetProperty(x => { x.IdGender = idUpdatableGender; x.GenderName = GenderName; });
                var index = Genders.IndexOf(updatableGender);

                Genders.RemoveAt(index);
                Genders.Insert(index, updatableGender);
            });
        }

        private RelayCommand _GenderDeleteCommand;
        public RelayCommand GenderDeleteCommand
        {
            get => _GenderDeleteCommand ??= new(async obj =>
            {
                await _genderService.DeleteAsyncGender(SelectedGender.IdGender);
                Genders.Remove(SelectedGender);
            });
        }

        #endregion
    }
}