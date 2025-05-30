using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IAuthorizationService
    {
        UserDTO? CurrentUser { get; set; }

        Task<Result<UserDTO>> AuthenticateAsync(string login, string password);
        Task<Result<(string Login, string Password)?>> RegistrationAsync(string userName, string email, string login, string password, int idGender, string? phone);
        Task<Result<(string Login, string NewPassword)?>> RecoveryAccessAsync(string email, string login, string newPassword);
    }
}