using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Domain.DomainModels;

namespace MoneyFlow.AuthenticationService.Application.Mapper
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this UserDomain userDomain)
        {
            return new UserDTO
            {
                IdUser = userDomain.IdUser,
                Login = userDomain.Login,
                UserName = userDomain.UserName,
                Email = userDomain.Email,
                Phone = userDomain.Phone,
                IdGender = userDomain.IdGender,
            };
        }
    }
}
