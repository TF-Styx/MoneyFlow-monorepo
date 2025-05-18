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
    internal class SubcategoryPageVM : BaseViewModel, IUpdatable
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;

        private readonly ICreateCatLinkSubUseCase _createCatLinkSubUseCase;
        private readonly IDeleteCatLinkSubUseCase _deleteCatLinkSubUseCase;

        private readonly INavigationPages _navigationPages;

        public SubcategoryPageVM(IAuthorizationService authorizationService,
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
        }

        public void Update(object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (parameter is SubcategoryDTO subcategory)
            {
                if (SelectedCategory == null)
                {
                    SelectedSubcategory = subcategory;

                    var idCategory = _categoryService.GetIdSubCat(CurrentUser.IdUser, SelectedSubcategory.IdSubcategory);

                    SelectedCategory = Categories.FirstOrDefault(x => x.IdUser == CurrentUser.IdUser && x.IdCategory == idCategory);

                    SubcategoryName = SelectedSubcategory.SubcategoryName;
                    DescriptionSub = SelectedSubcategory.Description;
                    SelectImageSub = SelectedSubcategory.Image;
                }
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


        private CategoryDTO _selectedCategory;
        public CategoryDTO SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;

                if (value == null) { return; }

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

                    SubcategoryName = value.SubcategoryName;
                    DescriptionSub = value.Description;
                    SelectImageSub = value.Image;
                }

                SubcategoryName = value.SubcategoryName;
                DescriptionSub = value.Description;
                SelectImageSub = value.Image;

                OnPropertyChanged();
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

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, newSub.SubcategoryDTO, ParameterType.Add);
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

                var entity = await _subcategoryService.GetAsyncSubcategory(idUpdateSub);

                _navigationPages.TransitObject(PageType.UserPage, FrameType.MainFrame, entity, ParameterType.Update);

                //var createCatLinlSub = await _subcategoryService.

                //var updateSubcategory = Subcategories
                //    .FirstOrDefault(x => x.IdSubcategory == SelectedSubcategory.IdSubcategory)
                //        .SetProperty(x =>
                //        {
                //            x.IdSubcategory = idUpdateSub;
                //            x.SubcategoryName = SubcategoryName;
                //            x.Description = DescriptionSub;
                //            x.Image = SelectImageCat;
                //        });

                //var index = Subcategories.IndexOf(updateSubcategory);

                //Subcategories.RemoveAt(index);
                //Subcategories.Insert(index, updateSubcategory);
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

                        //Subcategories.Remove(SelectedSubcategory);

                        SubcategoryName = string.Empty;
                        DescriptionSub = string.Empty;
                        SelectImageSub = null;

                        SelectedSubcategory = null;
                    }
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


        // ------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
