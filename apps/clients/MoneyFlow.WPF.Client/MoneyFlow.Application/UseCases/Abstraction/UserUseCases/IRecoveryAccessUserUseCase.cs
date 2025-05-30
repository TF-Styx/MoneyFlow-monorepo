using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.UseCases.Abstraction.UserUseCases
{
    public interface IRecoveryAccessUserUseCase
    {
        Task<Result<(string Login, string NewPassword)?>> RecoveryAccessAsync(string email, string login, string newPassword);
    }
}