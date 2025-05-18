using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using MoneyFlow.MVVM.Models.DB_MSSQL;
using MoneyFlow.MVVM.ViewModels.BaseVM;
using MoneyFlow.Utils.Commands;
using MoneyFlow.Utils.Helpers;
using MoneyFlow.Utils.Services.AuthorizationVerificationServices;
using MoneyFlow.Utils.Services.DataBaseServices;
using MoneyFlow.Utils.Services.NavigationServices;
using MoneyFlow.Utils.Services.NavigationServices.WindowNavigationsService;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.MVVM.ViewModels.PageVM
{
    class FinancialJournalPageVM : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IAuthorizationVerificationService _authorizationVerificationService;
        private readonly IDataBaseService _dataBaseService;
        private readonly IWindowNavigationService _windowNavigationService;

        private readonly LastRecordHelper _lastRecordHelper;

        public FinancialJournalPageVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _authorizationVerificationService = _serviceProvider.GetService<IAuthorizationVerificationService>();
            _dataBaseService = _serviceProvider.GetService<IDataBaseService>();
            _windowNavigationService = _serviceProvider.GetService<IWindowNavigationService>();

            _lastRecordHelper = _serviceProvider.GetRequiredService<LastRecordHelper>();

            GetFinancialRecordData(_authorizationVerificationService.CurrentUser);
        }

        public void Update(object parameter)
        {
            CurrentUser = _authorizationVerificationService.CurrentUser;

            SelectedFinancialRecord = null;

            if (parameter == null)
            {
                //FinancialRecords.Add(_lastRecordHelper.LastRecordFinancialRecord(CurrentUser));
            }
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

        private FinancialRecord _selectedFinancialRecord;
        public FinancialRecord SelectedFinancialRecord
        {
            get => _selectedFinancialRecord;
            set
            {
                _selectedFinancialRecord = value;

                //if (value != null)
                //{
                //    RecordName = value.RecordName;
                //    Amount = value.Amount;
                //    Description = value.Description;
                //    Date = value.Date;
                //    SelectedCategory = Categories.FirstOrDefault(x => x.IdCategory == value.IdCategory);
                //    SelectedSubCategory = Subcategories.FirstOrDefault(x => x.IdSubcategory == value.IdSubcategory);
                //}
                //else
                //{
                //    RecordName = string.Empty;
                //    Amount = decimal.Zero;
                //    Description = string.Empty;
                //    Date = DateTime.Now;
                //    SelectedCategory = null;
                //    SelectedSubCategory = null;
                //}

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Account> Accounts { get; set; } = [];
        public ObservableCollection<FinancialRecord> FinancialRecords { get; set; } = [];


        private RelayCommand _openFinancialRecordAddCommand;
        public RelayCommand OpenFinancialRecordAddCommand { get => _openFinancialRecordAddCommand ??= new(obj => { OpenFinancialRecordAdd(); }); }

        private RelayCommand _itemDoubleClickCommand;
        public RelayCommand ItemDoubleClickCommand => _itemDoubleClickCommand ??= new RelayCommand(DoubleClick);

        private void DoubleClick(object parameter)
        {
            if (parameter is FinancialRecord financialRecord)
            {
                _windowNavigationService.NavigateTo("FinancialRecordAdd", financialRecord);
            }
        }

        private async void GetFinancialRecordData(User user)
        {
            FinancialRecords.Clear();

            var financialRecordData = 
                await _dataBaseService.GetDataTableAsync<FinancialRecord>(x => x
                            .Include(x => x.IdAccountNavigation.IdBankNavigation)
                            .Include(x => x.IdAccountNavigation.IdAccountTypeNavigation)
                            //.Include(x => x.IdCategoryNavigation.IdSubcategoryNavigation)
                                .Where(x => x.IdUser == user.IdUser));

            foreach (var item in financialRecordData)
            {
                FinancialRecords.Add(item);
            }
        }

        private void OpenFinancialRecordAdd()
        {
            _windowNavigationService.NavigateTo("FinancialRecordAdd", CurrentUser);
        }
    }
}
