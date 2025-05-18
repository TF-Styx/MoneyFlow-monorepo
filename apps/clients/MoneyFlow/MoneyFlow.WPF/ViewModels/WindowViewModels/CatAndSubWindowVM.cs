using Microsoft.Win32;
using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Helpers;
using MoneyFlow.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.WindowViewModels
{
    class CatAndSubWindowVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly IFinancialRecordService _financialRecordService;

        private readonly ICreateCatLinkSubUseCase _createCatLinkSubUseCase;
        private readonly IDeleteCatLinkSubUseCase _deleteCatLinkSubUseCase;
        private readonly IGetCatLinkSubUseCase _getCatLinkSubUseCase;

        private readonly INavigationPages _navigationPages;
        private readonly INavigationWindows _navigationWindows;

        public CatAndSubWindowVM(IAuthorizationService authorizationService,
                              ICategoryService categoryService,
                              ISubcategoryService subcategoryService,
                              IFinancialRecordService financialRecordService,

                              ICreateCatLinkSubUseCase createCatLinkSubUseCase,
                              IDeleteCatLinkSubUseCase deleteCatLinkSubUseCase,
                              IGetCatLinkSubUseCase getCatLinkSubUseCase,

                              INavigationPages navigationPages,
                              INavigationWindows navigationWindows)
        {
            _authorizationService = authorizationService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _financialRecordService = financialRecordService;

            _createCatLinkSubUseCase = createCatLinkSubUseCase;
            _deleteCatLinkSubUseCase = deleteCatLinkSubUseCase;
            _getCatLinkSubUseCase = getCatLinkSubUseCase;

            _navigationPages = navigationPages;
            _navigationWindows = navigationWindows;

            CurrentUser = _authorizationService.CurrentUser;

            GetCategory();
            GetIdUserAllSubcategory();
        }

        #region ViewModel CategoryWithSubcategory

        private object? _initialCategoriesParameter;
        private object? _initialCategoriesWithSubcategoriesParameter;
        private object? _initialSubcategoriesParameter;

        private ParameterType _initialCategoryTypeParameter = ParameterType.None;

        public void Initialize(object? parameter, ParameterType typeParameter)
        {
            if (parameter is CategoriesWithSubcategoriesDTO catWithSub)
            {
                _initialCategoriesParameter = catWithSub;
                _initialCategoryTypeParameter = typeParameter;
            }

            if (parameter is ValueTuple<CategoriesWithSubcategoriesDTO, SubcategoryDTO> tuple)
            {
                _initialCategoriesWithSubcategoriesParameter = tuple.Item1;
                _initialSubcategoriesParameter = tuple.Item2;
            }
        }

        public async void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is CategoriesWithSubcategoriesDTO catWithSub)
            {
                var cat = Categories.FirstOrDefault(x => x.IdCategory == catWithSub.Category.IdCategory);

                if (cat is null)
                    return;

                SelectedCategory = cat;
            }

            if (parameter is ValueTuple<CategoriesWithSubcategoriesDTO, SubcategoryDTO> tuple)
            {
                SelectedCategory = Categories.FirstOrDefault(x => x.IdCategory == tuple.Item1.Category.IdCategory);
                SelectedSubcategory = Subcategories.FirstOrDefault(x => x.IdSubcategory == tuple.Item2.IdSubcategory);
            }

            #region Пока не надо

            //if (parameter is ValueTuple<CategoryDTO, SubcategoryDTO> value && typeParameter == ParameterType.CatWithSubWindowWithOutTuple)
            //{
            //    SelectedCategory = value.Item1;
            //    MessageBox.Show(SelectedCategory.CategoryName);
            //}

            //if (parameter is ValueTuple<CategoryDTO, SubcategoryDTO> value1 && typeParameter == ParameterType.CatWithSubWindowWithTuple)
            //{
            //    SelectedCategory = value1.Item1;
            //    MessageBox.Show(SelectedCategory.CategoryName);

            //    SelectedSubcategory = value1.Item2;
            //    MessageBox.Show(SelectedSubcategory.SubcategoryName);
            //}

            #endregion
        }

        #endregion

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


        #region Категории пользователя

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

        private string _descriptionCat;
        public string DescriptionCat
        {
            get => _descriptionCat;
            set
            {
                _descriptionCat = value;
                OnPropertyChanged();
            }
        }

        // TODO : Сделать выбор цветов
        private string _selectedColorCat;
        public string SelectedColorCat
        {
            get => _selectedColorCat;
            set
            {
                _selectedColorCat = value;
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

        private CategoryDTO _selectedCategory;
        public CategoryDTO SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;

                if (value == null) { return; }

                SubstitutingCategories(value.IdCategory);

                GetSubcategoryByIdCategory();

                OnPropertyChanged();
            }
        }

        private CategoryDTO _currentCategory;

        private void SubstitutingCategories(int idCategory)
        {
            _currentCategory = _categoryService.GetCategory(idCategory);

            CategoryName = _currentCategory.CategoryName;
            DescriptionCat = _currentCategory.Description;
            //SelectedColorCat = _currentCatWithSub.Category.Color;
            SelectImageCat = _currentCategory.Image;
        }

        //public ObservableCollection<Color> ColorCat { get; set; } = [];

        public ObservableCollection<CategoryDTO> Categories { get; set; } = [];

        public List<CategoryDTO> SelectedCategories { get; set; } = [];

        private async Task GetCategory()
        {
            Categories.Clear();

            var list = await _categoryService.GetCatAsyncCategory(CurrentUser.IdUser);

            foreach (var item in list)
            {
                Categories.Add(item);
                var index = Categories.IndexOf(item);
                item.Index = index + 1;
            }

            Update((_initialCategoriesWithSubcategoriesParameter, _initialSubcategoriesParameter), _initialCategoryTypeParameter);
            Update(_initialCategoriesParameter, _initialCategoryTypeParameter);
        }

        private RelayCommand _allSubCommand;
        public RelayCommand AllSubCommand
        {
            get => _allSubCommand ??= new(obj =>
            {
                CategoryName = string.Empty;
                DescriptionCat = string.Empty;
                //SelectedColorCat = null;
                SelectImageCat = null;

                SubcategoryName = string.Empty;
                DescriptionSub = string.Empty;
                //SelectedColorCat = null;
                SelectImageSub = null;

                SelectedCategory = null;
                SelectedSubcategory = null;

                GetIdUserAllSubcategory();
            });
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Команды Категорий

        private RelayCommand _categoryAddCommand;
        public RelayCommand CategoryAddCommand
        {
            get => _categoryAddCommand ??= new(async obj =>
            {
                var newCat = await _categoryService.CreateAsync(CategoryName, DescriptionCat, SelectedColorCat, SelectImageCat, CurrentUser.IdUser);

                if (newCat.Message != string.Empty)
                {
                    MessageBox.Show(newCat.Message);
                    return;
                }

                //if (newCat.CategoryDTO == null)
                //{
                //    // Это не должно происходить при текущей логике сервиса, если сообщение пустое,
                //    // но это хорошая защитная проверка.
                //    MessageBox.Show("Произошла неизвестная ошибка при создании категории.");
                //    // Возможно, здесь стоит залогировать ошибку
                //    return;
                //}

                newCat.CategoryDTO.Index = Categories.Count + 1;
                Categories.Add(newCat.CategoryDTO);
                
                var subcategory = await _subcategoryService.GetByIdSub(CurrentUser.IdUser, newCat.CategoryDTO.IdCategory);
                subcategory.Index = Subcategories.Count + 1;
                Subcategories.Add(subcategory);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, newCat.CategoryDTO, ParameterType.Add);
                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, subcategory, ParameterType.Add);

                _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, newCat.CategoryDTO, ParameterType.Add);
                _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, subcategory, ParameterType.Add);
            });
        }

        private RelayCommand _categoryUpdateCommand;
        public RelayCommand CategoryUpdateCommand
        {
            get => _categoryUpdateCommand ??= new(async obj =>
            {
                var idUpdateCat = await _categoryService.UpdateAsyncCategory
                    (
                        SelectedCategory.IdCategory,
                        CategoryName,
                        DescriptionCat,
                        SelectedColorCat,
                        SelectImageCat,
                        CurrentUser.IdUser
                    );

                var updateCategory = Categories
                    .FirstOrDefault(x => x.IdCategory == SelectedCategory.IdCategory)
                        .SetProperty(x =>
                        {
                            x.IdCategory = idUpdateCat;
                            x.CategoryName = CategoryName;
                            x.Description = DescriptionCat;
                            x.Color = SelectedColorCat;
                            x.Image = SelectImageCat;
                            x.IdUser = CurrentUser.IdUser;
                        });

                var index = Categories.IndexOf(updateCategory);

                Categories.RemoveAt(index);
                Categories.Insert(index, updateCategory);

                SelectedCategory = updateCategory;

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, updateCategory, ParameterType.Update);
                _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, updateCategory, ParameterType.Update);
            });
        }

        private RelayCommand _categoryDeleteCommand;
        public RelayCommand CategoryDeleteCommand
        {
            get => _categoryDeleteCommand ??= new(async obj =>
            {
                if (SelectedCategories.Count > 1)
                {
                    if (MessageBox.Show($"Вы точно хотите удалить <{SelectedCategories.Count}> кол-во категорий?\n" +
                        $"Удаляться так же все связанные данные!!", "Предупреждение!!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        var categoriesToRemove = SelectedCategories.ToList();

                        foreach (var item in categoriesToRemove)
                        {
                            var foreachMessage = await _categoryService.ExistRelatedDataAsync(SelectedCategory.IdCategory);

                            var financialRecordDelete = await _financialRecordService.DeleteListAsync(SelectedCategory.IdCategory, true);
                            var categoryDelete = await _categoryService.DeleteAsync(CurrentUser.IdUser, SelectedCategory.IdCategory, true);

                            _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, financialRecordDelete, ParameterType.Delete);
                            _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, financialRecordDelete, ParameterType.Delete);

                            _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, SelectedCategory, ParameterType.Delete);
                            _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, SelectedCategory, ParameterType.Delete);

                            Categories.Remove(Categories.FirstOrDefault(x => x.IdCategory == item.IdCategory));
                        }

                        CategoryName = string.Empty;
                        DescriptionCat = string.Empty;
                        //SelectedColorCat = null;
                        SelectImageCat = null;

                        SelectedCategory = null;

                        return;
                    }
                }
                else
                {
                    var message = await _categoryService.ExistRelatedDataAsync(SelectedCategory.IdCategory);

                    if (message != null)
                    {
                        if (MessageBox.Show(message, "Предупреждение!!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            var financialRecordDelete = await _financialRecordService.DeleteListAsync(SelectedCategory.IdCategory, true);
                            var categoryDelete = await _categoryService.DeleteAsync(CurrentUser.IdUser, SelectedCategory.IdCategory, true);

                            _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, financialRecordDelete, ParameterType.Delete);
                            _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, financialRecordDelete, ParameterType.Delete);

                            _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, SelectedCategory, ParameterType.Delete);
                            _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, SelectedCategory, ParameterType.Delete);

                            Categories.Remove(SelectedCategory);

                            CategoryName = string.Empty;
                            DescriptionCat = string.Empty;
                            //SelectedColorCat = null;
                            SelectImageCat = null;

                            SelectedCategory = null;
                        }
                    }
                    else
                    {
                        var financialRecordDelete = await _financialRecordService.DeleteListAsync(SelectedCategory.IdCategory, true);
                        var categoryDelete = await _categoryService.DeleteAsync(CurrentUser.IdUser, SelectedCategory.IdCategory, true);

                        _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, financialRecordDelete, ParameterType.Delete);
                        _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, financialRecordDelete, ParameterType.Delete);

                        _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, SelectedCategory, ParameterType.Delete);
                        _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, SelectedCategory, ParameterType.Delete);

                        Categories.Remove(SelectedCategory);

                        CategoryName = string.Empty;
                        DescriptionCat = string.Empty;
                        //SelectedColorCat = null;
                        SelectImageCat = null;

                        SelectedCategory = null;
                    }
                }
            });
        }


        private RelayCommand _selectedImageCatCommand;
        public RelayCommand SelectedImageCatCommand
        {
            get => _selectedImageCatCommand ??= new(async obj => { SelectedCategoryImage(); });
        }

        private RelayCommand _clearImageCatCommand;
        public RelayCommand ClearImageCatCommand
        {
            get => _clearImageCatCommand ??= new(async obj => { SelectImageCat = null; });
        }

        public async void SelectedCategoryImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Выберите (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectImageCat = await ImageHelper.ImageByteArray(openFileDialog.FileName);
            }
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Подкатегории пользователя

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

        private string _descriptionSub;
        public string DescriptionSub
        {
            get => _descriptionSub;
            set
            {
                _descriptionSub = value;
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

        private SubcategoryDTO _selectedSubcategory;
        public SubcategoryDTO SelectedSubcategory
        {
            get => _selectedSubcategory;
            set
            {
                _selectedSubcategory = value;

                if (value == null) { return; }

                SubstitutingSubcategories(value.IdSubcategory);

                //if (SelectedCategory == null)
                //{
                //    var idCategory = _categoryService.GetIdSubCat(CurrentUser.IdUser, SelectedSubcategory.IdSubcategory);

                //    SelectedCategory = Categories.FirstOrDefault(x => x.IdUser == CurrentUser.IdUser && x.IdCategory == idCategory);

                //    CategoryName = SelectedCategory.CategoryName;
                //    DescriptionCat = SelectedCategory.Description;
                //    //SelectedColorCat = SelectedCategory.Color;
                //    SelectImageCat = SelectedCategory.Image;

                //    SubcategoryName = value.SubcategoryName;
                //    DescriptionSub = value.Description;
                //    //SelectedColorCat = value.Color;
                //    SelectImageSub = value.Image;

                //    GetSubcategoryByIdCategory();
                //}

                //SubcategoryName = value.SubcategoryName;
                //DescriptionSub = value.Description;
                //SelectImageSub = value.Image;

                //GetIdUserIdCategorySubcategory();

                OnPropertyChanged();
            }
        }

        private SubcategoryDTO _currentSubcategory;

        private async void SubstitutingSubcategories(int idSubcategory)
        {
            _currentSubcategory = await _subcategoryService.GetAsyncSubcategory(idSubcategory);

            SubcategoryName = _currentSubcategory.SubcategoryName;
            DescriptionSub = _currentSubcategory.Description;
            SelectImageSub = _currentSubcategory.Image;
        }

        //public ObservableCollection<Color> ColorCat { get; set; } = [];

        public ObservableCollection<SubcategoryDTO> Subcategories { get; set; } = [];

        /// <summary>
        /// Заполняет список с учетом пользователя и категории
        /// </summary>
        private async void GetSubcategoryByIdCategory()
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

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Команды Подкатегорий

        private RelayCommand _subcategoryAddCommand;
        public RelayCommand SubcategoryAddCommand
        {
            get => _subcategoryAddCommand ??= new(async obj =>
            {
                var newSub = await _subcategoryService.CreateAsyncSubcategory(SubcategoryName, DescriptionSub, SelectImageSub, CurrentUser.IdUser);

                if (newSub.Message != string.Empty)
                {
                    MessageBox.Show(newSub.Message);
                    return;
                }

                var idCatLinkSub = await _createCatLinkSubUseCase.CreateAsyncCatLinkSub(CurrentUser.IdUser, SelectedCategory.IdCategory, newSub.SubcategoryDTO.IdSubcategory);

                Subcategories.Add(newSub.SubcategoryDTO);

                GetSubcategoryByIdCategory();

                SubcategoryName = string.Empty;
                DescriptionSub = string.Empty;
                //SelectedColorCat = null;
                SelectImageSub = null;

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, newSub.SubcategoryDTO, ParameterType.Add);
                _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, newSub.SubcategoryDTO, ParameterType.Add);
            });
        }

        private RelayCommand _subcategoryUpdateCommand;
        public RelayCommand SubcategoryUpdateCommand
        {
            get => _subcategoryUpdateCommand ??= new(async obj =>
            {
                var idUpdateSub = await _subcategoryService.UpdateAsyncSubcategory
                    (
                        SelectedSubcategory.IdSubcategory,
                        SubcategoryName,
                        DescriptionSub,
                        SelectImageSub
                    );
                //var createCatLinlSub = await _subcategoryService.

                var updateSubcategory = Subcategories
                    .FirstOrDefault(x => x.IdSubcategory == SelectedSubcategory.IdSubcategory)
                        .SetProperty(x =>
                        {
                            x.IdSubcategory = idUpdateSub;
                            x.SubcategoryName = SubcategoryName;
                            x.Description = DescriptionSub;
                            x.Image = SelectImageCat;
                        });

                var index = Subcategories.IndexOf(updateSubcategory);

                Subcategories.RemoveAt(index);
                Subcategories.Insert(index, updateSubcategory);

                SubcategoryName = string.Empty;
                DescriptionSub = string.Empty;
                SelectImageSub = null;

                SelectedSubcategory = null;

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, updateSubcategory, ParameterType.Update);
                _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, updateSubcategory, ParameterType.Update);
            });
        }

        private RelayCommand _subcategoryDeleteCommand;
        public RelayCommand SubcategoryDeleteCommand
        {
            get => _subcategoryDeleteCommand ??= new(async obj =>
            {
                var message = await _subcategoryService.ExistRelatedDataAsync(SelectedSubcategory.IdSubcategory);

                if (message != null)
                {
                    if (MessageBox.Show(message, "Предупреждение!!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        var financialRecordDelete = await _financialRecordService.DeleteListAsync(SelectedSubcategory.IdSubcategory, false);
                        var subcategoryDelete = await _subcategoryService.DeleteAsyncSubcategory(CurrentUser.IdUser, SelectedSubcategory.IdSubcategory, false);

                        _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, financialRecordDelete, ParameterType.Delete);
                        _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, financialRecordDelete, ParameterType.Delete);

                        _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (SelectedCategory, SelectedSubcategory), ParameterType.Delete);
                        _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, (SelectedCategory, SelectedSubcategory), ParameterType.Delete);

                        Subcategories.Remove(SelectedSubcategory);

                        SubcategoryName = string.Empty;
                        DescriptionSub = string.Empty;
                        SelectImageSub = null;

                        SelectedSubcategory = null;
                    }
                }
                else
                {
                    var financialRecordDelete = await _financialRecordService.DeleteListAsync(SelectedSubcategory.IdSubcategory, false);
                    var subcategoryDelete = await _subcategoryService.DeleteAsyncSubcategory(CurrentUser.IdUser, SelectedSubcategory.IdSubcategory, false);

                    _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, financialRecordDelete, ParameterType.Delete);
                    _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, financialRecordDelete, ParameterType.Delete);

                    _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, (SelectedCategory, SelectedSubcategory), ParameterType.Delete);
                    _navigationWindows.TransitObject(WindowType.FinancialRecordWindow, (SelectedCategory, SelectedSubcategory), ParameterType.Delete);

                    Subcategories.Remove(SelectedSubcategory);

                    SubcategoryName = string.Empty;
                    DescriptionSub = string.Empty;
                    SelectImageSub = null;

                    SelectedSubcategory = null;
                }
            });
        }


        private RelayCommand _selectedImageSubCommand;
        public RelayCommand SelectedImageSubCommand
        {
            get => _selectedImageSubCommand ??= new(async obj => { SelectedSubcategoryImage(); });
        }

        private RelayCommand _clearImageSubCommand;
        public RelayCommand ClearImageSubCommand
        {
            get => _clearImageSubCommand ??= new(async obj => { SelectImageSub = null; });
        }

        public async void SelectedSubcategoryImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Выберите (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectImageSub = await ImageHelper.ImageByteArray(openFileDialog.FileName);
            }
        }

        #endregion
    }
}
