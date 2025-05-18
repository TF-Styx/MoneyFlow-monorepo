using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;
using MoneyFlow.AuthenticationService.Domain.DomainModels;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.GenderUseCases
{
    public class CreateGenderUseCase(IGenderRepository genderRepository) : ICreateGenderUseCase
    {
        private readonly IGenderRepository _genderRepository = genderRepository;

        public async Task<(GenderDTO? GenderDTO, string Message)> CreateAsync(GenderDTO genderDTO)
        {
            var (Domain, Message) = GenderDomain.Create
                (
                    genderDTO.IdGender,
                    genderDTO.GenderName
                );

            if (Domain is null)
                return (null, Message);

            var domain = await _genderRepository.CreateAsync(Domain);

            return (Domain.ToDTO(), Message);
        }
    }
}
