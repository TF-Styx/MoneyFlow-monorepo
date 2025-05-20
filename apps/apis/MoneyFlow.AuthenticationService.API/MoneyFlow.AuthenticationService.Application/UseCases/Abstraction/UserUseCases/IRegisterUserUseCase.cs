using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases
{
    public interface IRegisterUserUseCase
    {
        Task<UserResult> RegisterAsync(RegisterUserCommand command);
    }
}