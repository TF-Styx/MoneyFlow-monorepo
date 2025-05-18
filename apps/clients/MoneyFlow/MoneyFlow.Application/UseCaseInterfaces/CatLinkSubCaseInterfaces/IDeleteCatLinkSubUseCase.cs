namespace MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces
{
    public interface IDeleteCatLinkSubUseCase
    {
        Task<(int IdCategory, List<int> IdSubcategories)> DeleteAsyncCategory(int idUser, int IdCategory);
        Task<int> DeleteSubcategoryAsync(int IdUser, int idSubcategory, bool isDeleteByIdCategory);
    }
}