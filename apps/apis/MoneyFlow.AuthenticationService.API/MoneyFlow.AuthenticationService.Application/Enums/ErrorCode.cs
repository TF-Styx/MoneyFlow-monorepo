namespace MoneyFlow.AuthenticationService.Application.Enums
{
    public enum ErrorCode
    {
        None,

        // Общие ошибки
        UnknownError,
        ValidationFailed,
        DataBaseFailed,

        DomainCreationError,

        // Специфичные для регистрации
        LoginAlreadyRegistered,
        LoginNotExist,
        EmailAlreadyRegistered,
        PhoneAlreadyRegistered,
        InvalidPassword,
        WeakPassword,
        InvalidRole, // Если роль проверяется
        InvalidAccountStatus, // Если статус проверяется
        SaveUserError,
    }
}
