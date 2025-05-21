using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.UserUseCases
{
    public class AuthenticateUserUseCase : IAuthenticateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResult> AuthenticateAsync(AuthenticateUserCommand command)
        {
            if (command is null)
                throw new ArgumentNullException($"Значение команды: {nameof(command)}\nБыло пустым");

            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(command.Login))
                validationErrors.Add("Вы не заполнили поле 'Login'!!");
            if (string.IsNullOrWhiteSpace(command.Password))
                validationErrors.Add("Вы не заполнили поле 'Password'!!");

            if (validationErrors.Any())
                return UserResult.ValidationFailureResult(validationErrors, "Ошибка валидации!!");

            if (!await _userRepository.ExistByLoginAsync(command.Login))
                return UserResult.FailureResult(ErrorCode.LoginNotExist,
                    $"Данный 'Login' <{command.Login}> не зарегистрирован!!");

            try
            {
                var userDomain = await _userRepository.GetUserByLoginAsync(command.Login);

                if (userDomain is null)
                    return UserResult.FailureResult(ErrorCode.LoginNotExist, "Пользователь не был найден!!");

                var storageHash = await _userRepository.GetHashByLoginAsync(command.Login);
                var verifiableHash = _passwordHasher.VerifyPassword(command.Password, storageHash);

                if (!verifiableHash)
                    return UserResult.FailureResult(ErrorCode.InvalidPassword, "Указанный пароль не верен!!");

                await _userRepository.UpdateDataEntryAsync(userDomain.IdUser);

                return UserResult.SuccessResult(userDomain.ToDTO());
            }
            catch (Exception)
            {
                return UserResult.FailureResult(ErrorCode.UnknownError, "Во время аутентификации произошла непредвиденная ошибка!!");
            }
        }
    }
}
