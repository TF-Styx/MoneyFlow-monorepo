using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces
{
    public interface IGetTransactionTypeUseCase
    {
        Task<List<TransactionTypeDTO>> GetAllAsyncTransactionType();
        List<TransactionTypeDTO> GetAllTransactionType();

        Task<TransactionTypeDTO> GetAsyncTransactionType(int idTransactionType);
        TransactionTypeDTO GetTransactionType(int idTransactionType);

        Task<TransactionTypeDTO> GetAsyncTransactionType(string transactionTypeName);
        TransactionTypeDTO GetTransactionType(string transactionTypeName);
    }
}