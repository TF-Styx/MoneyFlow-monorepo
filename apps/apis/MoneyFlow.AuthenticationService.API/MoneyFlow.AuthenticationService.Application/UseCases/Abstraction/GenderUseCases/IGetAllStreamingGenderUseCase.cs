using MoneyFlow.AuthenticationService.Application.DTOs;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases
{
    public interface IGetAllStreamingGenderUseCase
    {
        IAsyncEnumerable<GenderDTO> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}