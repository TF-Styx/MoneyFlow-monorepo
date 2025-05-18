using MoneyFlow.AuthenticationService.Domain.DomainModels;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.InterfaceRepositories
{
    public interface IUserRepository
    {
        Task<UserDomain> CreateAsync(UserDomain userDomain);
        //IAsyncEnumerable<UserDomain> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
        //Task<UserDomain?> GetByIdAsync(int idUser);
        //Task<UserDomain> UpdateAsync(UserDomain userDomain);
        //Task<int> DeleteAsync(int idUser);

        Task<bool> ExistByIdUserAsync(int idUser);
        Task<bool> ExistByLoginAsync(string login);
        Task<bool> ExistByEmailAsync(string email);
        Task<bool> ExistByPhoneAsync(string? phone);
    }
}