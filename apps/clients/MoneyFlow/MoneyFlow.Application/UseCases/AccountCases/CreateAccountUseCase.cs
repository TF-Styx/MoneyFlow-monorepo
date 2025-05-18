using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.AccountCases
{
    public class CreateAccountUseCase : ICreateAccountUseCase
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<(AccountDTO AccountDTO, string Message)> CreateAsync(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance)
        {
            string message = string.Empty;

            if (numberAccount == 0 &&
                bankDTO == null &&
                accountTypeDTO == null &&
                balance == 0)
            {
                return (null, "Вы не заполнили поля!!");
            }

            //var existAccount = await _accountRepository.GetNumberAsync(numberAccount);

            //if (existAccount != null) { return (null, "Пользователь с таким логином уже есть!!"); }

            var idAccount = await _accountRepository.CreateAsync(numberAccount, idUser, bankDTO.ToDomain().BankDomain, accountTypeDTO.ToDomain().AccountTypeDomain, balance);
            var accountDomain = await _accountRepository.GetAsync(idAccount);

            return (accountDomain.ToDTO().AccountDTO, message);
        }
        public (AccountDTO AccountDTO, string Message) Create(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance)
        {
            string message = string.Empty;

            if (numberAccount == 0 &&
                bankDTO == null &&
                accountTypeDTO == null &&
                balance == 0)
            {
                return (null, "Вы не заполнили поля!!");
            }

            var existAccount = _accountRepository.Get(numberAccount);

            if (existAccount != null) { return (null, "Пользователь с таким логином уже есть!!"); }

            var idAccount = _accountRepository.Create(numberAccount, idUser, bankDTO.ToDomain().BankDomain, accountTypeDTO.ToDomain().AccountTypeDomain, balance);
            var accountDomain = _accountRepository.Get(idAccount);

            return (accountDomain.ToDTO().AccountDTO, message);
        }
    }
}
