using MoneyFlow.MVVM.Models.DB_MSSQL;

namespace MoneyFlow.Utils.Services.AuthorizationVerificationServices
{
    public interface IAuthorizationVerificationService
    {
        User CurrentUser { get; }
        bool CheckAuthorization();
        void CreateJsonUser(User user);
    }
}
