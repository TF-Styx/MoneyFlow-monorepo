using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces
{
    public interface IGetCategoryWithSubcategoriesUseCase
    {
        Task<List<CategoriesWithSubcategoriesDTO>> GetCategoryWithSubcategories(int idUser);
    }
}