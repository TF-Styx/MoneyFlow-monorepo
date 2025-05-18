namespace MoneyFlow.AuthenticationService.Application.Enums
{
    public enum RegistrationErrorCode
    {
        None,

        // Общие ошибки
        UnknownError,
        ValidationFailed,
        DataBaseFailed,

        DomainCreationError,

        // Специфичные для регистрации
        LoginAlreadyTaken,
        EmailAlreadyRegistered,
        PhoneAlreadyRegistered,
        WeakPassword,
        InvalidRole, // Если роль проверяется
        InvalidAccountStatus, // Если статус проверяется
        SaveUserError,
    }
}
