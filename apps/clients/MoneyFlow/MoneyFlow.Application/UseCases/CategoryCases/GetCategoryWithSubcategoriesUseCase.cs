using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CategoryCases
{
    public class GetCategoryWithSubcategoriesUseCase : IGetCategoryWithSubcategoriesUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryWithSubcategoriesUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoriesWithSubcategoriesDTO>> GetCategoryWithSubcategories(int idUser)
        {
            var entity = await _categoryRepository.GetCategoryWithSubcategoryAsync(idUser);

            return entity.ToListDTO();
        }
    }
}
