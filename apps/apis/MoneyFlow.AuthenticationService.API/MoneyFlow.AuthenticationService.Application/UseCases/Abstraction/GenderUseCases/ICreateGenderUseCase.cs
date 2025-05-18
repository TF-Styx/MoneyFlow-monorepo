using MoneyFlow.AuthenticationService.Application.DTOs;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases
{
    public interface ICreateGenderUseCase
    {
        Task<(GenderDTO? GenderDTO, string Message)> CreateAsync(GenderDTO genderDTO);
    }
}