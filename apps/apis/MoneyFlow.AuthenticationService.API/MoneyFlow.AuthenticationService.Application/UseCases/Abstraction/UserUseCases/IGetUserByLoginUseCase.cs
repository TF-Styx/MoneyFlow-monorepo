using MoneyFlow.AuthenticationService.Application.DTOs.Results;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases
{
    public interface IGetUserByLoginUseCase
    {
        Task<UserResult> GetUserByLogin(string login);
    }
}