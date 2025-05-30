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

        public async IAsyncEnumerable<GenderDTO> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var genderList = _genderRepository.GetAllStreamingAsync(cancellationToken);

            await foreach (var gender in genderList.WithCancellation(cancellationToken))
                yield return gender.ToDTO();
        }
    }
}
