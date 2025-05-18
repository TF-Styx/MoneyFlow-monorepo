using MoneyFlow.AuthenticationService.Application.Enums;

namespace MoneyFlow.AuthenticationService.Application.DTOs.Results
{
    public class RegisterUserResult
    {
        public bool Success { get; private set; }
        public UserDTO? User { get; private set; }
        public RegistrationErrorCode? ErrorCode { get; private set; } // Используем enum
        public string? ErrorMessage { get; private set; } // Можно оставить для дефолтного сообщения или деталей
        public List<string> ValidationErrors { get; private set; } = new List<string>();
        //public List<string> DataBaseErrors { get; private set; } = new List<string>();

        // Фабричные методы
        public static RegisterUserResult SuccessResult(UserDTO user)
            => new() { Success = true, User = user };

        public static RegisterUserResult FailureResult(RegistrationErrorCode errorCode, string? message = null)
            => new() { Success = false, User = null, ErrorCode = errorCode, ErrorMessage = message ?? GetDefaultMessage(errorCode) };

        public static RegisterUserResult ValidationFailureResult(List<string> errors, string? generalMessage = null)
            => new()
            {
                Success = false,
                User = null,
                ErrorCode = RegistrationErrorCode.ValidationFailed,
                ValidationErrors = errors,
                ErrorMessage = generalMessage ?? GetDefaultMessage(RegistrationErrorCode.ValidationFailed)
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
        private static string GetDefaultMessage(RegistrationErrorCode errorCode)
        {
            return errorCode switch
            {
                RegistrationErrorCode.LoginAlreadyTaken => "This login is already taken.",
                RegistrationErrorCode.EmailAlreadyRegistered => "This email is already registered.",
                RegistrationErrorCode.WeakPassword => "The password does not meet complexity requirements.",
                RegistrationErrorCode.ValidationFailed => "One or more validation errors occurred.",
                // ... другие сообщения
                _ => "An unexpected error occurred."
            };
        }
    }
}
