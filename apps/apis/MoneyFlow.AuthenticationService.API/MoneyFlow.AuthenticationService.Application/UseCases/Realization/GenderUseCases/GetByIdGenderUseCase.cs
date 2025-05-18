using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.GenderUseCases
{
    public class GetByIdGenderUseCase(IGenderRepository genderRepository) : IGetByIdGenderUseCase
    {
        private readonly IGenderRepository _genderRepository = genderRepository;

        public async Task<(GenderDTO? GenderDTO, string Message)> GetByIdAsync(int idGender)
        {
            var domain = await _genderRepository.GetByIdAsync(idGender);

            if (domain is null)
                return (null, "Пол не найден!!");

            return (domain.ToDTO(), string.Empty);
        }
    }
}
