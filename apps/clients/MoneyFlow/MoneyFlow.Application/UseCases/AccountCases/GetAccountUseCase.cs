using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.AccountCases
{
    public class GetAccountUseCase : IGetAccountUseCase
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<AccountDTO>> GetAllAsync(int idUser)
        {
            var accounts = await _accountRepository.GetAllAsync(idUser);
            var accountsDTO = accounts.ToListDTO();

            return accountsDTO;
        }
        public List<AccountDTO> GetAll(int idUser)
        {
            var accounts = _accountRepository.GetAll(idUser);
            var accountsDTO = accounts.ToListDTO();

            return accountsDTO;
        }

        public async Task<AccountDTO> GetAsync(int idAccount)
        {
            var account = await _accountRepository.GetAsync(idAccount);

            if (account == null) { return null; } // TODO : Сделать исключение

            var accountDTO = account.ToDTO();

            return accountDTO.AccountDTO;
        }
        public AccountDTO Get(int idAccount)
        {
            var account = _accountRepository.Get(idAccount);

            if (account == null) { return null; } // TODO : Сделать исключение

            var accountDTO = account.ToDTO();

            return accountDTO.AccountDTO;
        }

        public async Task<AccountDTO> GetAsync(int? numberAccount)
        {
            var account = await _accountRepository.GetAsync(numberAccount);

            if (account == null) { return null; } // TODO : Сделать исключение

            var accountDTO = account.ToDTO();

            return accountDTO.AccountDTO;
        }
        public AccountDTO Get(int? numberAccount)
        {
            var account = _accountRepository.Get(numberAccount);

            if (account == null) { return null; } // TODO : Сделать исключение

            var accountDTO = account.ToDTO();

            return accountDTO.AccountDTO;
        }
    }
}
