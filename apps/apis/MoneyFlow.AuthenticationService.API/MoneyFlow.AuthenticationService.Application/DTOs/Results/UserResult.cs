using MoneyFlow.AuthenticationService.Application.Enums;

namespace MoneyFlow.AuthenticationService.Application.DTOs.Results
{
    public class UserResult
    {
        public bool Success { get; private set; }
        public UserDTO? User { get; private set; }
        public ErrorCode? ErrorCode { get; private set; } // Используем enum
        public string? ErrorMessage { get; private set; } // Можно оставить для дефолтного сообщения или деталей
        public List<string> ValidationErrors { get; private set; } = new List<string>();
        //public List<string> DataBaseErrors { get; private set; } = new List<string>();

        // Фабричные методы
        public static UserResult SuccessResult(UserDTO user)
            => new() { Success = true, User = user };

        public static UserResult SuccessResult()
            => new() { Success = true };

        public static UserResult FailureResult(ErrorCode errorCode, string? message = null)
            => new() { Success = false, User = null, ErrorCode = errorCode, ErrorMessage = message };

        public static UserResult ValidationFailureResult(List<string> errors, string? generalMessage = null)
            => new()
            {
                Success = false,
                User = null,
                ErrorCode = Enums.ErrorCode.ValidationFailed,
                ValidationErrors = errors,
                ErrorMessage = generalMessage
            };

        //public static RegisterUserResult DataBaseFailureResult(List<string> errors, string? generalMessage = null)
        //    => new()
        //    {
        //        Success = false,
        //        User = null,
        //        ErrorCode = RegistrationErrorCode.DataBaseFailed,
        //        ValidationErrors = [],
        //        DataBaseErrors = errors,
        //        ErrorMessage = generalMessage ?? GetDefaultMessage(RegistrationErrorCode.ValidationFailed)
        //    };

        // Опционально: метод для получения дефолтных сообщений
        //private static string GetDefaultMessage(ErrorCode errorCode)
        //{
        //    return errorCode switch
        //    {
        //        Enums.ErrorCode.LoginAlreadyRegistered => "This login is already taken.",
        //        Enums.ErrorCode.EmailAlreadyRegistered => "This email is already registered.",
        //        Enums.ErrorCode.WeakPassword => "The password does not meet complexity requirements.",
        //        Enums.ErrorCode.ValidationFailed => "One or more validation errors occurred.",
        //        // ... другие сообщения
        //        _ => "An unexpected error occurred."
        //    };
        //}
    }
}
