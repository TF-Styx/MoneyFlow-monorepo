using MoneyFlow.AuthenticationService.Domain.DomainModels;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.InterfaceRepositories
{
    public interface IGenderRepository
    {
        //Task<GenderDomain> CreateAsync(GenderDomain genderDomain);
        IAsyncEnumerable<GenderDomain> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
        //Task<GenderDomain> GetByIdAsync(int idGender);
        //Task<GenderDomain> UpdateAsync(GenderDomain genderDomain);
        //Task<int> DeleteAsync(int idGender);
        //Task<bool> ExistAsync(int idGender);
    }
}