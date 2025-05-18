using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface ITransactionTypeService
    {
        Task<(TransactionTypeDTO TransactionTypeDTO, string Message)> CreateAsyncTransactionType(string transactionTypeName, string description);
        (TransactionTypeDTO TransactionTypeDTO, string Message) CreateTransactionType(string transactionTypeName, string description);
        
        Task<List<TransactionTypeDTO>> GetAllAsyncTransactionType();
        List<TransactionTypeDTO> GetAllTransactionType();

        Task<TransactionTypeDTO> GetAsyncTransactionType(int idTransactionType);
        TransactionTypeDTO GetTransactionType(int idTransactionType);

        Task<TransactionTypeDTO> GetAsyncTransactionType(string transactionTypeName);
        TransactionTypeDTO GetTransactionType(string transactionTypeName);

        Task<int> UpdateAsyncTransactionType(int idTransactionType, string transactionTypeName, string description);
        int UpdateTransactionType(int idTransactionType, string transactionTypeName, string description);
        
        Task DeleteAsyncTransactionType(int idTransactionType);
        void DeleteTransactionType(int idTransactionType);
    }
}