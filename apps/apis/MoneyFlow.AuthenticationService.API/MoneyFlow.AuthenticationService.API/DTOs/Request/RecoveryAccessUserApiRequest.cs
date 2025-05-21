namespace MoneyFlow.AuthenticationService.API.DTOs.Request
{
    public class RecoveryAccessUserApiRequest
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string NewPassword { get; set; }
    }
}
