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
    internal class AuthenticateUserTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHasher> _passwordHasherMock;
        private AuthenticateUserUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _useCase = new AuthenticateUserUseCase(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        private AuthenticateUserCommand CreateSampleRegisterUserCommand(string? login = "Styx", string password = "ValidPass123.!")
        {
            return new AuthenticateUserCommand()
            {
                Login = login,
                Password = password
            };
        }

        [Test]
        public async Task AuthenticateAsync_ValidCommand_ReturnsSuccessResult()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.GetHashByLoginAsync(It.IsAny<string>())).ReturnsAsync("storageHash");
            _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = await _useCase.AuthenticateAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result, Is.Not.Null);
            });
        }

        [Test]
        public async Task AuthenticateAsync_InvalidCommand_ReturnsValidationResult_WhenLoginAndPasswordEmpty()
        {
            var command = CreateSampleRegisterUserCommand(login: string.Empty, password: string.Empty);

            var result = await _useCase.AuthenticateAsync(command);

            if (result.ValidationErrors.Any())
                for (int i = 0; i < result.ValidationErrors.Count; i++)
                    TestContext.Out.WriteLine($"Ошибка валидации: {result.ValidationErrors[i]}");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.User, Is.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ValidationErrors, Is.Not.Null);
                Assert.That(result.ValidationErrors.Any(), Is.True);
            });
        }

        [Test]
        public async Task AuthenticateAsync_InvalidCommand_ReturnsValidationResult_WhenLoginNotExist()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(false);

            var result = await _useCase.AuthenticateAsync(command);
            
            TestContext.Out.WriteLine($"Ошибка: {result.ErrorMessage}");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.User, Is.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo($"Данный 'Login' <{command.Login}> не зарегистрирован!!"));
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.LoginNotExist));
                Assert.That(result.ValidationErrors.Any(), Is.False);
            });
        }

        [Test]
        public async Task AuthenticateAsync_InvalidValidCommand_ReturnsFailureResult_WhenVerifyPassword()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.GetHashByLoginAsync(It.IsAny<string>())).ReturnsAsync("storageHash");
            _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = await _useCase.AuthenticateAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.User, Is.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Указанный пароль не верен!!"));
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.InvalidPassword));
                Assert.That(result.ValidationErrors.Any(), Is.False);
            });
        }

        [Test]
        public async Task AuthenticateAsync_UnknownError_ReturnsFailureResult_ThrowException()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.GetHashByLoginAsync(It.IsAny<string>())).Throws(new Exception("Исключение"));
            _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = await _useCase.AuthenticateAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.User, Is.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Во время аутентификации произошла непредвиденная ошибка!!"));
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnknownError));
                Assert.That(result.ValidationErrors.Any(), Is.False);
            });
        }

        [Test]
        public void AuthenticateAsync_Exception_ThrowArgumentNullException_WhenCommandNull()
        {
            AuthenticateUserCommand command = null!;

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _useCase.AuthenticateAsync(command));

            Assert.That(ex.ParamName, Is.EqualTo($"Значение команды: {nameof(command)}\nБыло пустым"));
        }
    }
}
