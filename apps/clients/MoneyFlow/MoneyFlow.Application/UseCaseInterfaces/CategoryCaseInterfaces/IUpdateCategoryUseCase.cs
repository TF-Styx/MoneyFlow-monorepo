namespace MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces
{
    public interface IUpdateCategoryUseCase
    {
        Task<int> UpdateAsyncCategory(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser);
        int UpdateCategory(int idCategory, string? categoryName, string? description, string? color, byte[]? image, int idUser);
    }
}