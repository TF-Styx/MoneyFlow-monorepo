using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;

namespace MoneyFlow.AuthenticationService.Application.Providers.Realization
{
    public class DefaultRegistrationErrorMessageProvider : IDefaultErrorMessageProvider
    {
        public string GetMessage(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.LoginAlreadyRegistered => "Данный логин уже занят.",
                ErrorCode.EmailAlreadyRegistered => "Данный email уже зарегистрирован.",
                ErrorCode.PhoneAlreadyRegistered => "Данный номер телефона уже зарегистрирован.",
                ErrorCode.WeakPassword => "Пароль не соответствует требованиям безопасности.",
                ErrorCode.ValidationFailed => "Одно или несколько полей не прошли валидацию.",
                ErrorCode.DomainCreationError => "Ошибка при создании объекта пользователя.",
                ErrorCode.InvalidRole => "Указана неверная роль.",
                ErrorCode.InvalidAccountStatus => "Указан неверный статус аккаунта.",
                ErrorCode.SaveUserError => "Ошибка при сохранении пользователя в базе данных.",
                ErrorCode.UnknownError or _ => "Произошла непредвиденная ошибка."
            };
        }
    }
}
