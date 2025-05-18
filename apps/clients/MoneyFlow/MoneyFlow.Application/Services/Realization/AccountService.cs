using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface;

namespace MoneyFlow.Application.Services.Realization
{
    public class AccountService : IAccountService
    {
        private readonly ICreateAccountUseCase _createAccountUseCase;
        private readonly IDeleteAccountUseCase _deleteAccountUseCase;
        private readonly IGetAccountUseCase _getAccountUseCase;
        private readonly IUpdateAccountUseCase _updateAccountUseCase;

        public AccountService(ICreateAccountUseCase createAccountUseCase, IDeleteAccountUseCase deleteAccountUseCase, IGetAccountUseCase getAccountUseCase, IUpdateAccountUseCase updateAccountUseCase)
        {
            _createAccountUseCase = createAccountUseCase;
            _deleteAccountUseCase = deleteAccountUseCase;
            _getAccountUseCase = getAccountUseCase;
            _updateAccountUseCase = updateAccountUseCase;
        }

        public async Task<(AccountDTO AccountDTO, string Message)> CreateAsync(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance)
        {
            return await _createAccountUseCase.CreateAsync(numberAccount, idUser, bankDTO, accountTypeDTO, balance);
        }
        public (AccountDTO AccountDTO, string Message) Create(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance)
        {
            return _createAccountUseCase.Create(numberAccount, idUser, bankDTO, accountTypeDTO, balance);
        }

        public async Task<List<AccountDTO>> GetAllAsync(int idUser)
        {
            return await _getAccountUseCase.GetAllAsync(idUser);
        }
        public List<AccountDTO> GetAll(int idUser)
        {
            return _getAccountUseCase.GetAll(idUser);
        }

        public async Task<AccountDTO> GetAsync(int idAccount)
        {
            return await _getAccountUseCase.GetAsync(idAccount);
        }
        public AccountDTO GetAccount(int idAccount)
        {
            return _getAccountUseCase.Get(idAccount);
        }

        public async Task<AccountDTO> GetAsync(int? numberAccount)
        {
            return await _getAccountUseCase.GetAsync(numberAccount);
        }
        public AccountDTO Get(int? numberAccount)
        {
            return _getAccountUseCase.Get(numberAccount);
        }

        public async Task<int> UpdateAsync(int idAccount, int? numberAccount, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance)
        {
            return await _updateAccountUseCase.UpdateAsync(idAccount, numberAccount, bankDTO, accountTypeDTO, balance);
        }
        public int Update(int idAccount, int? numberAccount, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance)
        {
            return _updateAccountUseCase.Update(idAccount, numberAccount, bankDTO, accountTypeDTO, balance);
        }

        public async Task DeleteAsync(int idAccount)
        {
            await _deleteAccountUseCase.DeleteAsync(idAccount);
        }
        public void Delete(int idAccount)
        {
            _deleteAccountUseCase.Delete(idAccount);
        }
    }
}
