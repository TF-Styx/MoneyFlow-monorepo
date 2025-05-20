using MoneyFlow.AuthenticationService.Application.Enums;

namespace MoneyFlow.AuthenticationService.Application.Providers.Abstraction
{
    public interface IDefaultErrorMessageProvider
    {
        string GetMessage(ErrorCode errorCode);
    }
}