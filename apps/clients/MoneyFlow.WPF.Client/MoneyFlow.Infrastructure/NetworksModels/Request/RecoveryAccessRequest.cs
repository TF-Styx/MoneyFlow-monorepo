namespace MoneyFlow.Infrastructure.NetworksModels.Request
{
    public class RecoveryAccessRequest
    {
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
