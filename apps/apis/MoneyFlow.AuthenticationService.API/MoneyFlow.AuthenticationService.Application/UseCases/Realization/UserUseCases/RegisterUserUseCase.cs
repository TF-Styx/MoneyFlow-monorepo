using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.AuthenticationService.Domain.DomainModels;
using MoneyFlow.AuthenticationService.Domain.Enums;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.UserUseCases
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        // TODO : Перенести валидацию в домайн модель
        public async Task<UserResult> RegisterAsync(RegisterUserCommand command)
        {
            // 1. Базовая валидация
            var validationError = new List<string>();

            if (string.IsNullOrWhiteSpace(command.UserName))
                validationError.Add("Вы не заполнили поле 'User Name'");

            // Проверка Email
            var emailValidationResult = EmailAddress.TryCreate(command.Email, out EmailAddress? emailAddress);
            if (!emailValidationResult.IsValid)
                validationError.AddRange(emailValidationResult.ErrorList);

            // Проверка Login
            var loginValidationResult = Login.TryCreate(command.Login, out Login? login);
            if (!loginValidationResult.IsValid)
                validationError.AddRange(loginValidationResult.ErrorList);

            // Проверка пароля
            var passwordValidationResult = Password.TryCreate(command.Password, out Password? password);
            if (!passwordValidationResult.IsValid)
                validationError.AddRange(passwordValidationResult.ErrorList);

            // Проверка номера телефона
            var phoneNumberValidationResult = PhoneNumber.TryCreate(command.Phone, out PhoneNumber? phoneNumber);
            if (!phoneNumberValidationResult.IsValid)
                validationError.AddRange(phoneNumberValidationResult.ErrorList);

            if (validationError.Any())
                return UserResult.ValidationFailureResult(validationError, "Ошибка валидации!!");

            // 2. Проверка на уникальность (например, логина и email)
            if (await _userRepository.ExistByLoginAsync(command.Login))
                return UserResult.FailureResult(ErrorCode.LoginAlreadyRegistered,
                    $"Пользователь с данным 'Login' <{command.Login}> уже существует!!");

            if (await _userRepository.ExistByEmailAsync(command.Email))
                return UserResult.FailureResult(ErrorCode.EmailAlreadyRegistered, 
                    $"Пользователь с данным 'Email' <{command.Email}> уже существует!!");

            if (await _userRepository.ExistByPhoneAsync(command.Phone))
                return UserResult.FailureResult(ErrorCode.PhoneAlreadyRegistered, 
                    $"Пользователь с данным 'Phone' <{command.Phone}> уже существует!!");

            // 3. Хэширование пароля
            var passwordHash = _passwordHasher.HashPassword(command.Password);

            // 4. Определите значение по умолчание, если не передано
            const int defaultIdRole = (int)Roles.User;

            var (Domain, Message) = UserDomain.Create
                (
                    login,
                    command.UserName,
                    passwordHash,
                    emailAddress,
                    phoneNumber,
                    command.IdGender,
                    defaultIdRole
                );

            if (Domain is null)
                return UserResult.FailureResult(ErrorCode.DomainCreationError, Message);

            try
            {
                var createUserDomain = await _userRepository.CreateAsync(Domain);

                if (createUserDomain is null)
                    return UserResult.FailureResult(ErrorCode.SaveUserError, "Ошибка регистрации пользователя!!");

                var userDTO = createUserDomain.ToDTO();

                return UserResult.SuccessResult(userDTO);
            }
            catch (Exception ex)
            {
                // Логирование ошибки ex
                return UserResult.FailureResult(ErrorCode.UnknownError, "Во время регистрации произошла непредвиденная ошибка!!");
            }
        }
    }
}
