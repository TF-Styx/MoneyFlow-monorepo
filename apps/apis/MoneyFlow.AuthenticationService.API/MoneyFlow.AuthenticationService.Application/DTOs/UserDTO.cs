namespace MoneyFlow.AuthenticationService.Application.DTOs
{
    public class UserDTO
    {
        public int IdUser { get; set; }
        public string Login { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int? IdGender { get; set; }
    }
}
