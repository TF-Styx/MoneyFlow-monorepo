namespace MoneyFlow.AuthenticationService.Application.DTOs.Commands
{
    public class RecoveryAccessUserCommand
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string NewPassword { get; set; }
    }
}
