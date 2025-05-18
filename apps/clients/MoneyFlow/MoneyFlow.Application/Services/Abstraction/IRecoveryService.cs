using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IRecoveryService
    {
        Task<(UserDTO UserDTO, string Message)> RecoveryAsync(string login, string password);
        (UserDTO UserDTO, string Message) Recovery(string login, string password);
    }
}