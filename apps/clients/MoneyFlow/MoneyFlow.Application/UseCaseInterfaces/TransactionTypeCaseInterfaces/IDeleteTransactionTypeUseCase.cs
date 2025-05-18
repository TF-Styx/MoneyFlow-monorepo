namespace MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces
{
    public interface IDeleteTransactionTypeUseCase
    {
        Task DeleteAsyncTransactionType(int idTransactionType);
        void DeleteTransactionType(int idTransactionType);
    }
}