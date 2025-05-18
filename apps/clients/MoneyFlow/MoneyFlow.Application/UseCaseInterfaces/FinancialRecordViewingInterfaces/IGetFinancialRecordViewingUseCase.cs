using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.FinancialRecordViewingInterfaces
{
    public interface IGetFinancialRecordViewingUseCase
    {
        Task<List<FinancialRecordViewingDTO>> GetAllViewingAsync(int idUser, FinancialRecordFilterDTO filter);
        List<FinancialRecordViewingDTO> GetAllViewing(int idUser, FinancialRecordFilterDTO filter);

        Task<FinancialRecordViewingDTO> GetViewingAsync(int idFinancialRecord);
        FinancialRecordViewingDTO GetViewing(int idFinancialRecord);

        Task<FinancialRecordViewingDTO> GetByIdAsync(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory);
        FinancialRecordViewingDTO GetById(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory);
    }
}