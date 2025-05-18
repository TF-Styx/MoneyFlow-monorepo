using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IBankService
    {
        Task<(BankDTO BankDTO, string Message)> CreateAsync(string bankName);
        (BankDTO BankDTO, string Message) Create(string bankName);

        Task<List<BankDTO>> GetAllAsync();
        List<BankDTO> GetAll();

        Task<BankDTO> GetAsync(int idBank);
        BankDTO Get(int idBank);

        Task<BankDTO> GetAsync(string bankName);
        BankDTO Get(string bankName);

        Task<UserBanksDTO> GetByIdUserAsync(int idUser);
        UserBanksDTO GetByIdUser(int idUser);

        Task<int> UpdateAsync(int idBank, string bankName);
        int Update(int idBank, string bankName);

        Task DeleteAsync(int idBank);
        void Delete(int idBank);
    }
}