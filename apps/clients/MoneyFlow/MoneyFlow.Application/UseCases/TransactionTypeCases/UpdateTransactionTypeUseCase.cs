using MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.TransactionTypeCases
{
    public class UpdateTransactionTypeUseCase : IUpdateTransactionTypeUseCase
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public UpdateTransactionTypeUseCase(ITransactionTypeRepository transactionTypeRepository)
        {
            _transactionTypeRepository = transactionTypeRepository;
        }

        public async Task<int> UpdateAsyncTransactionType(int idTransactionType, string transactionTypeName, string description)
        {
            var existTransactionType = await _transactionTypeRepository.GetAsync(idTransactionType);

            if (existTransactionType == null) { throw new Exception("Данного типа транзакции не существует!!"); }

            return await _transactionTypeRepository.UpdateAsync(idTransactionType, transactionTypeName, description);
        }
        public int UpdateTransactionType(int idTransactionType, string transactionTypeName, string description)
        {
            var existTransactionType = _transactionTypeRepository.Get(idTransactionType);

            if (existTransactionType == null) { throw new Exception("Данного типа транзакции не существует!!"); }

            return _transactionTypeRepository.Update(idTransactionType, transactionTypeName, description);
        }
    }
}
