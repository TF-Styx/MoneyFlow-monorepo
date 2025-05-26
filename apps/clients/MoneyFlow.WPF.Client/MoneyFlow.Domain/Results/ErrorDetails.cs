using MoneyFlow.Domain.Enums;

namespace MoneyFlow.Domain.Results
{
    public record class ErrorDetails(ErrorCode? errorCode, string description)
    {
        public static readonly ErrorDetails None = new ErrorDetails(ErrorCode.None, string.Empty);
    }
}
