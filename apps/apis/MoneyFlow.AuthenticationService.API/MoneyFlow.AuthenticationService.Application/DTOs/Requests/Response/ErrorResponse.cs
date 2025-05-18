namespace MoneyFlow.AuthenticationService.Application.DTOs.Requests.Response
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
