namespace MoneyFlow.AuthenticationService.Application.DTOs.Commands
{
    public class RegisterUserCommand
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int? IdGender { get; set; }
    }
}
