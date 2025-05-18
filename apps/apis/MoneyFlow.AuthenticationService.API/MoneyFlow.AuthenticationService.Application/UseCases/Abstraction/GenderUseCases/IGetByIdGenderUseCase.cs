using MoneyFlow.AuthenticationService.Application.DTOs;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases
{
    public interface IGetByIdGenderUseCase
    {
        Task<(GenderDTO GenderDTO, string Message)> GetByIdAsync(int idGender);
    }
}