using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordViewingInterfaces;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.PageViewModels
{
    internal class FinancialRecordPageVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly IFinancialRecordService _financialRecordService;
        private readonly IGetFinancialRecordViewingUseCase _getFinancialRecordViewingUseCase;

        private readonly INavigationPages _navigationPages;

        public FinancialRecordPageVM(   IAuthorizationService authorizationService, 
                                        IAccountService accountService,
                                        ICategoryService categoryService,
                                        ISubcategoryService subcategoryService,
                                        ITransactionTypeService transactionTypeService,
                                        IFinancialRecordService financialRecordService, 
                                        IGetFinancialRecordViewingUseCase getFinancialRecordViewingUseCase,
                                        INavigationPages navigationPages)
        {
            _authorizationService = authorizationService;
            _accountService = accountService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _transactionTypeService = transactionTypeService;
            _financialRecordService = financialRecordService;
            _getFinancialRecordViewingUseCase = getFinancialRecordViewingUseCase;

            _navigationPages = navigationPages;

            CurrentUser = _authorizationService.CurrentUser;
            
            GetTransactionType();
            GetCategory();
            GetAccount();

            //GetTransactionTypeFilter();
            //GetCategoryFilter();
            //GetAccountFilter();

            //var (Start, End) = DefaultDate();
            //DateStartFilter = Start;
            //DateEndFilter = End;

            //GetFinancialRecord();
        }

        public async void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is FinancialRecordViewingDTO financialRecord)
            {
                SelectedFinancialRecord = financialRecord;
                //SelectedFinancialRecord = FinancialRecords.FirstOrDefault(x => x.IdFinancialRecord == financialRecord.IdFinancialRecord);
                //GetFinancialRecordById(SelectedFinancialRecord.IdFinancialRecord);

                //RecordName = SelectedFinancialRecord.RecordName;
                //Amount = SelectedFinancialRecord.Amount;
                //Description = SelectedFinancialRecord.Description;

                //SelectedTransactionType = TransactionTypes.FirstOrDefault(x => x.TransactionTypeName == SelectedFinancialRecord.TransactionTypeName);

                //var idCategory = await _categoryService.GetById(SelectedFinancialRecord.IdFinancialRecord);
                //SelectedCategory = Categories.FirstOrDefault(x => x.IdCategory == idCategory);
                //SelectImageCat = SelectedCategory?.Image;

                //var idSubcategory = await _subcategoryService.GetById(SelectedFinancialRecord.IdFinancialRecord);
                //SelectedSubcategory = Subcategories.FirstOrDefault(x => x.IdSubcategory == idSubcategory);
                //SelectImageSub = SelectedSubcategory?.Image;

                //SelectedAccount = Accounts.FirstOrDefault(x => x.NumberAccount == SelectedFinancialRecord.AccountNumber);

                //Date = SelectedFinancialRecord.Date;
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


        // ------------------------------------------------------------------------------------------------------------------------------------------------

        #region Поля финансовой записи

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

        private TransactionTypeDTO _selectedTransactionType;
        public TransactionTypeDTO SelectedTransactionType
        {
            get => _selectedTransactionType;
            set
            {
                _selectedTransactionType = value;

                if (value == null) { return; }

                OnPropertyChanged();
            }
        }

        private CategoryDTO? _selectedCategory;
        public CategoryDTO? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;

                if (value == null) { return; }

                SelectImageCat = value.Image;

                GetSubcategoryByIdCategory();

                OnPropertyChanged();
            }
        }

        private byte[] _selectImageCat;
        public byte[] SelectImageCat
        {
            get => _selectImageCat;
            set
            {
                _selectImageCat = value;
                OnPropertyChanged();
            }
        }

        private SubcategoryDTO _selectedSubcategory;
        public SubcategoryDTO SelectedSubcategory
        {
            get => _selectedSubcategory;
            set
            {
                _selectedSubcategory = value;

                if (value == null) { return; }

                SelectImageSub = value.Image;

                OnPropertyChanged();
            }
        }

        private byte[] _selectImageSub;
        public byte[] SelectImageSub
        {
            get => _selectImageSub;
            set
            {
                _selectImageSub = value;
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

                if (value == null) { return; }

                OnPropertyChanged();
            }
        }

        #endregion


        private FinancialRecordViewingDTO _selectedFinancialRecord;
        public FinancialRecordViewingDTO SelectedFinancialRecord
        {
            get => _selectedFinancialRecord;
            set
            {
                _selectedFinancialRecord = value;

                if (value == null) { return; }

                GetFinancialRecordById(value.IdFinancialRecord);

                OnPropertyChanged();
            }
        }


        private FinancialRecordDTO _currentSelectedFinancialRecord;

        private async void GetFinancialRecordById(int idFinancialRecord)
        {
            _currentSelectedFinancialRecord = await _financialRecordService.GetAsync(idFinancialRecord);

            RecordName = _currentSelectedFinancialRecord.RecordName;
            Amount = _currentSelectedFinancialRecord.Amount;
            Description = _currentSelectedFinancialRecord.Description;
            SelectedTransactionType = TransactionTypes.FirstOrDefault(x => x.IdTransactionType == _currentSelectedFinancialRecord.IdTransactionType);
            SelectedCategory = Categories.FirstOrDefault(x => x.IdCategory == _currentSelectedFinancialRecord.IdCategory);
            SelectImageCat = SelectedCategory?.Image;
            SelectedSubcategory = Subcategories.FirstOrDefault(x => x.IdSubcategory == _currentSelectedFinancialRecord.IdSubcategory);
            SelectImageSub = SelectedSubcategory?.Image;
            SelectedAccount = Accounts.FirstOrDefault(x => x.IdAccount == _currentSelectedFinancialRecord.IdAccount);
            Date = _currentSelectedFinancialRecord.Date;
        }



        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Типы операций

        public ObservableCollection<TransactionTypeDTO> TransactionTypes { get; set; } = [];

        public void GetTransactionType()
        {
            TransactionTypes.Clear();

            var list = _transactionTypeService.GetAllTransactionType();

            foreach (var item in list)
            {
                TransactionTypes.Add(item);
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Категории

        public ObservableCollection<CategoryDTO> Categories { get; set; } = [];

        private void GetCategory()
        {
            Categories.Clear();

            var list = _categoryService.GetCatCategory(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Categories.Add(item);
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Подкатегории

        public ObservableCollection<SubcategoryDTO> Subcategories { get; set; } = [];

        private void GetSubcategoryByIdCategory()
        {
            Subcategories.Clear();

            var list = _subcategoryService.GetIdUserIdCategorySub(CurrentUser.IdUser, SelectedCategory.IdCategory);
            //var list = _subcategoryService.GetAllIdUserSub(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Subcategories.Add(item);
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Счета

        public ObservableCollection<AccountDTO> Accounts { get; set; } = [];

        private void GetAccount()
        {
            Accounts.Clear();

            var list = _accountService.GetAll(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Accounts.Add(item);
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Команды

        private RelayCommand _financialRecordAddCommand;
        public RelayCommand FinancialRecordAddCommand
        {
            get => _financialRecordAddCommand ??= new(async obj =>
            {
                var newRecord = await _financialRecordService.CreateAsync(RecordName, Amount, Description, SelectedTransactionType.IdTransactionType, CurrentUser.IdUser, SelectedCategory.IdCategory, SelectedSubcategory.IdSubcategory, SelectedAccount.IdAccount, Date);

                if (newRecord.Message != string.Empty)
                {
                    MessageBox.Show(newRecord.Message);
                    return;
                }

                var record = _getFinancialRecordViewingUseCase.GetById(CurrentUser.IdUser, newRecord.FinancialRecordDTO.IdFinancialRecord, SelectedCategory.IdCategory, SelectedSubcategory.IdSubcategory);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (record, SelectedTransactionType.IdTransactionType), ParameterType.Add);
            });
        }

        private RelayCommand _financialRecordUpdateCommand;
        public RelayCommand FinancialRecordUpdateCommand
        {
            get => _financialRecordUpdateCommand ??= new(async obj =>
            {
                var idUpdateRecord = await _financialRecordService.UpdateAsync
                    (
                        SelectedFinancialRecord.IdFinancialRecord,
                        RecordName,
                        Amount,
                        Description,
                        SelectedTransactionType.IdTransactionType,
                        CurrentUser.IdUser,
                        SelectedCategory.IdCategory,
                        SelectedSubcategory.IdSubcategory,
                        SelectedAccount.IdAccount,
                        Date
                    );

                //var updateFinancialRecord = FinancialRecords
                //    .FirstOrDefault(x => x.IdFinancialRecord == SelectedFinancialRecord.IdFinancialRecord)
                //        .SetProperty(x =>
                //        {
                //            x.IdFinancialRecord = idUpdateRecord;
                //            x.RecordName = RecordName;
                //            x.Amount = Amount;
                //            x.Description = Description;
                //            x.TransactionTypeName = SelectedTransactionType.TransactionTypeName;
                //            x.IdUser = CurrentUser.IdUser;
                //            x.CategoryName = SelectedCategory.CategoryName;
                //            x.SubcategoryName = SelectedSubcategory.SubcategoryName;
                //            x.AccountNumber = SelectedAccount.NumberAccount;
                //            x.Date = Date;
                //        });

                //var index = FinancialRecords.IndexOf(updateFinancialRecord);

                //FinancialRecords.RemoveAt(index);
                //FinancialRecords.Insert(index, updateFinancialRecord);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (idUpdateRecord, SelectedTransactionType.IdTransactionType), ParameterType.Update);
            });
        }

        private RelayCommand _financialRecordDeleteCommand;
        public RelayCommand FinancialRecordDeleteCommand
        {
            get => _financialRecordDeleteCommand ??= new(async obj =>
            {
                await _financialRecordService.DeleteAsync(SelectedFinancialRecord.IdFinancialRecord);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, SelectedFinancialRecord, ParameterType.Delete);

                RecordName = string.Empty;
                Amount = 0;
                Description = string.Empty;
                SelectedTransactionType = null;
                SelectedCategory = null;
                SelectedAccount = null;
                Date = DateTime.Now;
            });
        }

        #endregion


        //// ------------------------------------------------------------------------------------------------------------------------------------------------


        //#region Заполнение финансовых записей

        //public ObservableCollection<FinancialRecordViewingDTO> FinancialRecords { get; set; } = [];

        //public void GetFinancialRecord()
        //{
        //    FinancialRecords.Clear();

        //    var filter = new FinancialRecordFilterDTO()
        //    {
        //        AmountStart = AmountStartFilter,
        //        AmountEnd = AmountEndFilter,
        //        IsConsiderAmount = IsConsiderAmount,

        //        IdTransactionType = SelectedTransactionTypeFilter?.IdTransactionType == 0 ? null : SelectedTransactionTypeFilter?.IdTransactionType,
        //        IdCategory = SelectedCategoryFilter?.IdCategory == 0 ? null : SelectedCategoryFilter?.IdCategory,
        //        IdSubcategory = SelectedSubcategoryFilter?.IdSubcategory == 0 ? null : SelectedSubcategoryFilter?.IdSubcategory,
        //        IdAccount = SelectedAccountFilter?.IdAccount == 0 ? null : SelectedAccountFilter?.IdAccount,

        //        DateStart = DateStartFilter,
        //        DateEnd = DateEndFilter,
        //        IsConsiderDate = IsConsiderDate,
        //    };

        //    var list = _getFinancialRecordViewingUseCase.GetAllViewing(CurrentUser.IdUser, filter);

        //    foreach (var item in list)
        //    {
        //        FinancialRecords.Add(item);
        //        var index = FinancialRecords.IndexOf(item);
        //        item.Index = index + 1;
        //    }
        //}

        //#endregion


        //// ------------------------------------------------------------------------------------------------------------------------------------------------


        //#region Навигация

        //private RelayCommand _openProfileUserPageCommand;
        //public RelayCommand OpenProfileUserPageCommand
        //{
        //    get => _openProfileUserPageCommand ??= new(obj =>
        //    {
        //        _navigationPages.OpenPage(PageType.UserPage);
        //    });
        //}

        //#endregion


        //// ---------------------------------------------------------------------------------------------------------------------------------


        //#region Фильтрация

        //private bool _isOpenPopupFilter;
        //public bool IsOpenPopupFilter
        //{
        //    get => _isOpenPopupFilter;
        //    set
        //    {
        //        _isOpenPopupFilter = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private RelayCommand _openPopupFilterCommand;
        //public RelayCommand OpenPopupFilterCommand { get => _openPopupFilterCommand ??= new(async obj => { IsOpenPopupFilter = true; }); }

        //// ---------------------------------------------------------------------------------------------------------------------------------

        //#region Поля фильрации

        //private decimal? _amountStartFilter;
        //public decimal? AmountStartFilter
        //{
        //    get => _amountStartFilter;
        //    set
        //    {
        //        _amountStartFilter = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private decimal? _amountEndFilter;
        //public decimal? AmountEndFilter
        //{
        //    get => _amountEndFilter;
        //    set
        //    {
        //        _amountEndFilter = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private bool _isConsiderAmount;
        //public bool IsConsiderAmount
        //{
        //    get => _isConsiderAmount;
        //    set
        //    {
        //        _isConsiderAmount = value;
        //        OnPropertyChanged();
        //    }
        //}


        //private TransactionTypeDTO? _selectedTransactionTypeFilter;
        //public TransactionTypeDTO? SelectedTransactionTypeFilter
        //{
        //    get => _selectedTransactionTypeFilter;
        //    set
        //    {
        //        _selectedTransactionTypeFilter = value;

        //        if (value == null) { return; }

        //        OnPropertyChanged();
        //    }
        //}

        //private CategoryDTO? _selectedCategoryFilter;
        //public CategoryDTO? SelectedCategoryFilter
        //{
        //    get => _selectedCategoryFilter;
        //    set
        //    {
        //        _selectedCategoryFilter = value;

        //        if (value == null) { return; }

        //        GetSubcategoryFilter();

        //        OnPropertyChanged();
        //    }
        //}

        //private SubcategoryDTO? _selectedSubcategoryFilter;
        //public SubcategoryDTO? SelectedSubcategoryFilter
        //{
        //    get => _selectedSubcategoryFilter;
        //    set
        //    {
        //        _selectedSubcategoryFilter = value;

        //        if (value == null) { return; }

        //        OnPropertyChanged();
        //    }
        //}

        //private AccountDTO? _selectedAccountFilter;
        //public AccountDTO? SelectedAccountFilter
        //{
        //    get => _selectedAccountFilter;
        //    set
        //    {
        //        _selectedAccountFilter = value;

        //        if (value == null) { return; }

        //        OnPropertyChanged();
        //    }
        //}


        //private DateTime? _dateStartFilter;
        //public DateTime? DateStartFilter
        //{
        //    get => _dateStartFilter;
        //    set
        //    {
        //        _dateStartFilter = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private DateTime? _dateEndFilter;
        //public DateTime? DateEndFilter
        //{
        //    get => _dateEndFilter;
        //    set
        //    {
        //        _dateEndFilter = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private bool _isConsiderDate = true;
        //public bool IsConsiderDate
        //{
        //    get => _isConsiderDate;
        //    set
        //    {
        //        _isConsiderDate = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private (DateTime Start, DateTime End) DefaultDate()
        //{
        //    var start = new DateTime(DateTime.Now.Year, 1, 1);
        //    var end = new DateTime(DateTime.Now.Year, 12, 31);

        //    return (start, end);
        //}

        //#endregion

        //// ---------------------------------------------------------------------------------------------------------------------------------

        //#region Заполнение списка для фильтрации

        //public ObservableCollection<TransactionTypeDTO> TransactionTypesFilter { get; set; } = [];
        //public async Task GetTransactionTypeFilter()
        //{
        //    TransactionTypesFilter.Clear();

        //    var list = await _transactionTypeService.GetAllAsyncTransactionType();

        //    foreach (var item in list)
        //    {
        //        TransactionTypesFilter.Add(item);
        //    }

        //    TransactionTypesFilter.Insert(0, new TransactionTypeDTO()
        //    {
        //        IdTransactionType = 0,
        //        TransactionTypeName = "<<Не выбрано!!>>",
        //    });

        //    SelectedTransactionTypeFilter = TransactionTypesFilter.FirstOrDefault();
        //}

        //public ObservableCollection<CategoryDTO> CategoriesFilter { get; set; } = [];
        //public async Task GetCategoryFilter()
        //{
        //    CategoriesFilter.Clear();

        //    var list = await _categoryService.GetCatAsyncCategory(CurrentUser.IdUser);

        //    foreach (var item in list)
        //    {
        //        CategoriesFilter.Add(item);
        //    }

        //    CategoriesFilter.Insert(0, new CategoryDTO()
        //    {
        //        IdCategory = 0,
        //        CategoryName = "<<Не выбрано!!>>",
        //    });

        //    SelectedCategoryFilter = CategoriesFilter.FirstOrDefault();
        //}

        //public ObservableCollection<SubcategoryDTO> SubcategoriesFilter { get; set; } = [];
        //public async Task GetSubcategoryFilter()
        //{
        //    SubcategoriesFilter.Clear();

        //    var list = _subcategoryService.GetIdUserIdCategorySub(CurrentUser.IdUser, SelectedCategoryFilter.IdCategory);

        //    foreach (var item in list)
        //    {
        //        SubcategoriesFilter.Add(item);
        //    }

        //    SubcategoriesFilter.Insert(0, new SubcategoryDTO()
        //    {
        //        IdSubcategory = 0,
        //        SubcategoryName = "<<Не выбрано!!>>",
        //    });
        //}

        //public ObservableCollection<AccountDTO> AccountsFilter { get; set; } = [];
        //public async Task GetAccountFilter()
        //{
        //    AccountsFilter.Clear();

        //    var list = await _accountService.GetAllAsync(CurrentUser.IdUser);

        //    foreach (var item in list)
        //    {
        //        AccountsFilter.Add(item);
        //    }

        //    AccountsFilter.Insert(0, new AccountDTO()
        //    {
        //        IdAccount = 0,
        //        NumberAccount = 0,
        //    });

        //    SelectedAccountFilter = AccountsFilter.FirstOrDefault();
        //}

        //#endregion

        //// ---------------------------------------------------------------------------------------------------------------------------------

        //private RelayCommand _applyCommand;
        //public RelayCommand ApplyCommand { get => _applyCommand ??= new(async obj => { GetFinancialRecord(); }); }

        //private RelayCommand _dropCommand;
        //public RelayCommand DropCommand
        //{
        //    get => _dropCommand ??= new(obj =>
        //    {
        //        IsConsiderAmount = false;

        //        SelectedTransactionTypeFilter = TransactionTypesFilter.FirstOrDefault();
        //        SelectedCategoryFilter = CategoriesFilter.FirstOrDefault();
        //        SelectedSubcategoryFilter = SubcategoriesFilter.FirstOrDefault();
        //        SelectedAccountFilter = AccountsFilter.FirstOrDefault();

        //        var (Start, End) = DefaultDate();
        //        DateStartFilter = Start;
        //        DateEndFilter = End;

        //        IsConsiderDate = false;

        //        GetFinancialRecord();
        //    });
        //}

        //#endregion
    }
}
