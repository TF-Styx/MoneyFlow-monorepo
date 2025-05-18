using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;
using MoneyFlow.AuthenticationService.Domain.DomainModels;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.GenderUseCases
{
    public class UpdateGenderUseCase(IGenderRepository genderRepository) : IUpdateGenderUseCase
    {
        private readonly IGenderRepository _genderRepository = genderRepository;

        public async Task<(GenderDTO? GenderDTO, string Message)> UpdateAsync(GenderDTO genderDTO)
        {
            var (Domain, Message) = GenderDomain.Create
                (
                    genderDTO.IdGender,
                    genderDTO.GenderName
                );

            if (Domain is null)
                return (null, Message);

            var exist = await _genderRepository.ExistAsync(Domain.IdGender);

            if (exist is false)
                return (null, "Пол не найден!!");

            var newDomain = await _genderRepository.UpdateAsync(Domain);

            return (newDomain.ToDTO(), "Данные успешно изменены!!");
        }
    }
}
