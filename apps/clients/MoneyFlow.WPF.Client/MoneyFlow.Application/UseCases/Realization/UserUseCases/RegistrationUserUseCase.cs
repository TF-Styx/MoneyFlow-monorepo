using MoneyFlow.Application.InterfaceRepositories;
using MoneyFlow.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.Domain.Enums;
using MoneyFlow.Domain.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoneyFlow.Application.UseCases.Realization.UserUseCases
{
    public class RegistrationUserUseCase : IRegistrationUserUseCase
    {
        private readonly IActionUserProfileRepository _repository;

        public RegistrationUserUseCase(IActionUserProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<(string Login, string Password)?>> RegistrationAsync(string login, string password, string userName, string email, string? phone, int idGender)
        {
            var validationErrors = new List<ErrorDetails>();

            if (string.IsNullOrEmpty(login))
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не заполнили поле с логином!!"));
            if (string.IsNullOrEmpty(password))
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не заполнили поле с паролем!!"));
            if (string.IsNullOrEmpty(userName))
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не заполнили поле с именем пользователя!!"));
            if (string.IsNullOrEmpty(email))
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не заполнили поле с электронной почтой!!"));
            if (idGender <= 0)
                validationErrors.Add(new ErrorDetails(ErrorCode.ValueEmpty, "Вы не указали гендер."));

            if (validationErrors.Any())
                return Result<ValueTuple<string, string>?>.FailureResult([.. validationErrors]);

            Result<ValueTuple<string, string>?> result = await _repository.RegistrationAsync(userName, email, login, password, idGender, phone);

            return result;
        }
    }
}
