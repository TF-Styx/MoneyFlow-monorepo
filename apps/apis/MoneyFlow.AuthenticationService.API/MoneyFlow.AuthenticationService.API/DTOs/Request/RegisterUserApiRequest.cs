using System.ComponentModel.DataAnnotations;

namespace MoneyFlow.AuthenticationService.API.DTOs.Request
{
    public class RegisterUserApiRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int? IdGender { get; set; }
        public int? IdRole { get; set; }
    }
}
