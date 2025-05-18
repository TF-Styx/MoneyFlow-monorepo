using MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.AccountTypeCases
{
    public class UpdateAccountTypeUseCase : IUpdateAccountTypeUseCase
    {
        private readonly IAccountTypeRepository _accountTypeRepository;

        public UpdateAccountTypeUseCase(IAccountTypeRepository accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }

        public async Task<int> UpdateAsync(int idAccountType, string accountTypeName)
        {
            if (string.IsNullOrWhiteSpace(accountTypeName))
            {
                throw new Exception("Данного типа счета не существует!!");
            }

            var existAccountType = await _accountTypeRepository.GetAsync(idAccountType);

            if (existAccountType == null)
            {
                throw new Exception("Данного типа счета не существует!!");
            }

            return await _accountTypeRepository.UpdateAsync(idAccountType, accountTypeName);
        }
        public int Update(int idAccountType, string accountTypeName)
        {
            if (string.IsNullOrWhiteSpace(accountTypeName))
            {
                throw new Exception("Данного типа счета не существует!!");
            }

            var existAccountType = _accountTypeRepository.Get(accountTypeName);

            if (existAccountType == null)
            {
                throw new Exception("Данного типа счета не существует!!");
            }

            return _accountTypeRepository.Update(idAccountType, accountTypeName);
        }
    }
}
