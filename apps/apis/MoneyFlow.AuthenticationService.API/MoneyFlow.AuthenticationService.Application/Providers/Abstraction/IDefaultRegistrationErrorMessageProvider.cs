using MoneyFlow.AuthenticationService.Application.Enums;

namespace MoneyFlow.AuthenticationService.Application.Providers.Abstraction
{
    public interface IDefaultRegistrationErrorMessageProvider
    {
        string GetMessage(RegistrationErrorCode errorCode);
    }
}