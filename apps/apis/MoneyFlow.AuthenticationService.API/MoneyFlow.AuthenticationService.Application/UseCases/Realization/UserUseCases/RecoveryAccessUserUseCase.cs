using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.AuthenticationService.Domain.DomainModels.Validations;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.UserUseCases
{
    public class RecoveryAccessUserUseCase : IRecoveryAccessUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RecoveryAccessUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResult> RecoveryAccessAsync(RecoveryAccessUserCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            #region Валидация

            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(command.Email))
                validationErrors.Add("Вы не заполнили поле 'Email'!!");
            if (string.IsNullOrWhiteSpace(command.Login))
                validationErrors.Add("Вы не заполнили поле 'Login'!!");
            if (string.IsNullOrWhiteSpace(command.NewPassword))
                validationErrors.Add("Вы не заполнили поле 'NewPassword'!!");

            ResultValidation passwordResult = Password.TryCreate(command.NewPassword, out Password? passwordVO);

            if (!passwordResult.IsValid)
                validationErrors.AddRange(passwordResult.ErrorList);

            if (validationErrors.Any())
                return UserResult.ValidationFailureResult(validationErrors);

            #endregion

            #region Проверка БД

            if (!await _userRepository.ExistByEmailAsync(command.Email))
                return UserResult.FailureResult(ErrorCode.EmailNotExist, "Данного почтового адреса не существует!!");
            if (!await _userRepository.ExistByLoginAsync(command.Login))
                return UserResult.FailureResult(ErrorCode.LoginNotExist, "Данного логина не существует!!");

            #endregion

            var newHash = _passwordHasher.HashPassword(command.NewPassword);

            var updateResult = await _userRepository.UpdatePasswordAsync(command.Email, command.Login, newHash);

            if (updateResult)
                return UserResult.SuccessResult();
            else
                return UserResult.FailureResult(ErrorCode.SaveUserError, "Не удалось сохранить новый пароль пользователя!!");
        }
    }
}
