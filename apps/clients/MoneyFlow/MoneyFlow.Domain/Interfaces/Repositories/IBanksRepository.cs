using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Domain.Interfaces.Repositories
{
    public interface IBanksRepository
    {
        Task<int> CreateAsync(string bankName);
        int Create(string bankName);

        Task<List<BankDomain>> GetAllAsync();
        List<BankDomain> GetAll();

        Task<BankDomain> GetAsync(int idBank);
        BankDomain Get(int idBank);

        Task<BankDomain> GetAsync(string bankName);
        BankDomain Get(string bankName);

        Task<UserBanksDomain> GetByIdUserAsync(int idUser);
        UserBanksDomain GetByIdUser(int idUser);

        Task<int> UpdateAsync(int idBank, string bankName);
        int Update(int idBank, string bankName);

        Task DeleteAsync(int idBank);
        void Delete(int idBank);
    }
}