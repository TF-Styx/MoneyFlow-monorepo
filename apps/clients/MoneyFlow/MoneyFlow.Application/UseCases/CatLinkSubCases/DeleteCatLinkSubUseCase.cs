using MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CatLinkSubCases
{
    public class DeleteCatLinkSubUseCase : IDeleteCatLinkSubUseCase
    {
        private readonly ICatLinkSubRepository _catLinkSubRepository;

        public DeleteCatLinkSubUseCase(ICatLinkSubRepository catLinkSubRepository)
        {
            _catLinkSubRepository = catLinkSubRepository;
        }

        public async Task<int> DeleteSubcategoryAsync(int IdUser, int idSubcategory, bool isDeleteByIdCategory)
        {
            return await _catLinkSubRepository.DeleteAsync(IdUser, idSubcategory, isDeleteByIdCategory);
        }

        public async Task<(int IdCategory, List<int> IdSubcategories)> DeleteAsyncCategory(int idUser, int IdCategory)
        {
            return await _catLinkSubRepository.DeleteAsyncSubcategory(idUser, IdCategory);
        }
    }
}
