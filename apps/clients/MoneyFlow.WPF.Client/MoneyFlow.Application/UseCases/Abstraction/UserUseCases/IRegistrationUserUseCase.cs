using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.UseCases.Abstraction.UserUseCases
{
    public interface IRegistrationUserUseCase
    {
        Task<Result<(string Login, string Password)?>> RegistrationAsync(string login, string password, string userName, string email, string? phone, int idGender);
    }
}