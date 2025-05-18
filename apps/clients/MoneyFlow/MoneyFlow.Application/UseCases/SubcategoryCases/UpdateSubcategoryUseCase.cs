using MoneyFlow.Application.UseCaseInterfaces.SubcategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.SubcategoryCases
{
    public class UpdateSubcategoryUseCase : IUpdateSubcategoryUseCase
    {
        private readonly ISubcategoryRepository _subcategoryRepository;

        public UpdateSubcategoryUseCase(ISubcategoryRepository subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        public async Task<int> UpdateAsyncSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image)
        {
            var exist = await _subcategoryRepository.GetAsync(idSubcategory);

            if (exist == null)
            {
                throw new Exception("Данной подкатегории не существует!!");
            }

            return await _subcategoryRepository.UpdateAsync(idSubcategory, subcategoryName, description, image);
        }
        public int UpdateSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image)
        {
            var exist = _subcategoryRepository.Get(idSubcategory);

            if (exist == null)
            {
                throw new Exception("Данной подкатегории не существует!!");
            }

            return _subcategoryRepository.Update(idSubcategory, subcategoryName, description, image);
        }
    }
}
