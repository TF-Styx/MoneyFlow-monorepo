using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<int> CreateAsync(string userName, string login, string password);
        int Create(string userName, string login, string password);

        Task CreateDefaultRecordAsync(int idUser);
        void CreateDefaultRecord(int idUser);

        Task<List<UserDomain>> GetAllAsync();
        List<UserDomain> GetAll();

        Task<UserDomain> GetAsync(int idUser);
        UserDomain Get(int idUser);

        Task<UserDomain> GetAsync(string login);
        UserDomain Get(string login);

        UserTotalInfoDomain GetTotalUserInfo(int idUser);

        Task<int> UpdateAsync(int idUser, string? userName, byte[]? avatar, string password, int? idGender);
        int Update(int idUser, string? userName, byte[]? avatar, string password, int? idGender);

        Task DeleteAsync(int idUser);
        void Delete(int idUser);
    }
}