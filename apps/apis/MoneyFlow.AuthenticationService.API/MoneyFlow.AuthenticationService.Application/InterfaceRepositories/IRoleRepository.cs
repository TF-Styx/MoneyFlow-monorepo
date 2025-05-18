using MoneyFlow.AuthenticationService.Domain.DomainModels;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.InterfaceRepositories
{
    public interface IRoleRepository
    {
        Task<RoleDomain> CreateAsync(RoleDomain roleDomain);
        IAsyncEnumerable<RoleDomain> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
        Task<RoleDomain?> GetByIdAsync(int idRole);
        Task<RoleDomain?> UpdateAsync(RoleDomain roleDomain);
        Task<int> DeleteAsync(int idRole);
        Task<bool> ExistAsync(int idRole);
    }
}