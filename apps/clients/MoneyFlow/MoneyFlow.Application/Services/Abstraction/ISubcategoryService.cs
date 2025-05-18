using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface ISubcategoryService
    {
        Task<(SubcategoryDTO SubcategoryDTO, string Message)> CreateAsyncSubcategory(string? subcategoryName, string? description, byte[]? image, int idUser);
        (SubcategoryDTO SubcategoryDTO, string Message) CreateSubcategory(string? subcategoryName, string? description, byte[]? image, int idUser);

        Task<List<SubcategoryDTO>> GetAllAsyncSubcategory();
        List<SubcategoryDTO> GetAllSubcategory();

        List<SubcategoryDTO> GetAllIdUserSub(int idUser);

        List<SubcategoryDTO> GetIdUserIdCategorySub(int idUser, int idCategory);

        Task<SubcategoryDTO> GetByIdSub(int idUser, int idCategory);

        Task<int?> GetById(int idFinancialRecord);

        Task<SubcategoryDTO> GetAsyncSubcategory(int idSubcategory);
        Task<SubcategoryDTO> GetAsyncSubcategory(string subcategoryName);

        SubcategoryDTO GetSubcategory(int idSubcategory);
        SubcategoryDTO GetSubcategory(string subcategoryName);

        Task<int> UpdateAsyncSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image);
        int UpdateSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image);

        Task<string?> ExistRelatedDataAsync(int idSubcategory);

        Task<List<int>> DeleteAsyncSubcategory(int idUser, int idSubcategory, bool isDeleteByIdCategory);
        List<int> DeleteSubcategory(int idUser, int idSubcategory, bool isDeleteByIdCategory);
    }
}