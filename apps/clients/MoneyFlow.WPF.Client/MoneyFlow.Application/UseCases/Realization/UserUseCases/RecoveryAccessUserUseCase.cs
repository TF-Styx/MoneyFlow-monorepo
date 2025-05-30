using MoneyFlow.Application.InterfaceRepositories;
using MoneyFlow.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.Domain.Enums;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.UseCases.Realization.UserUseCases
{
    public class RecoveryAccessUserUseCase : IRecoveryAccessUserUseCase
    {
        private readonly IActionUserProfileRepository _repository;

        public RecoveryAccessUserUseCase(IActionUserProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<(string Login, string NewPassword)?>> RecoveryAccessAsync(string email, string login, string newPassword)
        {
            var validationErrors = new List<ErrorDetails>();

            if (string.IsNullOrWhiteSpace(email))
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не заполнили поле с почтой!!"));
            if (string.IsNullOrWhiteSpace(login))
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не заполнили поле с логином!!"));
            if (string.IsNullOrWhiteSpace(newPassword))
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не заполнили поле с новым паролем!!"));

            if (validationErrors.Any())
                return Result<ValueTuple<string, string>?>.FailureResult([.. validationErrors]);

            Result<ValueTuple<string, string>?> result = await _repository.RecoveryAccessAsync(email, login, newPassword);

            return result;
        }
    }
}
