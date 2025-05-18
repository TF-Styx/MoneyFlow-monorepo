using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.GenderUseCases
{
    public class GetAllStreamingGenderUseCase(IGenderRepository genderRepository) : IGetAllStreamingGenderUseCase
    {
        private readonly IGenderRepository _genderRepository = genderRepository;

        public IAsyncEnumerable<GenderDTO> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return _genderRepository.GetAllStreamingAsync(cancellationToken).Select(domain => domain.ToDTO());
        }
    }
}
