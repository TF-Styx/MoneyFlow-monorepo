using MoneyFlow.AuthenticationService.API.DTOs.Request;
using MoneyFlow.AuthenticationService.Application.DTOs.Commands;

namespace MoneyFlow.AuthenticationService.API.Mapper
{
    public static class RecoveryMapper
    {
        public static RecoveryAccessUserCommand ToMap(this RecoveryAccessUserApiRequest request)
        {
            return new RecoveryAccessUserCommand
            {
                Email = request.Email,
                Login = request.Login,
                NewPassword = request.NewPassword,
            };
        }
    }
}
