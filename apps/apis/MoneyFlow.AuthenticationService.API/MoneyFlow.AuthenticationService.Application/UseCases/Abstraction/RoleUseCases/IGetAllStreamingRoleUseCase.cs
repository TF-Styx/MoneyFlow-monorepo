using MoneyFlow.AuthenticationService.Application.DTOs;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases
{
    public interface IGetAllStreamingRoleUseCase
    {
        IAsyncEnumerable<RoleDTO> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}