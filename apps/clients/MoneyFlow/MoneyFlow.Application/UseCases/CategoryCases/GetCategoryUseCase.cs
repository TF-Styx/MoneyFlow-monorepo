using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CategoryCases
{
    public class GetCategoryUseCase : IGetCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var toDTO = categories.ToListDTO();

            return toDTO;
        }
        public List<CategoryDTO> GetAllCategory()
        {
            var categories = _categoryRepository.GetAll();
            var toDTO = categories.ToListDTO();

            return toDTO;
        }

        public int GetIdCatByIdUser(int idUser)
        {
            return _categoryRepository.GetIdCat(idUser);
        }
        public int GetIdSubCat(int idUser, int idSub)
        {
            return _categoryRepository.GetIdSubCat(idUser, idSub);
        }

        //public async Task<CategoryDTO> GetIdCatAsync(int idUser)
        //{
        //    var cat = await _categoryRepository.GetIdCatAsync(idUser);

        //    if (cat == null) { return null; }

        //    var toDTO = cat.ToDTO();

        //    return toDTO.CategoryDTO;
        //}
        //public CategoryDTO GetIdCat(int idUser)
        //{
        //    var cat = _categoryRepository.GetIdCat(idUser);

        //    if (cat == null) { return null; }

        //    var toDTO = cat.ToDTO();

        //    return toDTO.CategoryDTO;
        //}

        public async Task<List<CategoryDTO>> GetCatAsyncCategory(int idUser)
        {
            var categories = await _categoryRepository.GetCatAsync(idUser);

            if (idUser == null) { return null; }

            var toDTO = categories.ToListDTO();

            return toDTO;
        }
        public List<CategoryDTO> GetCatCategory(int idUser)
        {
            var categories = _categoryRepository.GetCat(idUser);

            if (idUser == null) { return null; }

            var toDTO = categories.ToListDTO();

            return toDTO;
        }

        public async Task<CategoryDTO> GetAsyncCategory(int idCategory)
        {
            var categories = await _categoryRepository.GetAsync(idCategory);

            if (categories == null) { return null; }

            var toDTO = categories.ToDTO();

            return toDTO.CategoryDTO;
        }
        public CategoryDTO GetCategory(int idCategory)
        {
            var categories = _categoryRepository.Get(idCategory);

            if (categories == null) { return null; }

            var toDTO = categories.ToDTO();

            return toDTO.CategoryDTO;
        }

        public async Task<int?> GetById(int idFinancialRecord)
        {
            return await _categoryRepository.GetById(idFinancialRecord);
        }

        public async Task<CategoryDTO> GetAsyncCategory(string categoryName)
        {
            var categories = await _categoryRepository.GetAsync(categoryName);

            if (categories == null) { return null; }

            var toDTO = categories.ToDTO();

            return toDTO.CategoryDTO;
        }
        public CategoryDTO GetCategory(string categoryName)
        {
            var categories = _categoryRepository.Get(categoryName);

            if (categories == null) { return null; }

            var toDTO = categories.ToDTO();

            return toDTO.CategoryDTO;
        }
    }
}
