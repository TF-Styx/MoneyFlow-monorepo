namespace MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces
{
    public interface IDeleteCategoryUseCase
    {
        Task<string?> ExistRelatedDataAsync(int idCategory);
        Task<int?> DeleteAsync(int idUser, int idCategory, bool isDeleteByIdCategory);
        int? Delete(int idUser, int idCategory, bool isDeleteByIdCategory);
    }
}