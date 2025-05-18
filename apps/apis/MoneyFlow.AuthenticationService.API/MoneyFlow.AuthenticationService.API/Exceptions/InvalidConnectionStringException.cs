namespace MoneyFlow.AuthenticationService.API.Exceptions
{
    public sealed class InvalidConnectionStringException : Exception
    {
        public InvalidConnectionStringException()
        {
            
        }

        public InvalidConnectionStringException(string message) : base(message)
        {
            
        }

        public InvalidConnectionStringException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
