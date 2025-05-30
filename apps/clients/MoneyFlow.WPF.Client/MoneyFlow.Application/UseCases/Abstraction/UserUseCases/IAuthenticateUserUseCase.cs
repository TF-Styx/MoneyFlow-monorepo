using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.UseCases.Abstraction.UserUseCases
{
    public interface IAuthenticateUserUseCase
    {
        Task<Result<UserDTO>> AuthenticateAsync(string login, string password);
    }
}