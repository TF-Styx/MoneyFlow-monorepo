using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IAccountTypeService
    {
        Task<(AccountTypeDTO AccountTypeDTO, string Message)> CreateAsyncAccountType(string accountTypeName);
        (AccountTypeDTO AccountTypeDTO, string Message) CreateAccountType(string accountTypeName);

        Task<List<AccountTypeDTO>> GetAllAsync();
        List<AccountTypeDTO> GetAll();

        Task<AccountTypeDTO> GetAsync(int idAccountType);
        AccountTypeDTO Get(int idAccountType);

        Task<AccountTypeDTO> GetAsync(string accountTypeName);
        AccountTypeDTO Get(string accountTypeName);

        Task<UserAccountTypesDTO> GetByIdUserAsync(int idUser);
        UserAccountTypesDTO GetByIdUser(int idUser);

        Task<int> UpdateAsync(int idAccountType, string accountTypeName);
        int Update(int idAccountType, string accountTypeName);

        Task DeleteAsync(int idAccountType);
        void Delete(int idAccountType);
    }
}