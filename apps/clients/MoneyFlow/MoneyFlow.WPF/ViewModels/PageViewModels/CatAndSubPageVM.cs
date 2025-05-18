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

namespace MoneyFlow.WPF.ViewModels.PageViewModels
{
    internal class CatAndSubPageVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;

        private readonly ICreateCatLinkSubUseCase _createCatLinkSubUseCase;
        private readonly IDeleteCatLinkSubUseCase _deleteCatLinkSubUseCase;

        private readonly INavigationPages _navigationPages;

        public CatAndSubPageVM(IAuthorizationService authorizationService,
                              ICategoryService categoryService,
                              ISubcategoryService subcategoryService,

                              ICreateCatLinkSubUseCase createCatLinkSubUseCase,
                              IDeleteCatLinkSubUseCase deleteCatLinkSubUseCase,

                              INavigationPages navigationPages)
        {
            _authorizationService = authorizationService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;

            _createCatLinkSubUseCase = createCatLinkSubUseCase;
            _deleteCatLinkSubUseCase = deleteCatLinkSubUseCase;

            _navigationPages = navigationPages;

            CurrentUser = _authorizationService.CurrentUser;

            GetCategory();
            GetIdUserAllSubcategory();
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is CategoryDTO category)
            {
                SelectedCategory = category;

                CategoryName = category.CategoryName;
                DescriptionCat = category.Description;
                SelectedColorCat = category.Color;
                SelectImageCat = category.Image;
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

                CategoryName = value.CategoryName;
                DescriptionCat = value.Description;
                //SelectedColorCat = value.Color;
                SelectImageCat = value.Image;

                SubcategoryName = string.Empty;
                DescriptionSub = string.Empty;
                //SelectedColorCat = null;
                SelectImageSub = null;

                GetSubcategoryByIdCategory();

                OnPropertyChanged();
            }
        }

        //public ObservableCollection<Color> ColorCat { get; set; } = [];

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

                Categories.Add(newCat.CategoryDTO);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, newCat, ParameterType.Add);
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
            });
        }

        private RelayCommand _categoryDeleteCommand;
        public RelayCommand CategoryDeleteCommand
        {
            get => _categoryDeleteCommand ??= new(async obj =>
            {
                //await _categoryService.DeleteAsyncCategory(SelectedCategory.IdCategory);

                Categories.Remove(SelectedCategory);

                CategoryName = string.Empty;
                DescriptionCat = string.Empty;
                //SelectedColorCat = null;
                SelectImageCat = null;

                SelectedCategory = null;

            });
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

                if (SelectedCategory == null)
                {
                    var idCategory = _categoryService.GetIdSubCat(CurrentUser.IdUser, SelectedSubcategory.IdSubcategory);

                    SelectedCategory = Categories.FirstOrDefault(x => x.IdUser == CurrentUser.IdUser && x.IdCategory == idCategory);

                    CategoryName = SelectedCategory.CategoryName;
                    DescriptionCat = SelectedCategory.Description;
                    //SelectedColorCat = SelectedCategory.Color;
                    SelectImageCat = SelectedCategory.Image;

                    SubcategoryName = value.SubcategoryName;
                    DescriptionSub = value.Description;
                    //SelectedColorCat = value.Color;
                    SelectImageSub = value.Image;

                    GetSubcategoryByIdCategory();
                }

                SubcategoryName = value.SubcategoryName;
                DescriptionSub = value.Description;
                //SelectedColorCat = value.Color;
                SelectImageSub = value.Image;

                //GetIdUserIdCategorySubcategory();

                OnPropertyChanged();
            }
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
                    if (MessageBox.Show(message) == MessageBoxResult.OK)
                    {
                        //await _subcategoryService.DeleteAsyncSubcategory(CurrentUser.IdUser, SelectedSubcategory.IdSubcategory);

                        Subcategories.Remove(SelectedSubcategory);

                        SubcategoryName = string.Empty;
                        DescriptionSub = string.Empty;
                        SelectImageSub = null;

                        SelectedSubcategory = null;
                    }
                }

            });
        }

        #endregion


        // ------------------------------------------------------------------------------------------------------------------------------------------------


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


        // ------------------------------------------------------------------------------------------------------------------------------------------------


        #region Добавление картинок

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
