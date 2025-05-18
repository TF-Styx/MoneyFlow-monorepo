using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using MoneyFlow.MVVM.Models.AppModel;
using MoneyFlow.MVVM.Models.DB_MSSQL;
using MoneyFlow.MVVM.ViewModels.BaseVM;
using MoneyFlow.Utils.Commands;
using MoneyFlow.Utils.Helpers;
using MoneyFlow.Utils.Services.AuthorizationVerificationServices;
using MoneyFlow.Utils.Services.DataBaseServices;
using MoneyFlow.Utils.Services.DialogServices.OpenFileDialogServices;
using MoneyFlow.Utils.Services.NavigationServices;
using MoneyFlow.Utils.Services.NavigationServices.WindowNavigationsService;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MoneyFlow.MVVM.ViewModels.PageVM
{
    internal class ProfilePageVM : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IAuthorizationVerificationService _authorizationVerificationService;
        private readonly IDataBaseService _dataBaseService;
        private readonly IOpenFileDialogService _openFileDialogService;
        private readonly IWindowNavigationService _windowNavigationService;

        private readonly LastRecordHelper _lastRecordHelper;

        public ProfilePageVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _authorizationVerificationService = _serviceProvider.GetService<IAuthorizationVerificationService>();
            _dataBaseService = _serviceProvider.GetService<IDataBaseService>();
            _openFileDialogService = _serviceProvider.GetService<IOpenFileDialogService>();
            _windowNavigationService = _serviceProvider.GetService<IWindowNavigationService>();

            _lastRecordHelper = _serviceProvider.GetRequiredService<LastRecordHelper>();

            CurrentUser = _authorizationVerificationService.CurrentUser;

            GetCategoriesData();
            GetAccountsData();

            GetAccountTypesData();
            GetGendersData();
            GetBanksData();

            GetFinancialRecordData(_authorizationVerificationService.CurrentUser);
            LoadFinancialRecordData();
        }

        public void Update(object parameter)
        {
            //CurrentUser = _authorizationVerificationService.CurrentUser;

            //GetCategoriesData();
            //GetAccountsData();

            //SelectedFinancialRecord = null;

            if (parameter == null)
            {
                FinancialRecords.Add(_lastRecordHelper.LastRecordFinancialRecord(CurrentUser, FinancialRecords.Count));
                CountRecords = FinancialRecords.Count;
            }

            if (parameter is int id)
            {
                var finRec = 
                    _dataBaseService.GetDataTable<FinancialRecord>(x => x
                                    .Include(x => x.IdCategoryNavigation)
                                    .Include(x => x.IdSubcategoryNavigation)
                                    .Include(x => x.IdTransactionTypeNavigation)
                                    .Include(x => x.IdAccountNavigation).ThenInclude(x => x.IdBankNavigation)
                                    .Include(x => x.IdAccountNavigation).ThenInclude(x => x.IdAccountTypeNavigation)
                                        .Where(x => x.IdFinancialRecord == id)).FirstOrDefault();

                FinancialRecords[FinancialRecords.IndexOf(FinancialRecords.FirstOrDefault(x => x.IdFinancialRecord == id))] = finRec;
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


        #region Данные профиля

        private Gender _selectedGender;
        public Gender SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Gender> Genders { get; set; } = [];

        private async void GetGendersData()
        {
            Genders.Clear();

            var genders = await _dataBaseService.GetDataTableAsync<Gender>();

            foreach (var item in genders)
            {
                Genders.Add(item);
            }

            SelectedGender = Genders.FirstOrDefault(x => x.IdGender == CurrentUser.IdGender);
        }


        private RelayCommand _selectedAvatarImageCommand;
        public RelayCommand SelectedAvatarImageCommand { get => _selectedAvatarImageCommand ??= new(obj => { SelectedAvatarImage(); }); }

        private void SelectedAvatarImage()
        {
            var files = _openFileDialogService.OpenDialog();
            if (files == null) { return; }

            if (files.Length == 1)
            {
                byte[] imageByte;

                using (Image image = Image.FromFile(files[0]))
                {
                    imageByte = FileHelper.ImageToBytes(image);
                }

                CurrentUser.Avatar = imageByte;
                OnPropertyChanged(nameof(CurrentUser));
            }
            else
            {
                MessageBox.Show("Больше одного изображения нельзя!!");
            }
        }


        private RelayCommand _saveProfileChangesCommand;
        public RelayCommand SaveProfileChangesCommand { get => _saveProfileChangesCommand ??= new(obj => { SaveProfileChanges(); }); }

        private async void SaveProfileChanges()
        {
            CurrentUser.IdGender = SelectedGender.IdGender;

            await _dataBaseService.UpdateAsync(CurrentUser);
        }

        #endregion 


        #region Категории 

        private byte[] _categoryImage;
        public byte[] CategoryImage
        {
            get => _categoryImage;
            set
            {
                _categoryImage = value;
                OnPropertyChanged();
            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        private string _categoryDescription;
        public string CategoryDescription
        {
            get => _categoryDescription;
            set
            {
                _categoryDescription = value;
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

                if (value == null) { return; }

                CategoryImage = value.Image;
                CategoryName = value.CategoryName;
                CategoryDescription = value.Description;

                SubcategoryImage = null;
                SubcategoryName = string.Empty;
                SubcategoryDescription = string.Empty;

                GetSubcategoriesData();
                OnPropertyChanged();
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

            SelectedCategoryFilter = Categories.FirstOrDefault();
        }


        private RelayCommand _categoryAddCommand;
        public RelayCommand CategoryAddCommand { get => _categoryAddCommand ??= new(obj => { CategoryAdd(); }); }

        private async void CategoryAdd()
        {
            Category category = new Category()
            {
                Image = CategoryImage,
                CategoryName = CategoryName,
                IdUser = _authorizationVerificationService.CurrentUser.IdUser,
            };

            await _dataBaseService.AddAsync(category);
            Categories.Add(_lastRecordHelper.LastRecordCategory(CurrentUser));
        }


        private RelayCommand _updateCategoryCommand;
        public RelayCommand UpdateCategoryCommand { get => _updateCategoryCommand ??= new(obj => { UpdateCategory(); }); }

        private async void UpdateCategory()
        {
            if (SelectedCategory == null) { return; }

            var updateItem = SelectedCategory;

            updateItem.Image = CategoryImage;
            updateItem.CategoryName = CategoryName;
            updateItem.Description = CategoryDescription;

            await _dataBaseService.UpdateAsync(updateItem);

            CategoryImage = null;
            CategoryName = null;
            CategoryDescription = null;

            GetCategoriesData();
        }


        private RelayCommand _selectedCategoryImageCommand;
        public RelayCommand SelectedCategoryImageCommand { get => _selectedCategoryImageCommand ??= new(obj => { SelectedCategoryImage(); }); }

        private void SelectedCategoryImage()
        {
            var files = _openFileDialogService.OpenDialog();
            if (files == null) { return; }

            if (files.Length == 1)
            {
                byte[] imageByte;

                using (Image image = Image.FromFile(files[0]))
                {
                    imageByte = FileHelper.ImageToBytes(image);
                }

                CategoryImage = imageByte;
            }
            else
            {
                MessageBox.Show("Больше одного изображения нельзя!!");
            }
        }

        #endregion 


        #region Подкатегории

        private byte[] _subCategoryImage;
        public byte[] SubcategoryImage
        {
            get => _subCategoryImage;
            set
            {
                _subCategoryImage = value;
                OnPropertyChanged();
            }
        }

        private string _subcategoryName;
        public string SubcategoryName
        {
            get => _subcategoryName;
            set
            {
                _subcategoryName = value;
                OnPropertyChanged();
            }
        }

        private string _subcategoryDescription;
        public string SubcategoryDescription
        {
            get => _subcategoryDescription;
            set
            {
                _subcategoryDescription = value;
                OnPropertyChanged();
            }
        }

        private Subcategory _selectedSubcategory;
        public Subcategory SelectedSubcategory
        {
            get => _selectedSubcategory;
            set
            {
                _selectedSubcategory = value;

                if (value == null) { return; }

                SubcategoryImage = value.Image;
                SubcategoryName = value.SubcategoryName;
                SubcategoryDescription = value.Description;

                OnPropertyChanged();
            }
        }


        public ObservableCollection<Subcategory> Subcategories { get; set; } = [];

        private void GetSubcategoriesData()
        {
            Subcategories.Clear();

            var subcategories = _dataBaseService.GetDataTable<Subcategory>(x => x.Where(x => x.IdCategory == SelectedCategory.IdCategory));

            foreach (var item in subcategories)
            {
                Subcategories.Add(item);
            }
        }


        private RelayCommand _subcategoryAddCommand;
        public RelayCommand SubcategoryAddCommand { get => _subcategoryAddCommand ??= new(obj => { SubcategoryAdd(); }); }

        private async void SubcategoryAdd()
        {
            Subcategory category = new Subcategory()
            {
                Image = SubcategoryImage,
                SubcategoryName = SubcategoryName,
                IdCategory = SelectedCategory.IdCategory,
            };

            await _dataBaseService.AddAsync(category);
            Subcategories.Add(_lastRecordHelper.LastRecordSubcategory(SelectedCategory));
        }


        private RelayCommand _updateSubcategoryCommand;
        public RelayCommand UpdateSubcategoryCommand { get => _updateSubcategoryCommand ??= new(obj => { UpdateSubcategory(); }); }

        private async void UpdateSubcategory()
        {
            if (SelectedSubcategory == null) { return; }

            var updateItem = SelectedSubcategory;

            updateItem.Image = SubcategoryImage;
            updateItem.SubcategoryName = SubcategoryName;
            updateItem.Description = SubcategoryDescription;

            await _dataBaseService.UpdateAsync(updateItem);

            SubcategoryImage = null;
            SubcategoryName = string.Empty;
            SubcategoryDescription = string.Empty;

            GetSubcategoriesData();
        }


        private RelayCommand _selectedSubcategoryImageCommand;
        public RelayCommand SelectedSubcategoryImageCommand { get => _selectedSubcategoryImageCommand ??= new(obj => { SelectedSubcategoryImage(); }); }

        private void SelectedSubcategoryImage()
        {
            var files = _openFileDialogService.OpenDialog();
            if (files == null) { return; }

            if (files.Length == 1)
            {
                byte[] imageByte;

                using (Image image = Image.FromFile(files[0]))
                {
                    imageByte = FileHelper.ImageToBytes(image);
                }

                SubcategoryImage = imageByte;
            }
            else
            {
                MessageBox.Show("Больше одного изображения нельзя!!");
            }
        }

        #endregion


        #region Счета

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

        private Bank _selectedBank;
        public Bank SelectedBank
        {
            get => _selectedBank;
            set
            {
                _selectedBank = value;
                OnPropertyChanged();
            }
        }

        private AccountType _selectedAccountTypes;
        public AccountType SelectedAccountTypes
        {
            get => _selectedAccountTypes;
            set
            {
                _selectedAccountTypes = value;
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

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;

                if (value == null) { return; }

                NumberAccount = value.NumberAccount;
                SelectedBank = Banks.FirstOrDefault(x => x.IdBank == value.IdBank);
                SelectedAccountTypes = AccountTypes.FirstOrDefault(x => x.IdAccountType == value.IdAccountType);
                Balance = value.Balance;

                OnPropertyChanged();
            }
        }


        public ObservableCollection<Account> Accounts { get; set; } = [];

        private void GetAccountsData()
        {
            Accounts.Clear();
            //AccountsFilter.Clear();

            var account = _dataBaseService.GetDataTable<Account>(x => x
                                                    .Include(x => x.IdBankNavigation)
                                                    .Include(x => x.IdAccountTypeNavigation)
                                                        .Where(x => x.IdUser == CurrentUser.IdUser));
            //AccountsFilter.Add(new Account()
            //{
            //    NumberAccount = 0000
            //});

            foreach (var item in account)
            {
                Accounts.Add(item);
                //AccountsFilter.Add(item);
            }

            SelectedAccountFilter = Accounts.FirstOrDefault();
        }

        public ObservableCollection<AccountType> AccountTypes { get; set; } = [];

        private async void GetAccountTypesData()
        {
            AccountTypes.Clear();

            var accountType = await _dataBaseService.GetDataTableAsync<AccountType>();

            foreach (var item in accountType)
            {
                AccountTypes.Add(item);
            }
        }

        public ObservableCollection<Bank> Banks { get; set; } = [];

        private void GetBanksData()
        {
            Banks.Clear(); 
            //BanksFilter.Clear();

            var banks = _dataBaseService.GetDataTable<Bank>();

            //BanksFilter.Add(new Bank()
            //{
            //    BankName = NOT_INCLUDE
            //});

            foreach (var item in banks)
            {
                Banks.Add(item);
                //BanksFilter.Add(item);
            }

            SelectedBankFilter = Banks.FirstOrDefault();
        }


        private RelayCommand _accountAddCommand;
        public RelayCommand AccountAddCommand { get => _accountAddCommand ??= new(obj => { AccountAdd(); }); }

        private async void AccountAdd()
        {
            if (NumberAccount == null && SelectedBank == null &&
                SelectedAccountTypes == null && Balance == null)
            {
                MessageBox.Show("Вы не заполнили «Номер счета»");
                return;
            }

            Account account = new Account()
            {
                IdUser = _authorizationVerificationService.CurrentUser.IdUser,
                NumberAccount = NumberAccount,
                IdBank = SelectedBank.IdBank,
                IdAccountType = SelectedAccountTypes.IdAccountType,
                Balance = Balance
            };

            await _dataBaseService.AddAsync(account);
            Accounts.Add(_lastRecordHelper.LastRecordAccount(CurrentUser));
        }


        private RelayCommand _updateAccountCommand;
        public RelayCommand UpdateAccountCommand { get => _updateAccountCommand ??= new(obj => { UpdateAccount(); }); }

        private async void UpdateAccount()
        {
            if (SelectedAccount == null) { return; }

            var updateItem = SelectedAccount;

            if (NumberAccount == null && SelectedBank == null && 
                SelectedAccountTypes == null && Balance == null)
            {
                MessageBox.Show("Вы не заполнили «Номер счета»");
                return;
            }

            updateItem.NumberAccount = NumberAccount;
            updateItem.IdBank = SelectedBank.IdBank;
            updateItem.IdAccountType = SelectedAccountTypes.IdAccountType;
            updateItem.Balance = Balance;

            await _dataBaseService.UpdateAsync(updateItem);

            NumberAccount = null;
            SelectedBank = null;
            SelectedAccountTypes = null;
            Balance = 0;

            GetAccountsData();
        }

        #endregion


        #region Транзакции

        private int _countRecords;
        public int CountRecords
        {
            get => _countRecords;
            set
            {
                _countRecords = value;
                OnPropertyChanged();
            }
        }

        private bool _isOpenPopupFilterFinancialRecords;
        public bool IsOpenPopupFilterFinancialRecords
        {
            get => _isOpenPopupFilterFinancialRecords;
            set
            {
                _isOpenPopupFilterFinancialRecords = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _openPopupFilterFinancialRecordsCommand;
        public RelayCommand OpenPopupFilterFinancialRecordsCommand { get => _openPopupFilterFinancialRecordsCommand ??= new(obj => { IsOpenPopupFilterFinancialRecords = true; }); }


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


        private List<FinancialRecord> _financialRecords { get; set; } = [];
        public ObservableCollection<FinancialRecord> FinancialRecords { get; set; } = [];

        private void GetFinancialRecordData(User user)
        {
            var financialRecordData =
                _dataBaseService.GetDataTable<FinancialRecord>(x => x
                            .Include(x => x.IdCategoryNavigation)
                            .Include(x => x.IdSubcategoryNavigation)
                            .Include(x => x.IdTransactionTypeNavigation)
                            .Include(x => x.IdAccountNavigation).ThenInclude(x => x.IdBankNavigation)
                            .Include(x => x.IdAccountNavigation).ThenInclude(x => x.IdAccountTypeNavigation)
                                .Where(x => x.IdUser == user.IdUser));

            _financialRecords.AddRange(financialRecordData);
        }


        private RelayCommand _openFinancialRecordAddCommand;
        public RelayCommand OpenFinancialRecordAddCommand { get => _openFinancialRecordAddCommand ??= new(obj => { OpenFinancialRecordAdd(); }); }

        private void OpenFinancialRecordAdd()
        {
            _windowNavigationService.NavigateTo("FinancialRecordAdd", CurrentUser);
        }

        private RelayCommand _itemDoubleClickCommand;
        public RelayCommand ItemDoubleClickCommand => _itemDoubleClickCommand ??= new RelayCommand(DoubleClick);

        private void DoubleClick(object parameter)
        {
            if (parameter is FinancialRecord financialRecord)
            {
                //MessageBox.Show($"{financialRecord.RecordName}\n" +
                //    $"{financialRecord.Ammount}\n" +
                //    $"{financialRecord.Date}\n" +
                //    $"{financialRecord.IdAccount}\n" +
                //    $"{financialRecord.IdTransactionType}\n" +
                //    $"{financialRecord.IdCategory}\n" +
                //    $"{financialRecord.Description}\n");

                _windowNavigationService.NavigateTo("FinancialRecordAdd", financialRecord);
            }
        }

        #endregion


        #region Фильтр

        private const string NOT_INCLUDE = "Не учитывать!";

        private decimal? _startAmount = 0;
        public decimal? StartAmount
        {
            get => _startAmount;
            set
            {
                _startAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal? _endAmount = 999999999;
        public decimal? EndAmount
        {
            get => _endAmount;
            set
            {
                _endAmount = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startDate = DateTime.Now.AddYears(-1);
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate = DateTime.Now;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        private Bank _selectedBankFilter;
        public Bank SelectedBankFilter
        {
            get => _selectedBankFilter;
            set
            {
                _selectedBankFilter = value;
                OnPropertyChanged();
            }
        }

        private Account _selectedAccountFilter;
        public Account SelectedAccountFilter
        {
            get => _selectedAccountFilter;
            set
            {
                _selectedAccountFilter = value;
                OnPropertyChanged();
            }
        }

        private Category _selectedCategoryFilter;
        public Category SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set
            {
                _selectedCategoryFilter = value;
                OnPropertyChanged();
            }
        }

        private bool _considerFilters = false;
        public bool ConsiderFilters
        {
            get => _considerFilters;
            set
            {
                _considerFilters = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Bank> BanksFilter { get; set; } = [];
        public ObservableCollection<Account> AccountsFilter { get; set; } = [];
        public ObservableCollection<Category> CategoriesFilter { get; set; } = [];


        private List<FinancialRecord> ApplyFilter(List<FinancialRecord> financialRecords)
        {
            if (ConsiderFilters)
            {
                var filters = new List<Func<FinancialRecord, bool>>();

                filters.Add(x => x.Ammount >= StartAmount && x.Ammount <= EndAmount);
                filters.Add(x => x.Date >= StartDate && x.Date <= EndDate);
                filters.Add(x => x.IdAccountNavigation.IdBank == SelectedBankFilter.IdBank);
                filters.Add(x => x.IdAccount == SelectedAccountFilter.IdAccount);
                filters.Add(x => x.IdCategory == SelectedCategoryFilter.IdCategory);

                var record = filters.Aggregate(_financialRecords.AsEnumerable(), (current, filter) => current.Where(filter));
                financialRecords = record.ToList();

                return financialRecords;
            }
            else { return financialRecords; }
        }


        private RelayCommand _acceptFilterCommand;
        public RelayCommand AcceptFilterCommand { get => _acceptFilterCommand ??= new(obj => { LoadFinancialRecordData(); }); }

        private void LoadFinancialRecordData()
        {
            FinancialRecords.Clear();

            foreach (var item in ApplyFilter(_financialRecords))
            {
                FinancialRecords.Add(item);
                item.IndexRecord = FinancialRecords.IndexOf(item) + 1;
            }

            CountRecords = FinancialRecords.Count;
        }

        #endregion
    }
}
