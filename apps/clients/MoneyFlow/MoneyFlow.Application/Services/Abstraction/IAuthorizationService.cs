using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IAuthorizationService
    {
        UserDTO CurrentUser { get; }

        Task<(UserDTO UserDTO, string Message)> AuthAsync(string login, string password);
        (UserDTO UserDTO, string Message) Auth(string login, string password);
    }
}