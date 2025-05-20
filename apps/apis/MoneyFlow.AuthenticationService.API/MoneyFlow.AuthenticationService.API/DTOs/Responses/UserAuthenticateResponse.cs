namespace MoneyFlow.AuthenticationService.API.DTOs.Responses
{
    public class UserAuthenticateResponse
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int? IdGender { get; set; }
    }
}
