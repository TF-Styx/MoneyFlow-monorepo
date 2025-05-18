using MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CatLinkSubCases
{
    public class GetCatLinkSubUseCase : IGetCatLinkSubUseCase
    {
        private readonly ICatLinkSubRepository _catLinkSubRepository;

        public GetCatLinkSubUseCase(ICatLinkSubRepository catLinkSubRepository)
        {
            _catLinkSubRepository = catLinkSubRepository;
        }

        public async Task<int> GetIdCatByIdSub(int idSubcategory)
        {
            return await _catLinkSubRepository.GetIdCatByIdSub(idSubcategory);
        }
    }
}
