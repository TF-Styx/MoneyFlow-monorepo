using MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.AccountTypeCases
{
    public class DeleteAccountTypeUseCase : IDeleteAccountTypeUseCase
    {
        private readonly IAccountTypeRepository _accountTypeRepository;

        public DeleteAccountTypeUseCase(IAccountTypeRepository accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }

        public async Task DeleteAsync(int idAccountType)
        {
            await _accountTypeRepository.DeleteAsync(idAccountType); // TODO : Сделать проверку на существование элемента
        }
        public void Delete(int idAccountType)
        {
            _accountTypeRepository.Delete(idAccountType); // TODO : Сделать проверку на существование элемента
        }
    }
}
