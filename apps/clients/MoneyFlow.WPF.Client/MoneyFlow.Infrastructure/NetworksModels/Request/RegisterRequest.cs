﻿namespace MoneyFlow.Infrastructure.NetworksModels.Request
{
    public class RegisterRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int? IdGender { get; set; }
    }
}
