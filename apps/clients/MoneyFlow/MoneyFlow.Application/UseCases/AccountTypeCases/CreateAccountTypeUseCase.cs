using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.AccountTypeCases
{
    public class CreateAccountTypeUseCase : ICreateAccountTypeUseCase
    {
        private readonly IAccountTypeRepository _accountTypeRepository;

        public CreateAccountTypeUseCase(IAccountTypeRepository accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }

        public async Task<(AccountTypeDTO AccountTypeDTO, string Message)> CreateAsync(string accountTypeName)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(accountTypeName))
            {
                return (null, "Вы не указали название банка!!");
            }

            var existAccountType = await _accountTypeRepository.GetAsync(accountTypeName);

            if (existAccountType != null)
            {
                return (null, "Банк с таким именем уже есть!!");
            }

            var idAccountType = await _accountTypeRepository.CreateAsync(accountTypeName);
            var accountTypeDomain = await _accountTypeRepository.GetAsync(idAccountType);

            return (accountTypeDomain.ToDTO().AccountTypeDTO, message);
        }
        public (AccountTypeDTO AccountTypeDTO, string Message) Create(string accountTypeName)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(accountTypeName))
            {
                return (null, "Вы не указали название банка!!");
            }

            var existAccountType = _accountTypeRepository.Get(accountTypeName);

            if (existAccountType != null)
            {
                return (null, "Банк с таким именем уже есть!!");
            }

            var idAccountType = _accountTypeRepository.Create(accountTypeName);
            var accountTypeDomain = _accountTypeRepository.Get(idAccountType);

            return (accountTypeDomain.ToDTO().AccountTypeDTO, message);
        }
    }
}
