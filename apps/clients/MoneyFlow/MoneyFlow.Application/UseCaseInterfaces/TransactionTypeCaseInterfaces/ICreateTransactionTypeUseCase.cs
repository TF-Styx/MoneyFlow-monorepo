using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces
{
    public interface ICreateTransactionTypeUseCase
    {
        Task<(TransactionTypeDTO TransactionTypeDTO, string Message)> CreateAsyncTransactionType(string? transactionTypeName, string? description);
        (TransactionTypeDTO TransactionTypeDTO, string Message) CreateTransactionType(string? transactionTypeName, string? description);
    }
}