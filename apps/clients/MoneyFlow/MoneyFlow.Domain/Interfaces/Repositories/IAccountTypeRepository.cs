using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Domain.Interfaces.Repositories
{
    public interface IAccountTypeRepository
    {
        Task<int> CreateAsync(string accountTypeName);
        int Create(string accountTypeName);

        Task<List<AccountTypeDomain>> GetAllAsync();
        List<AccountTypeDomain> GetAll();

        Task<AccountTypeDomain> GetAsync(int idAccountType);
        AccountTypeDomain Get(int idAccountType);

        Task<AccountTypeDomain> GetAsync(string accountTypeName);
        AccountTypeDomain Get(string accountTypeName);

        Task<UserAccountTypesDomain> GetByIdUserAsync(int idUser);
        UserAccountTypesDomain GetByIdUser(int idUser);

        Task<int> UpdateAsync(int idAccountType, string accountTypeName);
        int Update(int idAccountType, string accountTypeName);

        Task DeleteAsync(int idAccountType);
        void Delete(int idAccountType);
    }
}