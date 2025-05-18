using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;

namespace MoneyFlow.AuthenticationService.Application.Providers.Realization
{
    public class DefaultRegistrationErrorMessageProvider : IDefaultRegistrationErrorMessageProvider
    {
        public string GetMessage(RegistrationErrorCode errorCode)
        {
            return errorCode switch
            {
                RegistrationErrorCode.LoginAlreadyTaken => "Этот логин уже занят.",
                RegistrationErrorCode.EmailAlreadyRegistered => "Этот email уже зарегистрирован.",
                RegistrationErrorCode.WeakPassword => "Пароль не соответствует требованиям безопасности.",
                RegistrationErrorCode.ValidationFailed => "Одно или несколько полей не прошли валидацию.",
                RegistrationErrorCode.DomainCreationError => "Ошибка при создании доменного объекта пользователя.",
                RegistrationErrorCode.InvalidRole => "Указана неверная роль.",
                RegistrationErrorCode.InvalidAccountStatus => "Указан неверный статус аккаунта.",
                RegistrationErrorCode.SaveUserError => "Ошибка при сохранении пользователя в базе данных.",
                RegistrationErrorCode.UnknownError or _ => "Произошла непредвиденная ошибка."
            };
        }
    }
}
