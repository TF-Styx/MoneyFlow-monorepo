namespace MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces
{
    public interface IGetCatLinkSubUseCase
    {
        Task<int> GetIdCatByIdSub(int idSubcategory);
    }
}