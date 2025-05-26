namespace MoneyFlow.Domain.Results
{
    public class Result<T>
    {
        public T? Value { get; private set; }
        public bool Success { get; private set; }
        private readonly List<ErrorDetails> _errorDetails;
        public IReadOnlyList<ErrorDetails> ErrorDetails => _errorDetails.AsReadOnly();

        public Result(T? value, bool success, List<ErrorDetails> errorDetails)
        {
            Value = value;
            Success = success;
            _errorDetails = errorDetails;
        }

        public static Result<T> SuccessResult(T value) => new Result<T>(value, true, null!);

        public static Result<T> FailureResult(params ErrorDetails[] errorDetails) => new Result<T>(default, false, [.. errorDetails]);
    }
}
