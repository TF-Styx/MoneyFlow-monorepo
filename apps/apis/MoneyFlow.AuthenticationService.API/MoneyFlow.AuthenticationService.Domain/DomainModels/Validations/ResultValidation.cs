namespace MoneyFlow.AuthenticationService.Domain.DomainModels.Validations
{
    public class ResultValidation
    {
        private ResultValidation(bool isValid, IReadOnlyList<string> errorList)
        {
            IsValid = isValid;
            ErrorList = errorList;
        }

        public bool IsValid { get; private set; }
        public IReadOnlyList<string> ErrorList { get; private set; }

        public static ResultValidation Success() => new ResultValidation(true, null);

        public static ResultValidation Failure(string error) => new ResultValidation(false, [error]);

        public static ResultValidation Failure(List<string> errors) => new ResultValidation(false, errors);
    }
}
