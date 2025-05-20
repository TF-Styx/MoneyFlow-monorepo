using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using MoneyFlow.AuthenticationService.Application.UseCases.Realization.UserUseCases;
using MoneyFlow.AuthenticationService.Domain.DomainModels;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;
using Moq;

namespace Application.Test
{
    [TestFixture]
    internal class RegisterUserUseCaseTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHasher> _passwordHasherMock;
        private RegisterUserUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _useCase = new RegisterUserUseCase(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        private RegisterUserCommand CreateSampleRegisterUserCommand(string? login = "Styx", string? userName = "Семён", string password = "ValidPass123.!", string? email = "ValidEmail@yandex.ru", string? phone = "+7004001010")
        {
            return new RegisterUserCommand()
            {
                Login = login,
                UserName = userName,
                Password = password,
                Email = email,
                Phone = phone,
                IdGender = 1,
            };
        }

        [Test]
        public async Task RegisterAsync_ValidCommand_ReturnsSuccessResult()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.ExistByPhoneAsync(It.IsAny<string>())).ReturnsAsync(false);
            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashedPassword");

            Login.TryCreate(command.Login, out var login);
            EmailAddress.TryCreate(command.Email, out var email);
            PhoneNumber.TryCreate(command.Phone, out var phone);

            var (Domain, Message) = UserDomain.Create(login, command.UserName, "hashedPassword", email, phone, 1, 1);

            _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<UserDomain>())).ReturnsAsync(Domain);

            var result = await _useCase.RegisterAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True, "Регистрация прошла успешно!!");
                Assert.That(result.User, Is.Not.Null, "Пользователь не должен быть пустым!!");
                Assert.That(result.ErrorCode, Is.Null, "Код ошибок должен быть пустым");
                Assert.That(result.ErrorMessage, Is.Null, "Сообщение ошибки должно быть пустым");
                Assert.That(result.ValidationErrors, Is.Empty.Or.Null, "Список валдиционных ошибки должны быть пустыми или иметь пустую ссылку");
            });
        }

        [Test]
        public async Task RegisterAsync_ValidCommand_ReturnValidationFailure_WhenIncorrectValuesInCommand()
        {
            var command = CreateSampleRegisterUserCommand(login: "Не авлидный логин", password: "qqqq", email: "Novalid!yandex.com");

            var result = await _useCase.RegisterAsync(command);

            if (result.ValidationErrors.Any())
                for (int i = 0; i < result.ValidationErrors.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from CreateUserUseCase] - Ошибка: {result.ValidationErrors[i]}");

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False, "При регистрации произошла ошибка, 'такой логин уже занят'!!");
                Assert.That(result.User, Is.Null, "Пользователь должен быть пустым!!");
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValidationFailed), "Код ошибок не должен быть пустым");
                Assert.That(result.ErrorMessage, Is.Not.Null, "Сообщение ошибки не должно быть пустым");
                Assert.That(result.ValidationErrors, Is.Not.Null, "Список валдиционных ошибки не должны быть пустыми или иметь пустую ссылку");
            });
        }

        [Test]
        public async Task RegisterAsync_ValidCommand_ReturnFailureResult_WhenLoginExist()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(true);

            var result = await _useCase.RegisterAsync(command);

            if (!result.Success)
                TestContext.Out.WriteLine(result.ErrorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False, "Свойство 'Success' должно быть FALSE");
                Assert.That(result.User, Is.Null, "Пользователь должен быть NULL");
                Assert.That(result.ErrorCode, Is.Not.Null, "Ошибка не должна быть Null");
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.LoginAlreadyRegistered), "Код ошибки должен быть RegistrationErrorCode.LoginAlreadyTaken");
            });
        }

        [Test]
        public async Task RegisterAsync_ValidCommand_ReturnFailureResult_WhenEmailExist()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

            var result = await _useCase.RegisterAsync(command);

            if (!result.Success)
                TestContext.Out.WriteLine(result.ErrorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False, "Свойство 'Success' должно быть FALSE");
                Assert.That(result.User, Is.Null, "Пользователь должен быть NULL");
                Assert.That(result.ErrorCode, Is.Not.Null, "Ошибка не должна быть Null");
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.EmailAlreadyRegistered), "Код ошибки должен быть RegistrationErrorCode.EmailAlreadyRegistered");
            });
        }

        [Test]
        public async Task RegisterAsync_ValidCommand_ReturnFailureResult_WhenPhoneExist()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByPhoneAsync(It.IsAny<string>())).ReturnsAsync(true);

            var result = await _useCase.RegisterAsync(command);

            if (!result.Success)
                TestContext.Out.WriteLine(result.ErrorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False, "Свойство 'Success' должно быть FALSE");
                Assert.That(result.User, Is.Null, "Пользователь должен быть NULL");
                Assert.That(result.ErrorCode, Is.Not.Null, "Ошибка не должна быть Null");
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.PhoneAlreadyRegistered), "Код ошибки должен быть RegistrationErrorCode.PhoneAlreadyRegistered");
            });
        }
    }
}