using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IAccountService
    {
        Task<(AccountDTO AccountDTO, string Message)> CreateAsync(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);
        (AccountDTO AccountDTO, string Message) Create(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);

        Task<List<AccountDTO>> GetAllAsync(int idUser);
        List<AccountDTO> GetAll(int idUser);

        Task<AccountDTO> GetAsync(int idAccount);
        AccountDTO GetAccount(int idAccount);
        
        Task<AccountDTO> GetAsync(int? numberAccount);
        AccountDTO Get(int? numberAccount);

        Task<int> UpdateAsync(int idAccount, int? numberAccount, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);
        int Update(int idAccount, int? numberAccount, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);

        Task DeleteAsync(int idAccount);
        void Delete(int idAccount);
    }
}