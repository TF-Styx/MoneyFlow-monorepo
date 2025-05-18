using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IRegistrationService
    {
        Task<(UserDTO UserDTO, string Message)> RegistrationAsync(string userName, string login, string password);
        (UserDTO UserDTO, string Message) Registration(string userName, string login, string password);
    }
}