using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.SubcategoryCaseInterfaces;

namespace MoneyFlow.Application.Services.Realization
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly ICreateSubcategoryUseCase _createSubcategoryUseCase;
        private readonly IDeleteSubcategoryUseCase _deleteSubcategoryUseCase;
        private readonly IGetSubcategoryUseCase _getSubcategoryUseCase;
        private readonly IUpdateSubcategoryUseCase _updateSubcategoryUseCase;

        public SubcategoryService(ICreateSubcategoryUseCase createSubcategoryUseCase, IDeleteSubcategoryUseCase deleteSubcategoryUseCase, IGetSubcategoryUseCase getSubcategoryUseCase, IUpdateSubcategoryUseCase updateSubcategoryUseCase)
        {
            _createSubcategoryUseCase = createSubcategoryUseCase;
            _deleteSubcategoryUseCase = deleteSubcategoryUseCase;
            _getSubcategoryUseCase = getSubcategoryUseCase;
            _updateSubcategoryUseCase = updateSubcategoryUseCase;
        }

        public async Task<(SubcategoryDTO SubcategoryDTO, string Message)> CreateAsyncSubcategory(string? subcategoryName, string? description, byte[]? image, int idUser)
        {
            return await _createSubcategoryUseCase.CreateAsyncSubcategory(subcategoryName, description, image, idUser);
        }
        public (SubcategoryDTO SubcategoryDTO, string Message) CreateSubcategory(string? subcategoryName, string? description, byte[]? image, int idUser)
        {
            return _createSubcategoryUseCase.CreateSubcategory(subcategoryName, description, image, idUser);
        }

        public async Task<List<SubcategoryDTO>> GetAllAsyncSubcategory()
        {
            return await _getSubcategoryUseCase.GetAllAsyncSubcategory();
        }
        public List<SubcategoryDTO> GetAllSubcategory()
        {
            return _getSubcategoryUseCase.GetAllSubcategory();
        }

        public List<SubcategoryDTO> GetAllIdUserSub(int idUser)
        {
            return _getSubcategoryUseCase.GetAllIdUserSub(idUser);
        }

        public List<SubcategoryDTO> GetIdUserIdCategorySub(int idUser, int idCategory)
        {
            return _getSubcategoryUseCase.GetIdUserIdCategorySub(idUser, idCategory);
        }

        public async Task<SubcategoryDTO> GetByIdSub(int idUser, int idCategory)
        {
            return await _getSubcategoryUseCase.GetByIdSub(idUser, idCategory);
        }

        public async Task<SubcategoryDTO> GetAsyncSubcategory(int idSubcategory)
        {
            return await _getSubcategoryUseCase.GetAsyncSubcategory(idSubcategory);
        }
        public SubcategoryDTO GetSubcategory(int idSubcategory)
        {
            return _getSubcategoryUseCase.GetSubcategory(idSubcategory);
        }

        public async Task<int?> GetById(int idFinancialRecord)
        {
            return await _getSubcategoryUseCase.GetById(idFinancialRecord);
        }

        public async Task<SubcategoryDTO> GetAsyncSubcategory(string subcategoryName)
        {
            return await _getSubcategoryUseCase.GetAsyncSubcategory(subcategoryName);
        }
        public SubcategoryDTO GetSubcategory(string subcategoryName)
        {
            return _getSubcategoryUseCase.GetSubcategory(subcategoryName);
        }

        public async Task<int> UpdateAsyncSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image)
        {
            return await _updateSubcategoryUseCase.UpdateAsyncSubcategory(idSubcategory, subcategoryName, description, image);
        }
        public int UpdateSubcategory(int idSubcategory, string? subcategoryName, string? description, byte[]? image)
        {
            return _updateSubcategoryUseCase.UpdateSubcategory(idSubcategory, subcategoryName, description, image);
        }

        public async Task<string?> ExistRelatedDataAsync(int idSubcategory)
        {
            return await _deleteSubcategoryUseCase.ExistRelatedDataAsync(idSubcategory);
        }

        public async Task<List<int>> DeleteAsyncSubcategory(int idUser, int idSubcategory, bool isDeleteByIdCategory)
        {
            return await _deleteSubcategoryUseCase.DeleteAsync(idUser, idSubcategory, isDeleteByIdCategory);
        }
        public List<int> DeleteSubcategory(int idUser, int idSubcategory, bool isDeleteByIdCategory)
        {
            return _deleteSubcategoryUseCase.Delete(idUser, idSubcategory, isDeleteByIdCategory);
        }
    }
}
