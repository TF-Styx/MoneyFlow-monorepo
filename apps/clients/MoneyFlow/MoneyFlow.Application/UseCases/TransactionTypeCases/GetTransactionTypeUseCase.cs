using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.TransactionTypeCases
{
    public class GetTransactionTypeUseCase : IGetTransactionTypeUseCase
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public GetTransactionTypeUseCase(ITransactionTypeRepository transactionTypeRepository)
        {
            _transactionTypeRepository = transactionTypeRepository;
        }

        public async Task<List<TransactionTypeDTO>> GetAllAsyncTransactionType()
        {
            var transactionTypes = await _transactionTypeRepository.GetAllAsync();
            var transactionTypeDTO = transactionTypes.ToListDTO();

            return transactionTypeDTO;
        }
        public List<TransactionTypeDTO> GetAllTransactionType()
        {
            var transactionTypes = _transactionTypeRepository.GetAll();
            var transactionTypeDTO = transactionTypes.ToListDTO();

            return transactionTypeDTO;
        }

        public async Task<TransactionTypeDTO> GetAsyncTransactionType(int idTransactionType)
        {
            var transactionType = await _transactionTypeRepository.GetAsync(idTransactionType);

            if (transactionType == null) { return null; } // TODO : Сделать исключение

            var transactionTypeDTO = transactionType.ToDTO();

            return transactionTypeDTO.TransactionTypeDTO;
        }
        public TransactionTypeDTO GetTransactionType(int idTransactionType)
        {
            var transactionType = _transactionTypeRepository.Get(idTransactionType);

            if (transactionType == null) { return null; } // TODO : Сделать исключение

            var transactionTypeDTO = transactionType.ToDTO();

            return transactionTypeDTO.TransactionTypeDTO;
        }

        public async Task<TransactionTypeDTO> GetAsyncTransactionType(string transactionTypeName)
        {
            var transactionType = await _transactionTypeRepository.GetAsync(transactionTypeName);

            if (transactionType == null) { return null; } // TODO : Сделать исключение

            var transactionTypeDTO = transactionType.ToDTO();

            return transactionTypeDTO.TransactionTypeDTO;
        }
        public TransactionTypeDTO GetTransactionType(string transactionTypeName)
        {
            var transactionType = _transactionTypeRepository.Get(transactionTypeName);

            if (transactionType == null) { return null; } // TODO : Сделать исключение

            var transactionTypeDTO = transactionType.ToDTO();

            return transactionTypeDTO.TransactionTypeDTO;
        }
    }
}
