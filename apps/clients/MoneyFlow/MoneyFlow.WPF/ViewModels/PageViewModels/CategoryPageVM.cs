using Microsoft.Win32;
using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.WPF.Commands;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Helpers;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.Views.Pages;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyFlow.WPF.ViewModels.PageViewModels
{
    internal class CategoryPageVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICategoryService _categoryService;

        private readonly INavigationPages _navigationPages;

        public CategoryPageVM(IAuthorizationService authorizationService, ICategoryService categoryService, INavigationPages navigationPages)
        {
            _authorizationService = authorizationService;
            _categoryService = categoryService;

            _navigationPages = navigationPages;

            CurrentUser = _authorizationService.CurrentUser;
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is CategoryDTO category)
            {
                SelectedCategory = category;
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

                GetCategoryById(value.IdCategory);

                OnPropertyChanged();
            }
        }

        private CategoryDTO _currentCategory;

        private async void GetCategoryById(int idCategory)
        {
            _currentCategory = await _categoryService.GetAsyncCategory(idCategory);

            CategoryName = _currentCategory.CategoryName;
            DescriptionCat = _currentCategory.Description;
            //SelectedColorCat = _currentCategory.Color;
            SelectImageCat = _currentCategory.Image;
        }

        //public ObservableCollection<Color> ColorCat { get; set; } = [];

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

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, newCat.CategoryDTO, ParameterType.Add);
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

                var entity = await _categoryService.GetAsyncCategory(idUpdateCat);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, entity, ParameterType.Update);

                //var updateCategory = Categories
                //    .FirstOrDefault(x => x.IdCategory == SelectedCategory.IdCategory)
                //        .SetProperty(x =>
                //        {
                //            x.IdCategory = idUpdateCat;
                //            x.CategoryName = CategoryName;
                //            x.Description = DescriptionCat;
                //            x.Color = SelectedColorCat;
                //            x.Image = SelectImageCat;
                //            x.IdUser = CurrentUser.IdUser;
                //        });

                //var index = Categories.IndexOf(updateCategory);

                //Categories.RemoveAt(index);
                //Categories.Insert(index, updateCategory);
            });
        }

        private RelayCommand _categoryDeleteCommand;
        public RelayCommand CategoryDeleteCommand
        {
            get => _categoryDeleteCommand ??= new(async obj =>
            {
                //await _categoryService.DeleteAsyncCategory(SelectedCategory.IdCategory);
                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, SelectedCategory, ParameterType.Delete);

                //Categories.Remove(SelectedCategory);

                CategoryName = string.Empty;
                DescriptionCat = string.Empty;
                //SelectedColorCat = null;
                SelectImageCat = null;

                SelectedCategory = null;
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
    }
}
