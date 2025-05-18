using MoneyFlow.AuthenticationService.API.DTOs.Request;
using MoneyFlow.AuthenticationService.Application.DTOs.Commands;

namespace MoneyFlow.AuthenticationService.API.Mapper
{
    public static class RegisterMapper
    {
        public static RegisterUserCommand ToMap(this RegisterUserApiRequest request)
        {
            return new RegisterUserCommand
            {
                Login = request.Login,
                Password = request.Password,
                UserName = request.UserName,
                Email = request.Email,
                Phone = request.Phone,
                IdGender = request.IdGender,
            };
        }
    }
}
