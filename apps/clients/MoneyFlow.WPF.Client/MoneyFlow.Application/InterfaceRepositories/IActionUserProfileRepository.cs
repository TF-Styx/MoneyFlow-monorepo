using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.InterfaceRepositories
{
    public interface IActionUserProfileRepository
    {
        Task<Result<UserDomain>> AuthenticateAsync(string login, string password);
        Task<Result<(string Login, string Password)?>> RegistrationAsync(string userName, string email, string login, string password, int idGender, string? phone);
        Task<Result<(string Login, string NewPassword)?>> RecoveryAccessAsync(string email, string login, string newPassword);
    }
}