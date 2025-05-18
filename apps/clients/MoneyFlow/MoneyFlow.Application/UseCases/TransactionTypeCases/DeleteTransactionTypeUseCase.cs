using MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.TransactionTypeCases
{
    public class DeleteTransactionTypeUseCase : IDeleteTransactionTypeUseCase
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public DeleteTransactionTypeUseCase(ITransactionTypeRepository transactionTypeRepository)
        {
            _transactionTypeRepository = transactionTypeRepository;
        }

        public async Task DeleteAsyncTransactionType(int idTransactionType)
        {
            await _transactionTypeRepository.DeleteAsync(idTransactionType);
        }
        public void DeleteTransactionType(int idTransactionType)
        {
            _transactionTypeRepository.Delete(idTransactionType);
        }
    }
}
