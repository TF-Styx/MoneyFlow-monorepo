using MoneyFlow.AuthenticationService.Application.DTOs;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases
{
    public interface IUpdateGenderUseCase
    {
        Task<(GenderDTO? GenderDTO, string Message)> UpdateAsync(GenderDTO genderDTO);
    }
}