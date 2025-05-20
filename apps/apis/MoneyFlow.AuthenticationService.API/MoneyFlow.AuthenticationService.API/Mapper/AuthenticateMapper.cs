using MoneyFlow.AuthenticationService.API.DTOs.Request;
using MoneyFlow.AuthenticationService.Application.DTOs.Commands;

namespace MoneyFlow.AuthenticationService.API.Mapper
{
    public static class AuthenticateMapper
    {
        public static AuthenticateUserCommand ToMap(this AuthenticateUserApiRequest request)
        {
            return new AuthenticateUserCommand
            {
                Login = request.Login,
                Password = request.Password
            };
        }
    }
}
