using MoneyFlow.Application.ApplicationModel;
using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Enums;
using MoneyFlow.Application.Extension;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordViewingInterfaces;
using MoneyFlow.Domain.Enums;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.PageViewModels
{
    internal class UserPageVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountService _accountService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IBankService _bankService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IGetCategoryWithSubcategoriesUseCase _getCategoryWithSubcategoriesUseCase;
        private readonly IStatisticsService _statisticsService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly IFinancialRecordService _financialRecordService;
        private readonly IGetFinancialRecordViewingUseCase _getFinancialRecordViewingUseCase;
        private readonly IGetCatLinkSubUseCase _getCatLinkSubUseCase;

        private readonly INavigationPages _navigationPages;
        private readonly INavigationWindows _navigationWindows;

        public UserPageVM(IAuthorizationService authorizationService, 
                          IAccountService accountService, 
                          IAccountTypeService accountTypeService, 
                          IBankService bankService, 
                          IUserService userService,
                          ICategoryService categoryService,
                          IGetCategoryWithSubcategoriesUseCase getCategoryWithSubcategoriesUseCase,
                          IStatisticsService statisticsService,
                          ISubcategoryService subcategoryService,
                          ITransactionTypeService transactionTypeService,
                          IFinancialRecordService financialRecordService, 
                          IGetFinancialRecordViewingUseCase getFinancialRecordViewingUseCase,
                          IGetCatLinkSubUseCase getCatLinkSubUseCase,
                          INavigationPages navigationPages,
                          INavigationWindows navigationWindows)
        {
            _authorizationService = authorizationService;
            _accountService = accountService;
            _accountTypeService = accountTypeService;
            _bankService = bankService;
            _userService = userService;
            _categoryService = categoryService;
            _getCategoryWithSubcategoriesUseCase = getCategoryWithSubcategoriesUseCase;
            _statisticsService = statisticsService;
            _subcategoryService = subcategoryService;
            _transactionTypeService = transactionTypeService;
            _financialRecordService = financialRecordService;
            _getFinancialRecordViewingUseCase = getFinancialRecordViewingUseCase;
            _getCatLinkSubUseCase = getCatLinkSubUseCase;

            _navigationPages = navigationPages;
            _navigationWindows = navigationWindows;

            CurrentUser = _authorizationService.CurrentUser;
            UserTotalInfo = _userService.GetUserInfo(CurrentUser.IdUser);

            foreach (var item in LoadTimePeriod())
            {
                TimePeriods.Add(item);
            }

            SelectedTimePeriod = TimePeriods.FirstOrDefault();

            GetAccount();
            GetUserAccountTypes();
            GetUserBanks();
            GetCategory();
            GetCategoriesWithSubcategories();
            GetIdUserAllSubcategory();

            GetTransactionTypeFilter();
            GetCategoryFilter();
            GetAccountFilter();

            var (Start, End) = DefaultDate();
            DateStartFilter = Start;
            DateEndFilter = End;

            GetFinancialRecord();
            CalculateRecordTransactionByAccount();
            CalculateRecordTransactionByData();
        }

        public async void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            #region Обновление данных счетов и банков

            if (parameter is ValueTuple<AccountDTO, BankDTO> cartageAccountBankAdd && typeParameter is ParameterType.Add)
            {
                var accountToAdd = cartageAccountBankAdd.Item1;
                accountToAdd.Index = Accounts.Count + 1;
                Accounts.Add(cartageAccountBankAdd.Item1);
                UserTotalInfo.AccountCount += 1;

                var banksToAdd = cartageAccountBankAdd.Item2;
                banksToAdd.Index = UserBanks.Banks.Count + 1;
                UserBanks.Banks.Add(cartageAccountBankAdd.Item2);
                UserTotalInfo.BankCount += 1;

                UserTotalInfo.TotalBalance += cartageAccountBankAdd.Item1.Balance;
            }
            if (parameter is ValueTuple<AccountDTO, BankDTO> cartageAccountBankUpdate && typeParameter is ParameterType.Update)
            {
                var itemForDeleteAccount = Accounts.FirstOrDefault(x => x.IdAccount == cartageAccountBankUpdate.Item1.IdAccount);
                var indexAccount = Accounts.IndexOf(itemForDeleteAccount);

                Accounts.Remove(itemForDeleteAccount);
                Accounts.Insert(indexAccount, cartageAccountBankUpdate.Item1);


                var itemForDeleteBank = UserBanks.Banks.FirstOrDefault(x => x.IdBank == cartageAccountBankUpdate.Item2.IdBank);
                var indexBank = UserBanks.Banks.IndexOf(itemForDeleteBank);

                UserBanks.Banks.Remove(itemForDeleteBank);
                UserBanks.Banks.Insert(indexAccount, cartageAccountBankUpdate.Item2);


                if (Accounts.FirstOrDefault(x => x.IdAccount == cartageAccountBankUpdate.Item1.IdAccount).Balance <= cartageAccountBankUpdate.Item1.Balance)
                    UserTotalInfo.TotalBalance -= cartageAccountBankUpdate.Item1.Balance;
                if (Accounts.FirstOrDefault(x => x.IdAccount == cartageAccountBankUpdate.Item1.IdAccount).Balance >= cartageAccountBankUpdate.Item1.Balance)
                    UserTotalInfo.TotalBalance += cartageAccountBankUpdate.Item1.Balance;
            }
            if (parameter is ValueTuple<AccountDTO, BankDTO> cartageAccountBankDelete && typeParameter is ParameterType.Delete)
            {
                Accounts.Remove(Accounts.FirstOrDefault(x => x.IdAccount == cartageAccountBankDelete.Item1.IdAccount));
                UserTotalInfo.AccountCount -= 1;

                UserBanks.Banks.Remove(UserBanks.Banks.FirstOrDefault(x => x.IdBank == cartageAccountBankDelete.Item2.IdBank));
                UserTotalInfo.BankCount -= 1;

                UserTotalInfo.TotalBalance -= cartageAccountBankDelete.Item1.Balance;
            }

            #endregion

            // -----------------------------------------------------------------------------------------------------------------------------

            #region Обновление категорий
                
            if (parameter is CategoryDTO categoryAdd && typeParameter is ParameterType.Add)
            {
                CategoriesWithSubcategories.Add(new CategoriesWithSubcategoriesDTO { Category = categoryAdd, Subcategories = [] });
                categoryAdd.Index = CategoriesWithSubcategories.Count;
                UserTotalInfo.CategoryCount += 1;
            }
            if (parameter is CategoryDTO categoryUpdate && typeParameter is ParameterType.Update)
            {
                #region Страый варинат

                var itemForDelete = Categories.FirstOrDefault(x => x.IdCategory == categoryUpdate.IdCategory);
                var index = Categories.IndexOf(itemForDelete);

                Categories.Remove(itemForDelete);
                Categories.Insert(index, categoryUpdate);

                #endregion
                // ----------------------------------------------------------------------------------------------------------------------------

                var itemForDelete1 = CategoriesWithSubcategories.FirstOrDefault(x => x.Category.IdCategory == categoryUpdate.IdCategory);
                var index1 = CategoriesWithSubcategories.IndexOf(itemForDelete1);
                var catWithSub = new CategoriesWithSubcategoriesDTO { Category = categoryUpdate, Subcategories = itemForDelete1.Subcategories };

                CategoriesWithSubcategories.Remove(itemForDelete1);
                CategoriesWithSubcategories.Insert(index1, catWithSub);
            }
            if (parameter is CategoryDTO categoryDelete && typeParameter is ParameterType.Delete)
            {
                CategoriesWithSubcategories.Remove(CategoriesWithSubcategories.FirstOrDefault(x => x.Category.IdCategory == categoryDelete.IdCategory));

                UserTotalInfo.CategoryCount -= 1;
            }

            #endregion

            // -----------------------------------------------------------------------------------------------------------------------------

            #region Обновление подкатегорий

            if (parameter is SubcategoryDTO subcategoryAdd && typeParameter is ParameterType.Add)
            {
                var idCat = await _getCatLinkSubUseCase.GetIdCatByIdSub(subcategoryAdd.IdSubcategory);
                var cat = CategoriesWithSubcategories.FirstOrDefault(x => x.Category.IdCategory == idCat);

                cat.Subcategories.Add(subcategoryAdd);
                subcategoryAdd.Index = cat.Subcategories.Count;

                UserTotalInfo.SubcategoryCount += 1;
            }
            if (parameter is SubcategoryDTO subcategoryUpdate && typeParameter is ParameterType.Update)
            {
                var idCat = await _getCatLinkSubUseCase.GetIdCatByIdSub(subcategoryUpdate.IdSubcategory);
                var cat = CategoriesWithSubcategories.FirstOrDefault(x => x.Category.IdCategory == idCat);
                var itemForDelete1 = cat.Subcategories.FirstOrDefault(x => x.IdSubcategory == subcategoryUpdate.IdSubcategory);
                var indexSubcategory = cat.Subcategories.IndexOf(itemForDelete1);

                cat.Subcategories.Remove(itemForDelete1);
                cat.Subcategories.Insert(indexSubcategory, subcategoryUpdate);
            }
            if (parameter is ValueTuple<CategoryDTO, SubcategoryDTO> tupleDelete && typeParameter is ParameterType.Delete)
            {
                var cat = CategoriesWithSubcategories.FirstOrDefault(x => x.Category.IdCategory == tupleDelete.Item1.IdCategory);

                cat.Subcategories.Remove(cat.Subcategories.FirstOrDefault(x => x.IdSubcategory == tupleDelete.Item2.IdSubcategory));

                UserTotalInfo.SubcategoryCount -= 1;
            }

            #endregion

            // -----------------------------------------------------------------------------------------------------------------------------

            #region Обновление финаносых записей и пересчет баланса счета, цказанного в записи

            if (parameter is ValueTuple<FinancialRecordViewingDTO, int> cartageFinancialRecordAdd && typeParameter is ParameterType.Add)
            {
                FinancialRecords.Add(cartageFinancialRecordAdd.Item1);

                var accountDTO = _accountService.Get(cartageFinancialRecordAdd.Item1.AccountNumber);

                var account = Accounts.FirstOrDefault(x => x.IdAccount == accountDTO.IdAccount);
                var indexAccount = Accounts.IndexOf(account);

                if (cartageFinancialRecordAdd.Item2 == 1)
                    UserTotalInfo.TotalBalance += cartageFinancialRecordAdd.Item1.Amount;
                else if (cartageFinancialRecordAdd.Item2 == 2)
                    UserTotalInfo.TotalBalance -= cartageFinancialRecordAdd.Item1.Amount;

                Accounts.Remove(account);
                Accounts.Insert(indexAccount, accountDTO);

                UserTotalInfo.FinancialRecordCount += 1;
                UserTotalInfo.AccountCount += 1;
            }
            if (parameter is ValueTuple<FinancialRecordViewingDTO, int> cartageFinancialRecordUpdate && typeParameter is ParameterType.Update)
            {
                var itemForDelete = FinancialRecords.FirstOrDefault(x => x.IdFinancialRecord == cartageFinancialRecordUpdate.Item1.IdFinancialRecord);
                var index = FinancialRecords.IndexOf(itemForDelete);

                FinancialRecords.Remove(itemForDelete);
                FinancialRecords.Insert(index, cartageFinancialRecordUpdate.Item1);

                var accountDTO = _accountService.Get(cartageFinancialRecordUpdate.Item1.AccountNumber);

                var account = Accounts.FirstOrDefault(x => x.IdAccount == accountDTO.IdAccount);
                var indexAccount = Accounts.IndexOf(account);

                Accounts.Remove(account);
                Accounts.Insert(indexAccount, accountDTO);

                if (cartageFinancialRecordUpdate.Item2 == 1)
                    UserTotalInfo.TotalBalance += cartageFinancialRecordUpdate.Item1.Amount;
                else if (cartageFinancialRecordUpdate.Item2 == 2)
                    UserTotalInfo.TotalBalance -= cartageFinancialRecordUpdate.Item1.Amount;
            }
            if (parameter is FinancialRecordViewingDTO financialRecordDelete && typeParameter is ParameterType.Delete)
            {
                FinancialRecords.Remove(FinancialRecords.FirstOrDefault(x => x.IdFinancialRecord == financialRecordDelete.IdFinancialRecord));

                UserTotalInfo.FinancialRecordCount -= 1;
                UserTotalInfo.AccountCount -= 1;
            }

            #endregion

            // -----------------------------------------------------------------------------------------------------------------------------

            #region Обновление данных пользователя

            UserTotalInfo = _userService.GetUserInfo(CurrentUser.IdUser);

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

        private UserTotalInfoDTO _userTotalInfo;
        public UserTotalInfoDTO UserTotalInfo
        {
            get => _userTotalInfo;
            set
            {
                _userTotalInfo = value;
                OnPropertyChanged();
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Статистика

        private KeyValuePair<TimePeriod, string> _selectedTimePeriod;
        public KeyValuePair<TimePeriod, string> SelectedTimePeriod
        {
            get => _selectedTimePeriod;
            set
            {
                _selectedTimePeriod = value;

                // TODO : Добавить перерасчет нужных списков

                CalculateRecordTransactionByData();

                OnPropertyChanged();
            }
        }

        public ObservableCollection<KeyValuePair<TimePeriod, string>> TimePeriods { get; set; } = [];
        private List<KeyValuePair<TimePeriod, string>> LoadTimePeriod()
        {
            return Enum.GetValues<TimePeriod>().ToList().Select(x =>
            {
                string period = x switch
                {
                    TimePeriod.None => "<<Не выбрано!!>>",
                    TimePeriod.Day => "По дням",
                    TimePeriod.Week => "По неделям",
                    TimePeriod.Month => "По месяцам",
                    TimePeriod.Year => "По годам",
                    _ => string.Empty
                };

                return new KeyValuePair<TimePeriod, string>(x, period);
            }).Where(x => x.Value != string.Empty).ToList();
        }

        public ObservableCollection<NameValue> DetailsTransactionByAccount { get; set; } = [];
        private void CalculateRecordTransactionByAccount()
        {
            DetailsTransactionByAccount.Clear();

            var stat = _statisticsService.DetailingTransaction<AccountDTO>(
                [.. FinancialRecords],
                (record, account) => record.AccountNumber == account.NumberAccount,
                type => (SelectedTransactionTypeFilter?.TransactionTypeName == "<<Не выбрано!!>>") ?
                    (type.IdTransactionType == (int)TransactionType.Profit || type.IdTransactionType == (int)TransactionType.Expenses) :
                    type.IdTransactionType == SelectedTransactionTypeFilter?.IdTransactionType,
                [.. Accounts],
                account => account.NumberAccount.ToString());

            foreach (var item in stat)
            {
                DetailsTransactionByAccount.Add(item);
            }
        }


        public ObservableCollection<NameValue> DetailsTransactionByData { get; set; } = [];
        private void CalculateRecordTransactionByData()
        {
            DetailsTransactionByData.Clear();

            Action action = SelectedTimePeriod.Key switch
            {
                #region Первый вариант

                //TimePeriod.Day => () =>
                //{
                //    var startDay = FinancialRecords.Min(date => date.Date.GetValueOrDefault().Date);
                //    var endDay = FinancialRecords.Max(date => date.Date.GetValueOrDefault().Date);

                //    var dateList = new List<DateTime>();

                //    for (DateTime date = startDay; date <= endDay; date.AddDays(1))
                //    {
                //        dateList.Add(date);
                //    }

                //    var groupedRecord = FinancialRecords.GroupBy(date => date.Date.GetValueOrDefault().Date);

                //    var result = dateList.GroupJoin
                //    (
                //        groupedRecord,
                //        date => date,
                //        group => group.Key,
                //        (date, groups) => new
                //        {
                //            Date = date,
                //            Groups = groups
                //        }
                //    ).SelectMany
                //    (
                //        joined => joined.Groups.DefaultIfEmpty(),
                //        (joined, group) => new NameValue()
                //        {
                //            Name = joined.Date.ToString("yyyy-MM-dd"),
                //            Value = group.Sum(x => x.Amount),

                //            ConvertValue = joined.Date.ToOADate(),
                //        }
                //    ).OrderBy(date => date.ConvertValue).ToList();

                //    foreach (var item in result)
                //    {
                //        DetailsTransactionByData.Add(item);
                //    }
                //},

                #endregion

                TimePeriod.Day => () =>
                {
                    // С нулями на датах
                    //var startDay = FinancialRecords.Min(date => date.Date.GetValueOrDefault().Date);
                    //var endDay = FinancialRecords.Max(date => date.Date.GetValueOrDefault().Date);

                    //var allDates = new List<DateTime>();

                    //for (DateTime date = startDay; date <= endDay; date = date.AddDays(1))
                    //{
                    //    allDates.Add(date);
                    //}

                    var stat = _statisticsService.DetailingTransaction
                        (
                            [.. FinancialRecords],
                            (record, day) => record.Date.Value.Date.Day == day.Value.Day,
                            type => (SelectedTransactionTypeFilter?.TransactionTypeName == "<<Не выбрано!!>>") ?
                                (type.IdTransactionType == (int)TransactionType.Profit || type.IdTransactionType == (int)TransactionType.Expenses) :
                                type.IdTransactionType == SelectedTransactionTypeFilter?.IdTransactionType,
                            FinancialRecords.Select(x => x.Date).Distinct().ToList(), // allDates
                            day => $"{day.Value.Day} : {day.Value.Month} : {day.Value.Year}"
                            //day => $"День: {day.Value.Day}, Месяц: {day.Value.Month}, Год: {day.Value.Year}"
                            //day => day?.Date.ToString("dd-MM-yyyy")
                        );

                    foreach (var item in stat)
                    {
                        DetailsTransactionByData.Add(item);
                    }
                },

                TimePeriod.Week => () =>
                {
                    // С нулями на датах
                    var startDay = FinancialRecords.Min(date => date.Date.GetValueOrDefault().Date);
                    var endDay = FinancialRecords.Max(date => date.Date.GetValueOrDefault().Date);

                    var allWeeks = new List<(int Year, int Month, int WeekOfMonth)>();
                    DateTime currentDate = startDay;

                    while (currentDate <= endDay)
                    {
                        int year = currentDate.Year;
                        int month = currentDate.Month;
                        int weekOfMonth = GetWeekOfMonth(currentDate);

                        allWeeks.Add((year, month, weekOfMonth));
                        currentDate = currentDate.AddDays(7);
                    }

                    allWeeks = allWeeks.Distinct().ToList();
                    
                    int GetWeekOfMonth(DateTime dateTime)
                    {
                        DateTime firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
                        int weekNumber = (dateTime.Day + (int)firstDayOfMonth.DayOfWeek + 6) / 7; // + 6 часов / 7 минут на неделю, начиная с понедельника

                        return weekNumber;
                    }

                    var groupedRecord = FinancialRecords.GroupBy
                        (
                            r => new
                            {
                                Year = r.Date.GetValueOrDefault().Year,
                                Month = r.Date.GetValueOrDefault().Month,
                                WeekOfMonth = GetWeekOfMonth(r.Date.GetValueOrDefault()),
                            }
                        );

                    var result = allWeeks.GroupJoin
                        (
                            groupedRecord,
                            week => (week.Year, week.Month, week.WeekOfMonth),
                            group => (group.Key.Year, group.Key.Month, group.Key.WeekOfMonth),
                            (week, group) => new
                            {
                                Week = week,
                                Group = group
                            }
                        ).SelectMany
                        (
                            joined => joined.Group.DefaultIfEmpty(),
                            (joined, group) => new NameValue()
                            {
                                Name = $"{joined.Week.Year}:{joined.Week.Month:D2}: Неделя№ {joined.Week.WeekOfMonth}",
                                Value = group == null ? 0 : group.Sum(x => x.Amount),
                                ConvertValue = joined.Week.Year * 10000 + joined.Week.Month * 100 + joined.Week.WeekOfMonth,
                            }
                        ).OrderBy(x => x.ConvertValue).ToList();

                    foreach (var item in result)
                    {
                        DetailsTransactionByData.Add(item);
                    }
                },

                TimePeriod.Month => () =>
                {
                    var months = Enum.GetValues<Month>().ToDictionary(month => ((int)month), month => month);
                    var allMonths = new List<KeyValuePair<int, Month>>();

                    allMonths.AddRange(months.Select(x => x));

                    var stat = _statisticsService.DetailingTransaction<KeyValuePair<int, Month>>
                        (
                            [.. FinancialRecords],
                            (record, month) => record.Date.Value.Month == month.Key,
                            type => (SelectedTransactionTypeFilter?.TransactionTypeName == "<<Не выбрано!!>>") ?
                                (type.IdTransactionType == (int)TransactionType.Profit || type.IdTransactionType == (int)TransactionType.Expenses) :
                                type.IdTransactionType == SelectedTransactionTypeFilter?.IdTransactionType,
                            allMonths,
                            month => month.Value.ToString()
                        );

                    foreach (var item in stat)
                    {
                        DetailsTransactionByData.Add(item);
                    }
                },

                TimePeriod.Year => () =>
                {
                    var allYear = FinancialRecords.Select(x => x.Date.GetValueOrDefault().Year).Distinct();

                    var stat = _statisticsService.DetailingTransaction
                        (
                            [.. FinancialRecords],
                            (record, year) => record.Date.Value.Year == year,
                            type => (SelectedTransactionTypeFilter?.TransactionTypeName == "<<Не выбрано!!>>") ?
                                (type.IdTransactionType == (int)TransactionType.Profit || type.IdTransactionType == (int)TransactionType.Expenses) :
                                type.IdTransactionType == SelectedTransactionTypeFilter?.IdTransactionType,
                            allYear.ToList(),
                            year => $"Год: {year}"
                        );

                    foreach (var item in stat)
                    {
                        DetailsTransactionByData.Add(item);
                    }
                },

                _ => () => { },
            };
            action?.Invoke();
        }

        public ObservableCollection<NameValue> DetailsTransactionByCategory { get; set; } = [];
        private void CalculateRecordTransactionByCategory()
        {
            DetailsTransactionByCategory.Clear();

            var stat = _statisticsService.DetailingTransaction<CategoryDTO>
                (
                    [.. FinancialRecords],
                    (record, category) => record.IdCategory == category.IdCategory,
                    type => (SelectedTransactionTypeFilter?.TransactionTypeName == "<<Не выбрано!!>>") ?
                        (type.IdTransactionType == (int)TransactionType.Profit || type.IdTransactionType == (int)TransactionType.Expenses) :
                        type.IdTransactionType == SelectedTransactionTypeFilter?.IdTransactionType,
                    [.. Categories],
                    category => category.CategoryName
                );

            foreach (var item in stat)
            {
                DetailsTransactionByCategory.Add(item);
            }
        }


        public ObservableCollection<NameValue> DetailsTransactionBySubcategory { get; set; } = [];
        private void CalculateRecordTransactionBySubcategory()
        {
            DetailsTransactionBySubcategory.Clear();

            var stat = _statisticsService.DetailingTransaction<SubcategoryDTO>
                (
                    [.. FinancialRecords],
                    (record, subcategory) => record.IdSubcategory == subcategory.IdSubcategory,
                    type => (SelectedTransactionTypeFilter?.TransactionTypeName == "<<Не выбрано!!>>") ?
                        (type.IdTransactionType == (int)TransactionType.Profit || type.IdTransactionType == (int)TransactionType.Expenses) :
                        type.IdTransactionType == SelectedTransactionTypeFilter?.IdTransactionType,
                    [.. Subcategories],
                    category => category.SubcategoryName
                );

            foreach (var item in stat)
            {
                DetailsTransactionBySubcategory.Add(item);
            }
        }

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Счета пользователя

        private AccountDTO _selectedAccount;
        public AccountDTO SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
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

        private RelayCommand _accountDoubleClickCommand;
        public RelayCommand AccountDoubleClickCommand => _accountDoubleClickCommand ??= new RelayCommand(DoubleClick);

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Банк пользователя

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
                OnPropertyChanged();
            }
        }

        private async Task GetUserBanks()
        {
            UserBanks = await _bankService.GetByIdUserAsync(CurrentUser.IdUser);
        }

        private RelayCommand _userBankDoubleClickCommand;
        public RelayCommand UserBankDoubleClickCommand => _userBankDoubleClickCommand ??= new RelayCommand(DoubleClick);

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


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

        private RelayCommand _userAccountTypeDoubleClickCommand;
        public RelayCommand UserAccountTypeDoubleClickCommand => _userAccountTypeDoubleClickCommand ??= new RelayCommand(DoubleClick);

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Категории пользователя

        private CategoryDTO _selectedCategory;
        public CategoryDTO SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;

                if (value == null) { return; }

                GetIdUserIdCategorySubcategory();

                OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryDTO> Categories { get; set; } = [];

        private async void GetCategory()
        {
            Categories.Clear();

            var list = await _categoryService.GetCatAsyncCategory(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Categories.Add(item);
                var index = Categories.IndexOf(item);
                item.Index = index + 1;
            }
        }

        private RelayCommand _allSubCommand;
        public RelayCommand AllSubCommand
        {
            get => _allSubCommand ??= new(obj =>
            {
                GetIdUserAllSubcategory();
            });
        }

        private RelayCommand _categoryInfoCommand;
        public RelayCommand CategoryInfoCommand => _categoryInfoCommand ??= new RelayCommand(CategoryInfoClick);

        private void CategoryInfoClick(object parameter)
        {
            if (parameter is CategoriesWithSubcategoriesDTO catWithSub)
            {
                //_navigationWindows.OpenWindow(WindowType.CatAndSubWindow);
                _navigationWindows.OpenWindow(WindowType.CatAndSubWindow, catWithSub, ParameterType.None);
            }
        }

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Категории Подкатегории

        private CategoriesWithSubcategoriesDTO _selectedCategoriesWithSubcategories;
        public CategoriesWithSubcategoriesDTO SelectedCategoriesWithSubcategories
        {
            get => _selectedCategoriesWithSubcategories;
            set
            {
                _selectedCategoriesWithSubcategories = value;

                SelectedSubcategory = null;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoriesWithSubcategoriesDTO> CategoriesWithSubcategories { get; set; } = [];

        private async void GetCategoriesWithSubcategories()
        {
            CategoriesWithSubcategories.Clear();

            var list = await _getCategoryWithSubcategoriesUseCase.GetCategoryWithSubcategories(CurrentUser.IdUser);

            foreach (var item in list)
            {
                CategoriesWithSubcategories.Add(item);

                var indexCat = CategoriesWithSubcategories.Select(x => x.Category).ToList().IndexOf(item.Category);
                item.Category.Index = indexCat + 1;

                foreach (var sub in item.Subcategories)
                {
                    var indexSub = item.Subcategories.IndexOf(sub);
                    sub.Index = indexSub + 1;
                }
            }
        }

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Подкатегории пользователя

        private SubcategoryDTO _selectedSubcategory;
        public SubcategoryDTO SelectedSubcategory
        {
            get => _selectedSubcategory;
            set
            {
                _selectedSubcategory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SubcategoryDTO> Subcategories { get; set; } = [];

        /// <summary>
        /// Заполняет список с учетом пользователя и категории
        /// </summary>
        private async void GetIdUserIdCategorySubcategory()
        {
            Subcategories.Clear();

            var list = _subcategoryService.GetIdUserIdCategorySub(CurrentUser.IdUser, SelectedCategory.IdCategory);
            //var list = _subcategoryService.GetAllIdUserSub(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Subcategories.Add(item);
                var index = Subcategories.IndexOf(item);
                item.Index = index + 1;
            }
        }

        /// <summary>
        /// Заполняет список всеми подкатегориями пользователя
        /// </summary>
        private async void GetIdUserAllSubcategory()
        {
            Subcategories.Clear();

            //var list = _subcategoryService.GetIdUserIdCategorySub(CurrentUser.IdUser, SelectedCategory.IdCategory);
            var list = _subcategoryService.GetAllIdUserSub(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Subcategories.Add(item);
                var index = Subcategories.IndexOf(item);
                item.Index = index + 1;
            }
        }

        private RelayCommand _userSubcategoryDoubleClickCommand;
        public RelayCommand UserSubcategoryDoubleClickCommand => _userSubcategoryDoubleClickCommand ??= new RelayCommand(DoubleClick);

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Финансовые записи пользователя

        private FinancialRecordViewingDTO _selectedFinancialRecord;
        public FinancialRecordViewingDTO SelectedFinancialRecord
        {
            get => _selectedFinancialRecord;
            set
            {
                _selectedFinancialRecord = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FinancialRecordViewingDTO> FinancialRecords { get; set; } = [];

        public async Task GetFinancialRecord()
        {
            FinancialRecords.Clear();

            var filter = new FinancialRecordFilterDTO()
            {
                AmountStart = AmountStartFilter,
                AmountEnd = AmountEndFilter,
                IsConsiderAmount = IsConsiderAmount,

                IdTransactionType = SelectedTransactionTypeFilter?.IdTransactionType == 0 ? null : SelectedTransactionTypeFilter?.IdTransactionType,
                IdCategory = SelectedCategoryFilter?.IdCategory == 0 ? null : SelectedCategoryFilter?.IdCategory,
                IdSubcategory = SelectedSubcategoryFilter?.IdSubcategory == 0 ? null : SelectedSubcategoryFilter?.IdSubcategory,
                IdAccount = SelectedAccountFilter?.IdAccount == 0 ? null : SelectedAccountFilter?.IdAccount,

                DateStart = DateStartFilter,
                DateEnd = DateEndFilter,
                IsConsiderDate = IsConsiderDate,
            };

            var list = await _getFinancialRecordViewingUseCase.GetAllViewingAsync(CurrentUser.IdUser, filter);

            foreach (var item in list)
            {
                FinancialRecords.Add(item);
                var index = FinancialRecords.IndexOf(item);
                item.Index = index + 1;
            }
        }

        private RelayCommand _userFinancialRecordDoubleClickCommand;
        public RelayCommand UserFinancialRecordDoubleClickCommand => _userFinancialRecordDoubleClickCommand ??= new RelayCommand(DoubleClick);

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Навигация

        private void DoubleClick(object parameter)
        {
            MessageBox.Show("Дабл клик");
            if (parameter is AccountDTO account)
            {
                _navigationWindows.OpenWindow(WindowType.AccountWindow, account);
            }
            if (parameter is AccountTypeDTO accountType)
            {
                _navigationWindows.OpenWindow(WindowType.AccountTypeWindow, accountType);
            }
            if (parameter is BankDTO bank)
            {
                _navigationWindows.OpenWindow(WindowType.BankWindow, bank);
            }
            if (parameter is CategoryDTO category)
            {
                _navigationWindows.OpenWindow(WindowType.CatAndSubWindow, category);
            }

            #region Главная страница

            if (parameter is CategoriesWithSubcategoriesDTO categoryWithSubcategory)
            {
                if (SelectedSubcategory is null)
                {
                    _navigationWindows.OpenWindow(WindowType.CatAndSubWindow, (categoryWithSubcategory.Category, SelectedSubcategory), ParameterType.CatWithSubWindowWithOutTuple);
                }
                else
                {
                    _navigationWindows.OpenWindow(WindowType.CatAndSubWindow, (categoryWithSubcategory.Category, SelectedSubcategory), ParameterType.CatWithSubWindowWithTuple);
                }
                
            }
            if (parameter is SubcategoryDTO subcategory)
            {
                MessageBox.Show("Подкатегория");
                _navigationWindows.OpenWindow(WindowType.CatAndSubWindow, (CategoriesWithSubcategories.Where(x => x.Subcategories.Contains(subcategory)).FirstOrDefault(), subcategory));
            }

            #endregion

            if (parameter is FinancialRecordViewingDTO financialRecordViewing)
            {
                _navigationWindows.OpenWindow(WindowType.FinancialRecordWindow, financialRecordViewing);
            }
        }

        // ---------------------------------------------------------------------------------------------------------------------------------

        private RelayCommand _openAccountWindowCommand;
        public RelayCommand OpenAccountWindowCommand
        {
            get => _openAccountWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.AccountWindow);
            });
        }

        private RelayCommand _openAccountTypeWindowCommand;
        public RelayCommand OpenAccountTypeWindowCommand
        {
            get => _openAccountTypeWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.AccountTypeWindow);
            });
        }

        private RelayCommand _openBankWindowCommand;
        public RelayCommand OpenBankWindowCommand
        {
            get => _openBankWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.BankWindow);
            });
        }

        private RelayCommand _openCatAndSubWindowCommand;
        public RelayCommand OpenCatAndSubWindowCommand
        {
            get => _openCatAndSubWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.CatAndSubWindow);
            });
        }

        private RelayCommand _openFinancialRecordWindowCommand;
        public RelayCommand OpenFinancialRecordWindowCommand
        {
            get => _openFinancialRecordWindowCommand ??= new(obj =>
            {
                _navigationWindows.OpenWindow(WindowType.FinancialRecordWindow);
            });
        }

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Фильтрация

        private bool _isOpenPopupFilter;
        public bool IsOpenPopupFilter
        {
            get => _isOpenPopupFilter;
            set
            {
                _isOpenPopupFilter = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _openPopupFilterCommand;
        public RelayCommand OpenPopupFilterCommand { get => _openPopupFilterCommand ??= new(async obj => { IsOpenPopupFilter = true; }); }

        // ---------------------------------------------------------------------------------------------------------------------------------

        #region Поля фильрации

        private decimal? _amountStartFilter;
        public decimal? AmountStartFilter
        {
            get => _amountStartFilter;
            set
            {
                _amountStartFilter = value;
                OnPropertyChanged();
            }
        }

        private decimal? _amountEndFilter;
        public decimal? AmountEndFilter
        {
            get => _amountEndFilter;
            set
            {
                _amountEndFilter = value;
                OnPropertyChanged();
            }
        }

        private bool _isConsiderAmount;
        public bool IsConsiderAmount
        {
            get => _isConsiderAmount;
            set
            {
                _isConsiderAmount = value;
                OnPropertyChanged();
            }
        }


        private TransactionTypeDTO? _selectedTransactionTypeFilter;
        public TransactionTypeDTO? SelectedTransactionTypeFilter
        {
            get => _selectedTransactionTypeFilter;
            set
            {
                _selectedTransactionTypeFilter = value;

                if (value == null) { return; }

                OnPropertyChanged();
            }
        }

        private CategoryDTO? _selectedCategoryFilter;
        public CategoryDTO? SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set
            {
                _selectedCategoryFilter = value;

                if (value == null) { return; }

                GetSubcategoryFilter();

                OnPropertyChanged();
            }
        }

        private SubcategoryDTO? _selectedSubcategoryFilter;
        public SubcategoryDTO? SelectedSubcategoryFilter
        {
            get => _selectedSubcategoryFilter;
            set
            {
                _selectedSubcategoryFilter = value;

                if (value == null) { return; }

                OnPropertyChanged();
            }
        }

        private AccountDTO? _selectedAccountFilter;
        public AccountDTO? SelectedAccountFilter
        {
            get => _selectedAccountFilter;
            set
            {
                _selectedAccountFilter = value;

                if (value == null) { return; }

                OnPropertyChanged();
            }
        }


        private DateTime? _dateStartFilter;
        public DateTime? DateStartFilter
        {
            get => _dateStartFilter;
            set
            {
                _dateStartFilter = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateEndFilter;
        public DateTime? DateEndFilter
        {
            get => _dateEndFilter;
            set
            {
                _dateEndFilter = value;
                OnPropertyChanged();
            }
        }

        private bool _isConsiderDate = true;
        public bool IsConsiderDate
        {
            get => _isConsiderDate;
            set
            {
                _isConsiderDate = value;
                OnPropertyChanged();
            }
        }

        private (DateTime Start, DateTime End) DefaultDate()
        {
            var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

            return (start, end);
        }

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------

        #region Заполнение списка финансовых записей

        public ObservableCollection<TransactionTypeDTO> TransactionTypesFilter { get; set; } = [];
        public async Task GetTransactionTypeFilter()
        {
            TransactionTypesFilter.Clear();

            var list = await _transactionTypeService.GetAllAsyncTransactionType();

            foreach (var item in list)
            {
                TransactionTypesFilter.Add(item);
            }

            TransactionTypesFilter.Insert(0, new TransactionTypeDTO()
            {
                IdTransactionType = 0,
                TransactionTypeName = "<<Не выбрано!!>>",
            });

            SelectedTransactionTypeFilter = TransactionTypesFilter.FirstOrDefault();
        }

        public ObservableCollection<CategoryDTO> CategoriesFilter { get; set; } = [];
        public async Task GetCategoryFilter()
        {
            CategoriesFilter.Clear();

            var list = await _categoryService.GetCatAsyncCategory(CurrentUser.IdUser);

            foreach (var item in list)
            {
                CategoriesFilter.Add(item);
            }

            CategoriesFilter.Insert(0, new CategoryDTO()
            {
                IdCategory = 0,
                CategoryName = "<<Не выбрано!!>>",
            });

            SelectedCategoryFilter = CategoriesFilter.FirstOrDefault();
        }

        public ObservableCollection<SubcategoryDTO> SubcategoriesFilter { get; set; } = [];
        public async Task GetSubcategoryFilter()
        {
            SubcategoriesFilter.Clear();

            if (SelectedCategoryFilter is null) { return; }

            var list = _subcategoryService.GetIdUserIdCategorySub(CurrentUser.IdUser, SelectedCategoryFilter.IdCategory);

            foreach (var item in list)
            {
                SubcategoriesFilter.Add(item);
            }

            SubcategoriesFilter.Insert(0, new SubcategoryDTO()
            {
                IdSubcategory = 0,
                SubcategoryName = "<<Не выбрано!!>>",
            });

            SelectedSubcategoryFilter = SubcategoriesFilter.FirstOrDefault();
        }

        public ObservableCollection<AccountDTO> AccountsFilter { get; set; } = [];
        public async Task GetAccountFilter()
        {
            AccountsFilter.Clear();

            var list = await _accountService.GetAllAsync(CurrentUser.IdUser);

            foreach (var item in list)
            {
                AccountsFilter.Add(item);
            }

            AccountsFilter.Insert(0, new AccountDTO()
            {
                IdAccount = 0,
                NumberAccount = 0,
            });

            SelectedAccountFilter = AccountsFilter.FirstOrDefault();
        }

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------

        private RelayCommand _applyCommand;
        public RelayCommand ApplyCommand 
        { 
            get => _applyCommand ??= new(async obj => 
            { 
                await GetFinancialRecord();
                CalculateRecordTransactionByAccount();
                CalculateRecordTransactionByData();
                CalculateRecordTransactionByCategory();
                CalculateRecordTransactionBySubcategory();
            }); 
        }

        private RelayCommand _dropCommand;
        public RelayCommand DropCommand 
        { 
            get => _dropCommand ??= new(obj => 
            {
                IsConsiderAmount = false;

                SelectedTransactionTypeFilter = TransactionTypesFilter.FirstOrDefault();
                SelectedCategoryFilter = CategoriesFilter.FirstOrDefault();
                SelectedSubcategoryFilter = SubcategoriesFilter.FirstOrDefault();
                SelectedAccountFilter = AccountsFilter.FirstOrDefault();

                var (Start, End) = DefaultDate();
                DateStartFilter = Start;
                DateEndFilter = End;

                IsConsiderDate = false;

                GetFinancialRecord();
            });
        }

        #endregion


        // ---------------------------------------------------------------------------------------------------------------------------------


        #region Сортировка

        private KeyValuePair<Sorting, string> _selectedSorting;
        public KeyValuePair<Sorting, string> SelectedSorting
        {
            get => _selectedSorting;
            set
            {
                _selectedSorting = value;

                SortingRecord(value.Key);

                OnPropertyChanged();
            }
        }
        public ObservableCollection<KeyValuePair<Sorting, string>> SortingCollection { get; set; } = new() 
        { 
            new KeyValuePair<Sorting, string>(Sorting.RecordName, "По имени"),
            new KeyValuePair<Sorting, string>(Sorting.Amount, "По тратам"),
            new KeyValuePair<Sorting, string>(Sorting.Transaction, "По транзакциям"),
            new KeyValuePair<Sorting, string>(Sorting.Category, "По категориям"),
            new KeyValuePair<Sorting, string>(Sorting.Account, "По счетам"),
            new KeyValuePair<Sorting, string>(Sorting.Date, "По дате"),
        };

        private void SortingRecord(Sorting sorting)
        {
            Action action = sorting switch
            {
                Sorting.RecordName => () => FinancialRecords.ReplaceCollection(FinancialRecords.OrderByDescending(x => x.RecordName).ToList()),
                Sorting.Amount => () => FinancialRecords.ReplaceCollection(FinancialRecords.OrderByDescending(x => x.Amount).ToList()),
                Sorting.Transaction => () => FinancialRecords.ReplaceCollection(FinancialRecords.OrderByDescending(x => x.IdTransactionType).ToList()),
                Sorting.Category => () => FinancialRecords.ReplaceCollection(FinancialRecords.OrderByDescending(x => x.IdCategory).ToList()),
                Sorting.Account => () => FinancialRecords.ReplaceCollection(FinancialRecords.OrderByDescending(x => x.AccountNumber).ToList()),
                Sorting.Date => () => FinancialRecords.ReplaceCollection(FinancialRecords.OrderByDescending(x => x.Date).ToList()),

                _ => () => { throw new Exception("Такая сортировка не реализована!"); },
            };
            action.Invoke();
        }

        private RelayCommand _reversListCommand;
        public RelayCommand ReversListCommand
        {
            get => _reversListCommand ??= new(obj =>
            {
                FinancialRecords.ReversList();
            });
        }

        #endregion
    }
}
