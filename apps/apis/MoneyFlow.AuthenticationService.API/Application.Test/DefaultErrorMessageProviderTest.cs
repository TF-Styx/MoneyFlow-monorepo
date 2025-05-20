using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Interfaces.Realization;
using MoneyFlow.AuthenticationService.Application.Providers.Realization;

namespace Application.Test
{
    internal class DefaultErrorMessageProviderTest
    {
        private DefaultRegistrationErrorMessageProvider _registrationErrorMessage;
        private DefaultAuthenticateErrorMessageProvider _authenticateErrorMessage;

        [SetUp]
        public void SetUp()
        {
            _registrationErrorMessage = new DefaultRegistrationErrorMessageProvider();
            _authenticateErrorMessage = new DefaultAuthenticateErrorMessageProvider();
        }

        [Test]
        public void RegistrationErrorMessageProvider_GetMessage_ReturnStringMessage()
        {
            var loginAlreadyRegistered = _registrationErrorMessage.GetMessage(ErrorCode.LoginAlreadyRegistered);
            var emailAlreadyRegistered = _registrationErrorMessage.GetMessage(ErrorCode.EmailAlreadyRegistered);
            var phoneAlreadyRegistered = _registrationErrorMessage.GetMessage(ErrorCode.PhoneAlreadyRegistered);
            var weakPassword = _registrationErrorMessage.GetMessage(ErrorCode.WeakPassword);
            var validationFailed = _registrationErrorMessage.GetMessage(ErrorCode.ValidationFailed);
            var domainCreationError = _registrationErrorMessage.GetMessage(ErrorCode.DomainCreationError);
            var invalidRole = _registrationErrorMessage.GetMessage(ErrorCode.InvalidRole);
            var saveUserError = _registrationErrorMessage.GetMessage(ErrorCode.SaveUserError);
            var unknownError = _registrationErrorMessage.GetMessage(ErrorCode.UnknownError);

            Assert.Multiple(() =>
            {
                Assert.That(loginAlreadyRegistered, Is.EqualTo("Данный логин уже занят."));
                Assert.That(emailAlreadyRegistered, Is.EqualTo("Данный email уже зарегистрирован."));
                Assert.That(phoneAlreadyRegistered, Is.EqualTo("Данный номер телефона уже зарегистрирован."));
                Assert.That(weakPassword, Is.EqualTo("Пароль не соответствует требованиям безопасности."));
                Assert.That(validationFailed, Is.EqualTo("Одно или несколько полей не прошли валидацию."));
                Assert.That(domainCreationError, Is.EqualTo("Ошибка при создании объекта пользователя."));
                Assert.That(invalidRole, Is.EqualTo("Указана неверная роль."));
                Assert.That(saveUserError, Is.EqualTo("Ошибка при сохранении пользователя в базе данных."));
                Assert.That(unknownError, Is.EqualTo("Произошла непредвиденная ошибка."));
            });
        }

        [Test]
        public void AuthenticateErrorMessageProvider_GetMessage_ReturnStringMessage()
        {
            var loginNotExist = _authenticateErrorMessage.GetMessage(ErrorCode.LoginNotExist);
            var invalidPassword = _authenticateErrorMessage.GetMessage(ErrorCode.InvalidPassword);
            var validationFailed = _authenticateErrorMessage.GetMessage(ErrorCode.ValidationFailed);
            var saveUserError = _authenticateErrorMessage.GetMessage(ErrorCode.SaveUserError);
            var unknownError = _authenticateErrorMessage.GetMessage(ErrorCode.UnknownError);

            Assert.Multiple(() =>
            {
                Assert.That(loginNotExist, Is.EqualTo("Данный логин не существует."));
                Assert.That(invalidPassword, Is.EqualTo("Указанный пароль не верен."));
                Assert.That(validationFailed, Is.EqualTo("Одно или несколько полей не прошли валидацию."));
                Assert.That(saveUserError, Is.EqualTo("Ошибка при сохранении пользователя в базе данных."));
                Assert.That(unknownError, Is.EqualTo("Произошла непредвиденная ошибка."));
            });
        }
    }
}
