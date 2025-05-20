namespace MoneyFlow.AuthenticationService.API.DTOs.Request
{
    public class AuthenticateUserApiRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
