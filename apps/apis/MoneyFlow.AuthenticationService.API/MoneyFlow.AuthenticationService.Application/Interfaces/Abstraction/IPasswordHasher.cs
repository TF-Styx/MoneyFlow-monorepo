namespace MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
