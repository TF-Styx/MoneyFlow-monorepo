namespace MoneyFlow.AuthenticationService.Application.DTOs.Commands
{
    public class AuthenticateUserCommand
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
