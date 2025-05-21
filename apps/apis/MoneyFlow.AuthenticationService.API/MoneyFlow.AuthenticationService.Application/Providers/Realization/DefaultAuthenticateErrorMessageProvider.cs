using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;

namespace MoneyFlow.AuthenticationService.Application.Providers.Realization
{
    public class DefaultAuthenticateErrorMessageProvider : IDefaultErrorMessageProvider
    {
        public string GetMessage(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.LoginNotExist => "Данный логин не существует.",
                ErrorCode.EmailNotExist => "Данный почтовый адрес не существует.",
                ErrorCode.InvalidPassword => "Указанный пароль не верен.",
                ErrorCode.ValidationFailed => "Одно или несколько полей не прошли валидацию.",
                ErrorCode.SaveUserError => "Ошибка при сохранении пользователя в базе данных.",
                ErrorCode.UnknownError or _ => "Произошла непредвиденная ошибка."
            };
        }
    }
}
