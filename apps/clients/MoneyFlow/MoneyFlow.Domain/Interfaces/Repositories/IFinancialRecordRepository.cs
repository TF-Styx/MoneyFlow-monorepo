using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Domain.Interfaces.Repositories
{
    public interface IFinancialRecordRepository
    {
        Task<int> CreateAsync(string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date);

        Task<List<FinancialRecordDomain>> GetAllAsync(int idUser);
        List<FinancialRecordDomain> GetAll(int idUser);

        Task<FinancialRecordDomain> GetAsync(int idFinancialRecord);
        FinancialRecordDomain Get(int idFinancialRecord);

        Task<List<FinancialRecordViewingDomain>> GetAllViewingAsync(int idUser, FinancialRecordFilterDomain filter);
        List<FinancialRecordViewingDomain> GetAllViewing(int idUser, FinancialRecordFilterDomain filter);

        Task<FinancialRecordViewingDomain> GetViewingAsync(int idFinancialRecord);
        FinancialRecordViewingDomain GetViewing(int idFinancialRecord);

        Task<FinancialRecordViewingDomain> GetByIdAsync(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory);
        FinancialRecordViewingDomain GetById(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory);

        Task<int> UpdateAsync(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date);
        int Update(int idFinancialRecord, string? recordName, decimal? amount, string? description, int? idTransactionType, int? idUser, int? idCategory, int? idSubcategory, int? idAccount, DateTime? date);

        Task<List<int>> DeleteListAsync(int id, bool isDeleteByIdCategory);

        Task<int> DeleteAsync(int idFinancialRecord);
        int Delete(int idFinancialRecord);
    }
}