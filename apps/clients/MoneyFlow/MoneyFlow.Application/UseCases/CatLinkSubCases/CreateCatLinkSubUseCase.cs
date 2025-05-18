using MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CatLinkSubCases
{
    public class CreateCatLinkSubUseCase : ICreateCatLinkSubUseCase
    {
        private readonly ICatLinkSubRepository _catLinkSubRepository;

        public CreateCatLinkSubUseCase(ICatLinkSubRepository catLinkSubRepository)
        {
            _catLinkSubRepository = catLinkSubRepository;
        }

        // TODO : Додумать
        public async Task<int> CreateAsyncCatLinkSub(int idUser, int idCategory, int idSubcategory)
        {
            var catLinSub = CatLinkSubDomain.Create(idUser, idCategory, idSubcategory);
            var id = await _catLinkSubRepository.CreateAsync(idUser, idCategory, idSubcategory);

            return id;
        }
        public int CreateCatLinkSub(int idUser, int idCategory, int idSubcategory)
        {
            var catLinSub = CatLinkSubDomain.Create(idUser, idCategory, idSubcategory);
            var id = _catLinkSubRepository.Create(idUser, idCategory, idSubcategory);

            return id;
        }
    }
}
