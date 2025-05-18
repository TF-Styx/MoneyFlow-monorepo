using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces;

namespace MoneyFlow.Application.Services.Realization
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly ICreateAccountTypeUseCase _createAccountTypeUseCase;
        private readonly IDeleteAccountTypeUseCase _deleteAccountTypeUseCase;
        private readonly IGetAccountTypeUseCase _getAccountTypeUseCase;
        private readonly IUpdateAccountTypeUseCase _updateAccountTypeUseCase;

        public AccountTypeService(ICreateAccountTypeUseCase createAccountTypeUseCase, IDeleteAccountTypeUseCase deleteAccountTypeUseCase, IGetAccountTypeUseCase getAccountTypeUseCase, IUpdateAccountTypeUseCase updateAccountTypeUseCase)
        {
            _createAccountTypeUseCase = createAccountTypeUseCase;
            _deleteAccountTypeUseCase = deleteAccountTypeUseCase;
            _getAccountTypeUseCase = getAccountTypeUseCase;
            _updateAccountTypeUseCase = updateAccountTypeUseCase;
        }

        public async Task<(AccountTypeDTO AccountTypeDTO, string Message)> CreateAsyncAccountType(string accountTypeName)
        {
            return await _createAccountTypeUseCase.CreateAsync(accountTypeName);
        }
        public (AccountTypeDTO AccountTypeDTO, string Message) CreateAccountType(string accountTypeName)
        {
            return _createAccountTypeUseCase.Create(accountTypeName);
        }

        public async Task<List<AccountTypeDTO>> GetAllAsync()
        {
            return await _getAccountTypeUseCase.GetAllAsync();
        }
        public List<AccountTypeDTO> GetAll()
        {
            return _getAccountTypeUseCase.GetAll();
        }

        public async Task<AccountTypeDTO> GetAsync(int idAccountType)
        {
            return await _getAccountTypeUseCase.GetAsync(idAccountType);
        }
        public AccountTypeDTO Get(int idAccountType)
        {
            return _getAccountTypeUseCase.Get(idAccountType);
        }

        public async Task<AccountTypeDTO> GetAsync(string accountTypeName)
        {
            return await _getAccountTypeUseCase.GetAsync(accountTypeName);
        }
        public AccountTypeDTO Get(string accountTypeName)
        {
            return _getAccountTypeUseCase.Get(accountTypeName);
        }

        public async Task<UserAccountTypesDTO> GetByIdUserAsync(int idUser)
        {
            return await _getAccountTypeUseCase.GetByIdUserAsync(idUser);
        }
        public UserAccountTypesDTO GetByIdUser(int idUser)
        {
            return _getAccountTypeUseCase.GetByIdUser(idUser);
        }

        public async Task<int> UpdateAsync(int idAccountType, string accountTypeName)
        {
            return await _updateAccountTypeUseCase.UpdateAsync(idAccountType, accountTypeName);
        }
        public int Update(int idAccountType, string accountTypeName)
        {
            return _updateAccountTypeUseCase.Update(idAccountType, accountTypeName);
        }

        public async Task DeleteAsync(int idAccountType)
        {
            await _deleteAccountTypeUseCase.DeleteAsync(idAccountType);
        }
        public void Delete(int idAccountType)
        {
            _deleteAccountTypeUseCase.Delete(idAccountType);
        }
    }
}
