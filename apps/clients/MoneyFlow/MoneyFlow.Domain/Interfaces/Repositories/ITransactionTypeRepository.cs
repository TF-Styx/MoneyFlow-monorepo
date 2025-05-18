using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Domain.Interfaces.Repositories
{
    public interface ITransactionTypeRepository
    {
        Task<int> CreateAsync(string? transactionTypeName, string? description);
        int Create(string? transactionTypeName, string? description);

        Task<List<TransactionTypeDomain>> GetAllAsync();
        List<TransactionTypeDomain> GetAll();

        Task<TransactionTypeDomain> GetAsync(int idTransactionType);
        TransactionTypeDomain Get(int idTransactionType);

        Task<TransactionTypeDomain> GetAsync(string transactionTypeName);
        TransactionTypeDomain Get(string transactionTypeName);

        Task<int> UpdateAsync(int idTransactionType, string? transactionTypeName, string? description);
        int Update(int idTransactionType, string? transactionTypeName, string? description);

        Task DeleteAsync(int idTransactionType);
        void Delete(int idTransactionType);
    }
}