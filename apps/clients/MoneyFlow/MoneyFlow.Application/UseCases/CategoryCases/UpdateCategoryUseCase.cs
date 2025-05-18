using MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CategoryCases
{
    public class UpdateCategoryUseCase : IUpdateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> UpdateAsyncCategory(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            var exist = await _categoryRepository.GetAsync(idCategory);

            if (exist == null) { throw new Exception("Данной категории не существует!!"); }

            return await _categoryRepository.UpdateAsync(idCategory, categoryName, description, color, image, idUser);
        }
        public int UpdateCategory(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            var exist = _categoryRepository.Get(idCategory);

            if (exist == null) { throw new Exception("Данной категории не существует!!"); }

            return _categoryRepository.Update(idCategory, categoryName, description, color, image, idUser);
        }
    }
}
