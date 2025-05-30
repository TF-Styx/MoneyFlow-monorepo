using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.InterfaceRepositories;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.Domain.Enums;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.UseCases.Realization.UserUseCases
{
    public class AuthenticateUserUseCase : IAuthenticateUserUseCase
    {
        private readonly IActionUserProfileRepository _repository;

        public AuthenticateUserUseCase(IActionUserProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<UserDTO>> AuthenticateAsync(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                return Result<UserDTO>.FailureResult(new ErrorDetails(ErrorCode.Empty, "Вы не заполнили поле с логином!!"));
            if (string.IsNullOrEmpty(password))
                return Result<UserDTO>.FailureResult(new ErrorDetails(ErrorCode.Empty, "Вы не заполнили поле с паролем!!"));

            var result = await _repository.AuthenticateAsync(login, password);

            if (!result.Success)
                return Result<UserDTO>.FailureResult([.. result.ErrorDetails]);

            return Result<UserDTO>.SuccessResult(result.Value!.ToDTO());
        }
    }
}
