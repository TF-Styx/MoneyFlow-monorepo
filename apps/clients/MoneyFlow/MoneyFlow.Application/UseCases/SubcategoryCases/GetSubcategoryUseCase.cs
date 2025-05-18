using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.SubcategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.SubcategoryCases
{
    public class GetSubcategoryUseCase : IGetSubcategoryUseCase
    {
        private readonly ISubcategoryRepository _subcategoryRepository;

        public GetSubcategoryUseCase(ISubcategoryRepository subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        public async Task<List<SubcategoryDTO>> GetAllAsyncSubcategory()
        {
            var subcategories = await _subcategoryRepository.GetAllAsync();
            var toDTO = subcategories.ToListDTO();

            return toDTO;
        }
        public List<SubcategoryDTO> GetAllSubcategory()
        {
            var subcategories = _subcategoryRepository.GetAll();
            var toDTO = subcategories.ToListDTO();

            return toDTO;
        }

        public List<SubcategoryDTO> GetAllIdUserSub(int idUser)
        {
            var sub = _subcategoryRepository.GetAllIdUserSub(idUser);
            var toDTO = sub.ToListDTO();

            return toDTO;
        }

        public List<SubcategoryDTO> GetIdUserIdCategorySub(int idUser, int idCategory)
        {
            var sub = _subcategoryRepository.GetIdUserIdCategorySub(idUser, idCategory);
            var toDTO = sub.ToListDTO();

            return toDTO;
        }

        public async Task<SubcategoryDTO> GetByIdSub(int idUser, int idCategory)
        {
            var sub = await _subcategoryRepository.GetByIdSub(idUser, idCategory);
            var toDTO = sub.ToDTO().SubcategoryDTO;

            return toDTO;
        }

        public async Task<int?> GetById(int idFinancialRecord)
        {
            return await _subcategoryRepository.GetById(idFinancialRecord);
        }

        public async Task<SubcategoryDTO> GetAsyncSubcategory(int idSubcategory)
        {
            var subcategories = await _subcategoryRepository.GetAsync(idSubcategory);

            if (subcategories == null) { return null; }

            var toDTO = subcategories.ToDTO();

            return toDTO.SubcategoryDTO;
        }
        public SubcategoryDTO GetSubcategory(int idSubcategory)
        {
            var subcategories = _subcategoryRepository.Get(idSubcategory);

            if (subcategories == null) { return null; }

            var toDTO = subcategories.ToDTO();

            return toDTO.SubcategoryDTO;
        }

        public async Task<SubcategoryDTO> GetAsyncSubcategory(string subcategoryName)
        {
            var subcategories = await _subcategoryRepository.GetAsync(subcategoryName);

            if (subcategories == null) { return null; }

            var toDTO = subcategories.ToDTO();

            return toDTO.SubcategoryDTO;
        }
        public SubcategoryDTO GetSubcategory(string subcategoryName)
        {
            var subcategories = _subcategoryRepository.Get(subcategoryName);

            if (subcategories == null) { return null; }

            var toDTO = subcategories.ToDTO();

            return toDTO.SubcategoryDTO;
        }
    }
}
