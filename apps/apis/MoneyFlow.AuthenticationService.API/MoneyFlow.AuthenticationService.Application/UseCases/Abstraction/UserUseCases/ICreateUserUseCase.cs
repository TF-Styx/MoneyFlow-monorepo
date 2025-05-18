using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases
{
    public interface ICreateUserUseCase
    {
        Task<RegisterUserResult> CreateAsync(RegisterUserCommand command);
    }
}