namespace MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces
{
    public interface ICreateCatLinkSubUseCase
    {
        Task<int> CreateAsyncCatLinkSub(int idUser, int idCategory, int idSubcategory);
        int CreateCatLinkSub(int idUser, int idCategory, int idSubcategory);
    }
}