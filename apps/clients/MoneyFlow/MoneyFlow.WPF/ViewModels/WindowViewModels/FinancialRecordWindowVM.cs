using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordViewingInterfaces;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.WindowViewModels
{
    internal class FinancialRecordWindowVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly IFinancialRecordService _financialRecordService;
        private readonly IGetFinancialRecordViewingUseCase _getFinancialRecordViewingUseCase;

        private readonly INavigationPages _navigationPages;

        public FinancialRecordWindowVM(IAuthorizationService authorizationService,
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
        }

        public async void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is FinancialRecordViewingDTO financialRecord)
            {
                SelectedFinancialRecord = financialRecord;
            }


            #region Обновление категорий

            if (parameter is CategoryDTO categoryAdd && typeParameter is ParameterType.Add)
            {
                Categories.Add(categoryAdd);
            }
            if (parameter is CategoryDTO categoryUpdate && typeParameter is ParameterType.Update)
            {
                var itemForDelete = Categories.FirstOrDefault(x => x.IdCategory == categoryUpdate.IdCategory);
                var index = Categories.IndexOf(itemForDelete);

                Categories.Remove(itemForDelete);
                Categories.Insert(index, categoryUpdate);
            }
            if (parameter is CategoryDTO categoryDelete && typeParameter is ParameterType.Delete)
            {
                Categories.Remove(Categories.FirstOrDefault(x => x.IdCategory == categoryDelete.IdCategory));
            }

            #endregion


            #region Обновление подкатегорий

            if (SelectedCategory != null)
            {
                if (parameter is SubcategoryDTO subcategoryAdd && typeParameter is ParameterType.Add)
                {
                    Subcategories.Add(subcategoryAdd);
                }
                if (parameter is SubcategoryDTO subcategoryUpdate && typeParameter is ParameterType.Update)
                {
                    var itemForDelete = Subcategories.FirstOrDefault(x => x.IdSubcategory == subcategoryUpdate.IdSubcategory);
                    var index = Subcategories.IndexOf(itemForDelete);

                    Subcategories.Remove(itemForDelete);
                    Subcategories.Insert(index, subcategoryUpdate);
                }
                if (parameter is SubcategoryDTO subcategoryDelete && typeParameter is ParameterType.Delete)
                {
                    Subcategories.Remove(Subcategories.FirstOrDefault(x => x.IdSubcategory == subcategoryDelete.IdSubcategory));
                }
            }

            #endregion
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
                int? idCategory = SelectedCategory == null ? null : SelectedCategory.IdCategory;
                int? idSubcategory = SelectedSubcategory == null ? null : SelectedSubcategory.IdSubcategory;

                if (SelectedTransactionType == null)
                { 
                    MessageBox.Show("Вы не выбрали <<Тип операции>>!!"); 
                    return;
                }
                if (SelectedCategory == null)
                {
                    MessageBox.Show("Вы не выбрали <<Счёт>>!!");
                    return;
                }

                var newRecord = await _financialRecordService.CreateAsync
                (
                    RecordName, 
                    Amount, 
                    Description, 
                    SelectedTransactionType.IdTransactionType, 
                    CurrentUser.IdUser, 
                    idCategory, 
                    idSubcategory, 
                    SelectedAccount.IdAccount, 
                    Date
                );

                if (newRecord.Message != string.Empty)
                {
                    MessageBox.Show(newRecord.Message);
                    return;
                }

                var record = _getFinancialRecordViewingUseCase.GetById
                (
                    CurrentUser.IdUser, 
                    newRecord.FinancialRecordDTO.IdFinancialRecord, 
                    idCategory.Value, 
                    idSubcategory.Value
                );

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (record, SelectedTransactionType.IdTransactionType), ParameterType.Add);
            });
        }

        private RelayCommand _financialRecordUpdateCommand;
        public RelayCommand FinancialRecordUpdateCommand
        {
            get => _financialRecordUpdateCommand ??= new(async obj =>
            {
                if (SelectedFinancialRecord == null)
                {
                    MessageBox.Show("Вы не выбрали <<Финансовую запись>>!!");
                    return;
                }
                if (SelectedTransactionType == null)
                {
                    MessageBox.Show("Вы не выбрали <<Тип операции>>!!");
                    return;
                }
                if (SelectedCategory == null)
                {
                    MessageBox.Show("Вы не выбрали <<Счёт>>!!");
                    return;
                }

                int? idCategory = SelectedCategory == null ? null : SelectedCategory.IdCategory;
                int? idSubcategory = SelectedSubcategory == null ? null : SelectedSubcategory.IdSubcategory;

                var idUpdateRecord = await _financialRecordService.UpdateAsync
                    (
                        SelectedFinancialRecord.IdFinancialRecord,
                        RecordName,
                        Amount,
                        Description,
                        SelectedTransactionType.IdTransactionType,
                        CurrentUser.IdUser,
                        idCategory,
                        idSubcategory,
                        SelectedAccount.IdAccount,
                        Date
                    );

                var updateRecord = await _getFinancialRecordViewingUseCase.GetViewingAsync(idUpdateRecord);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (updateRecord, SelectedTransactionType.IdTransactionType), ParameterType.Update);
            });
        }

        private RelayCommand _financialRecordDeleteCommand;
        public RelayCommand FinancialRecordDeleteCommand
        {
            get => _financialRecordDeleteCommand ??= new(async obj =>
            {
                if (SelectedFinancialRecord == null)
                    MessageBox.Show("Вы не выбрали <<Финансовую запись>>!!");

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
    }
}