using MoneyFlow.AuthenticationService.Application.DTOs.Commands;
using MoneyFlow.AuthenticationService.Application.DTOs.Results;
using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using MoneyFlow.AuthenticationService.Application.UseCases.Realization.UserUseCases;
using Moq;

namespace Application.Test
{
    internal class RecoveryAccessUserUseCaseTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHasher> _passwordHasherMock;
        private RecoveryAccessUserUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _useCase = new RecoveryAccessUserUseCase(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        private static RecoveryAccessUserCommand CreateSampleRegisterUserCommand(string? email = "ValidEmail@yandex.ru", string? login = "Styx", string password = "ValidPass123.!")
        {
            return new RecoveryAccessUserCommand()
            {
                Email = email,
                Login = login,
                NewPassword = password,
            };
        }

        [Test]
        public async Task RecoveryAccessAsync_ValidCommand_ReturnsSuccessResult()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);
            _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _userRepositoryMock.Setup(x => x.UpdatePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var result = await _useCase.RecoveryAccessAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result, Is.Not.Null);
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_ValidCommand_ReturnsFailureResult()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);
            _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _userRepositoryMock.Setup(x => x.UpdatePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = await _useCase.RecoveryAccessAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("Не удалось сохранить новый пароль пользователя!!"));
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.SaveUserError));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_InvalidCommand_ThrowsArgumentNullException()
        {
            RecoveryAccessUserCommand command = null;

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _useCase.RecoveryAccessAsync(command));

            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.ParamName, Is.EqualTo(nameof(command)));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_InvalidCommand_ReturnsValidationFailureResult_WhenLoginAndEmailAndPasswordEmpty()
        {
            var command = CreateSampleRegisterUserCommand(email: " ", login: " ", password: " ");

            var userResult = await _useCase.RecoveryAccessAsync(command);

            foreach (var result in userResult.ValidationErrors)
                TestContext.Out.WriteLine(result);

            Assert.Multiple(() =>
            {
                Assert.That(userResult, Is.Not.Null);
                Assert.That(userResult.Success, Is.False);
                Assert.That(userResult.ValidationErrors.Any(), Is.True);
                Assert.That(userResult.ErrorMessage, Is.EqualTo(null));
                Assert.That(userResult.ErrorCode, Is.EqualTo(ErrorCode.ValidationFailed));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_InvalidCommand_ReturnsValidationFailureResult_WhenPasswordResultValidationReturnNull()
        {
            var command = CreateSampleRegisterUserCommand(password: "");

            var userResult = await _useCase.RecoveryAccessAsync(command);

            foreach (var result in userResult.ValidationErrors)
                TestContext.Out.WriteLine(result);

            Assert.Multiple(() =>
            {
                Assert.That(userResult, Is.Not.Null);
                Assert.That(userResult.Success, Is.False);
                Assert.That(userResult.ValidationErrors.Any(), Is.True);
                Assert.That(userResult.ErrorMessage, Is.EqualTo(null));
                Assert.That(userResult.ErrorCode, Is.EqualTo(ErrorCode.ValidationFailed));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_InvalidCommand_ReturnsValidationFailureResult_WhenLoginNotExist()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(false);

            var userResult = await _useCase.RecoveryAccessAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(userResult, Is.Not.Null);
                Assert.That(userResult.Success, Is.False);
                Assert.That(userResult.ErrorCode, Is.EqualTo(ErrorCode.LoginNotExist));
            });
        }

        [Test]
        public async Task RecoveryAccessAsync_InvalidCommand_ReturnsValidationFailureResult_WhenEmailNotExist()
        {
            var command = CreateSampleRegisterUserCommand();

            _userRepositoryMock.Setup(x => x.ExistByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.ExistByLoginAsync(It.IsAny<string>())).ReturnsAsync(true);

            var userResult = await _useCase.RecoveryAccessAsync(command);

            Assert.Multiple(() =>
            {
                Assert.That(userResult, Is.Not.Null);
                Assert.That(userResult.Success, Is.False);
                Assert.That(userResult.ErrorCode, Is.EqualTo(ErrorCode.EmailNotExist));
            });
        }
    }
}
