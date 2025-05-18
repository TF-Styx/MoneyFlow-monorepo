using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Domain.Interfaces.Repositories
{
    public interface IGendersRepository
    {
        Task<int> CreateAsync(string genderName);
        int Create(string genderName);

        Task<List<GenderDomain>> GetAllAsync();
        List<GenderDomain> GetAll();

        Task<GenderDomain> GetAsync(int idGender);
        GenderDomain Get(int idGender);

        Task<GenderDomain?> GetAsync(string genderName);
        GenderDomain? Get(string genderName);

        Task<int> UpdateAsync(int idGender, string genderName);
        int Update(int idGender, string genderName);

        Task DeleteAsync(int idGender);
        void Delete(int idGender);
    }
}