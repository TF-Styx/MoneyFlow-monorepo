namespace MoneyFlow.Application.UseCaseInterfaces.SubcategoryCaseInterfaces
{
    public interface IUpdateSubcategoryUseCase
    {
        Task<int> UpdateAsyncSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image);
        int UpdateSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image);
    }
}