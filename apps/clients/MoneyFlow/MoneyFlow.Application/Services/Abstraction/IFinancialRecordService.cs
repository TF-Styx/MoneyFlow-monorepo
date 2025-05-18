using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IFinancialRecordService
    {
        Task<(FinancialRecordDTO FinancialRecordDTO, string Message)> CreateAsync(string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date);

        Task<List<FinancialRecordDTO>> GetAllAsync(int idUser);
        List<FinancialRecordDTO> GetAll(int idUser);

        Task<FinancialRecordDTO> GetAsync(int idFinancialRecord);
        FinancialRecordDTO Get(int idFinancialRecord);

        //Task<FinancialRecordDTO> GetAsyncFinancialRecord(string recordName);
        //FinancialRecordDTO GetFinancialRecord(string recordName);

        Task<int> UpdateAsync(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date);
        int Update(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date);

        Task<List<int>> DeleteListAsync(int id, bool isDeleteByIdCategory);

        Task<int> DeleteAsync(int idFinancialRecord);
        int Delete(int idFinancialRecord);
    }
}