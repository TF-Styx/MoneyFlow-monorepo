namespace MoneyFlow.Application.UseCaseInterfaces.SubcategoryCaseInterfaces
{
    public interface IDeleteSubcategoryUseCase
    {
        Task<string?> ExistRelatedDataAsync(int idSubcategory);
        Task<List<int>> DeleteAsync(int idUser, int idSubcategory, bool isDeleteByIdCategory);
        List<int> Delete(int idUser, int idSubcategory, bool isDeleteByIdCategory);
    }
}