using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.Services.Realization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthenticateUserUseCase _authenticateUserUseCase;
        private readonly IRegistrationUserUseCase _registrationUserUseCase;
        private readonly IRecoveryAccessUserUseCase _recoveryAccessUserUseCase;

        public AuthorizationService(IAuthenticateUserUseCase authenticateUserUseCase, IRegistrationUserUseCase registrationUserUseCase, IRecoveryAccessUserUseCase recoveryAccessUserUseCase)
        {
            _authenticateUserUseCase = authenticateUserUseCase;
            _registrationUserUseCase = registrationUserUseCase;
            _recoveryAccessUserUseCase = recoveryAccessUserUseCase;
        }

        public UserDTO? CurrentUser { get; set; }

        public async Task<Result<UserDTO>> AuthenticateAsync(string login, string password)
        {
            var result = await _authenticateUserUseCase.AuthenticateAsync(login, password);

            if (result.Success)
                CurrentUser = result.Value;

            return result;
        }

        public async Task<Result<(string Login, string Password)?>> RegistrationAsync(string userName, string email, string login, string password, int idGender, string? phone) => await _registrationUserUseCase.RegistrationAsync(login, password, userName, email, phone, idGender);

        public async Task<Result<(string Login, string NewPassword)?>> RecoveryAccessAsync(string email, string login, string newPassword) => await _recoveryAccessUserUseCase.RecoveryAccessAsync(email, login, newPassword);
    }
}
