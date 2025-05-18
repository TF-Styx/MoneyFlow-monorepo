using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces;

namespace MoneyFlow.Application.Services.Realization
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ICreateTransactionTypeUseCase _createTransactionTypeUseCase;
        private readonly IDeleteTransactionTypeUseCase _deleteTransactionTypeUseCase;
        private readonly IGetTransactionTypeUseCase    _getTransactionTypeUseCase;
        private readonly IUpdateTransactionTypeUseCase _updateTransactionTypeUseCase;

        public TransactionTypeService(ICreateTransactionTypeUseCase createTransactionTypeUseCase, IDeleteTransactionTypeUseCase deleteTransactionTypeUseCase, IGetTransactionTypeUseCase getTransactionTypeUseCase, IUpdateTransactionTypeUseCase updateTransactionTypeUseCase)
        {
            _createTransactionTypeUseCase = createTransactionTypeUseCase;
            _deleteTransactionTypeUseCase = deleteTransactionTypeUseCase;
            _getTransactionTypeUseCase    = getTransactionTypeUseCase;
            _updateTransactionTypeUseCase = updateTransactionTypeUseCase;
        }

        public async Task<(TransactionTypeDTO TransactionTypeDTO, string Message)> CreateAsyncTransactionType(string transactionTypeName, string description)
        {
            return await _createTransactionTypeUseCase.CreateAsyncTransactionType(transactionTypeName, description);
        }
        public (TransactionTypeDTO TransactionTypeDTO, string Message) CreateTransactionType(string transactionTypeName, string description)
        {
            return _createTransactionTypeUseCase.CreateTransactionType(transactionTypeName, description);
        }

        public async Task<List<TransactionTypeDTO>> GetAllAsyncTransactionType()
        {
            return await _getTransactionTypeUseCase.GetAllAsyncTransactionType();
        }
        public List<TransactionTypeDTO> GetAllTransactionType()
        {
            return _getTransactionTypeUseCase.GetAllTransactionType();
        }

        public async Task<TransactionTypeDTO> GetAsyncTransactionType(int idTransactionType)
        {
            return await _getTransactionTypeUseCase.GetAsyncTransactionType(idTransactionType);
        }
        public TransactionTypeDTO GetTransactionType(int idTransactionType)
        {
            return _getTransactionTypeUseCase.GetTransactionType(idTransactionType);
        }

        public async Task<TransactionTypeDTO> GetAsyncTransactionType(string transactionTypeName)
        {
            return await _getTransactionTypeUseCase.GetAsyncTransactionType(transactionTypeName);
        }
        public TransactionTypeDTO GetTransactionType(string transactionTypeName)
        {
            return _getTransactionTypeUseCase.GetTransactionType(transactionTypeName);
        }

        public async Task<int> UpdateAsyncTransactionType(int idTransactionType, string transactionTypeName, string description)
        {
            return await _updateTransactionTypeUseCase.UpdateAsyncTransactionType(idTransactionType, transactionTypeName, description);
        }
        public int UpdateTransactionType(int idTransactionType, string transactionTypeName, string description)
        {
            return _updateTransactionTypeUseCase.UpdateTransactionType(idTransactionType, transactionTypeName, description);
        }

        public async Task DeleteAsyncTransactionType(int idTransactionType)
        {
            await _deleteTransactionTypeUseCase.DeleteAsyncTransactionType(idTransactionType);
        }
        public void DeleteTransactionType(int idTransactionType)
        {
            _deleteTransactionTypeUseCase.DeleteTransactionType(idTransactionType);
        }
    }
}
