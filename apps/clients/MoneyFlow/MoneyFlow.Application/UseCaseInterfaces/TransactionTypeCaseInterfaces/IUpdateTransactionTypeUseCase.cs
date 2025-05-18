namespace MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces
{
    public interface IUpdateTransactionTypeUseCase
    {
        Task<int> UpdateAsyncTransactionType(int idTransactionType, string transactionTypeName, string description);
        int UpdateTransactionType(int idTransactionType, string transactionTypeName, string description);
    }
}