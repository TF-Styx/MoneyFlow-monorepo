using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.MVVM.Models.DB_MSSQL;
using MoneyFlow.MVVM.ViewModels.BaseVM;
using MoneyFlow.Utils.Commands;
using MoneyFlow.Utils.Helpers;
using MoneyFlow.Utils.Services.AuthorizationVerificationServices;
using MoneyFlow.Utils.Services.DataBaseServices;
using MoneyFlow.Utils.Services.NavigationServices;
using MoneyFlow.Utils.Services.NavigationServices.PageNavigationsService;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.MVVM.ViewModels.WindowVM
{
    class FinancialRecordAddVM : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IAuthorizationVerificationService _authorizationVerificationService;
        private readonly IDataBaseService _dataBaseService;
        private readonly IPageNavigationService _pageNavigationService;

        public FinancialRecordAddVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _authorizationVerificationService = _serviceProvider.GetService<IAuthorizationVerificationService>();
            _dataBaseService = _serviceProvider.GetService<IDataBaseService>();
            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();

            GetTransactionTypesData();
        }

        public void Update(object parameter)
        {
            CurrentUser = _authorizationVerificationService.CurrentUser;

            if (parameter is User user)
            {
                GetAccountsData();
                GetCategoriesData();
            }

            if (parameter is FinancialRecord financialRecord)
            {
                CurrentFinancialRecord = financialRecord;

                GetAccountsData();
                GetCategoriesData();

                RecordName = financialRecord.RecordName;
                Amount = financialRecord.Ammount;
                Description = financialRecord.Description;
                Date = financialRecord.Date;
                SelectedAccount = Accounts.FirstOrDefault(x => x.IdAccount == financialRecord.IdAccount);
                SelectedTransactionType = TransactionTypes.FirstOrDefault(x => x.IdTransactionType == financialRecord.IdTransactionType);
                SelectedCategory = Categories.FirstOrDefault(x => x.IdCategory == financialRecord.IdCategory);
                SelectedSubCategory = Subcategories.FirstOrDefault(x => x.IdSubcategory == financialRecord.IdSubcategory);
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

        private string _recordName;
        public string RecordName
        {
            get => _recordName;
            set
            {
                _recordName = value;
                OnPropertyChanged();
            }
        }

        private decimal? _amount;
        public decimal? Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _date = DateTime.Now;
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
            }
        }

        private TransactionType _selectedTransactionType;
        public TransactionType SelectedTransactionType
        {
            get => _selectedTransactionType;
            set
            {
                _selectedTransactionType = value;
                OnPropertyChanged();
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                GetSubCategoriesData();
                OnPropertyChanged();
            }
        }

        private Subcategory _selectedSubCategory;
        public Subcategory SelectedSubCategory
        {
            get => _selectedSubCategory;
            set
            {
                _selectedSubCategory = value;
                OnPropertyChanged();
            }
        }

        private FinancialRecord _currentFinancialRecord;
        public FinancialRecord CurrentFinancialRecord
        {
            get => _currentFinancialRecord;
            set
            {
                _currentFinancialRecord = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Account> Accounts { get; set; } = [];

        private void GetAccountsData()
        {
            Accounts.Clear();

            var accounts = _dataBaseService.GetDataTable<Account>(x => x.Where(x => x.IdUser == CurrentUser.IdUser));

            foreach (var item in accounts)
            {
                Accounts.Add(item);
            }
        }

        public ObservableCollection<TransactionType> TransactionTypes { get; set; } = [];

        private void GetTransactionTypesData()
        {
            TransactionTypes.Clear();

            var transactionTypes = _dataBaseService.GetDataTable<TransactionType>();

            foreach (var item in transactionTypes)
            {
                TransactionTypes.Add(item);
            }
        }

        public ObservableCollection<Category> Categories { get; set; } = [];

        private void GetCategoriesData()
        {
            Categories.Clear();

            var categories = _dataBaseService.GetDataTable<Category>(x => x.Where(x => x.IdUser == CurrentUser.IdUser));

            foreach (var item in categories)
            {
                Categories.Add(item);
            }
        }

        public ObservableCollection<Subcategory> Subcategories { get; set; } = [];

        private void GetSubCategoriesData()
        {
            Subcategories.Clear();

            var subCategories = _dataBaseService.GetDataTable<Subcategory>(x => x.Where(x => x.IdCategory == SelectedCategory.IdCategory));

            foreach (var item in subCategories)
            {
                Subcategories.Add(item);
            }
        }


        private RelayCommand _addFinancialRecordCommand;
        public RelayCommand AddFinancialRecordCommand { get => _addFinancialRecordCommand ??= new(obj => { AddFinancialRecord(); }); }

        private async void AddFinancialRecord()
        {
            if (string.IsNullOrEmpty(RecordName)
                && Amount == 0
                && SelectedAccount == null && SelectedTransactionType == null
                && SelectedCategory == null && SelectedSubCategory == null)
            {
                MessageBox.Show("Вы не заполнили поля!!");
                return;
            }

            FinancialRecord financialRecord = new FinancialRecord
            {
                IdUser = CurrentUser.IdUser,
                RecordName = RecordName,
                Ammount = Amount,
                Description = Description,
                Date = Date,
                IdAccount = SelectedAccount.IdAccount,
                IdTransactionType = SelectedTransactionType.IdTransactionType,
                IdCategory = SelectedCategory.IdCategory,
                IdSubcategory = SelectedSubCategory.IdSubcategory
            };

            await _dataBaseService.AddAsync(financialRecord);

            _pageNavigationService.RefreshData("Profile");

            ClearFinancialRecordData();
            //GetFinancialRecordData();
        }

        private RelayCommand _updateFinancialRecordCommand;
        public RelayCommand UpdateFinancialRecordCommand { get => _updateFinancialRecordCommand ??= new(obj => { UpdateFinancialRecord(); }); }

        private async void UpdateFinancialRecord()
        {
            if (CurrentFinancialRecord == null) { return; }

            if (string.IsNullOrEmpty(RecordName)
                && Amount == 0
                && SelectedAccount == null && SelectedTransactionType == null
                && SelectedCategory == null && SelectedSubCategory == null)
            {
                MessageBox.Show("Вы не заполнили поля!!");
                return;
            }

            var record = await _dataBaseService.FindIdAsync<FinancialRecord>(CurrentFinancialRecord.IdFinancialRecord);

            if (record != null)
            {
                record.RecordName = RecordName;
                record.Ammount = Amount;
                record.Description = Description;
                record.Date = Date;
                record.IdAccount = SelectedAccount.IdAccount;
                record.IdTransactionType = SelectedTransactionType.IdTransactionType;
                record.IdCategory = SelectedCategory.IdCategory;
                record.IdSubcategory = SelectedSubCategory.IdSubcategory;

                await _dataBaseService.UpdateAsync(record);

                _pageNavigationService.RefreshData("Profile", record.IdFinancialRecord);
            }
        }


        private void ClearFinancialRecordData()
        {
            RecordName = string.Empty;
            Amount = 0;
            Description = string.Empty;
            Date = DateTime.Now;
            SelectedAccount = null;
            SelectedTransactionType = null;
            SelectedSubCategory = null;
            SelectedSubCategory = null;
        }
    }
}
