using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this UserDomain userDomain)
        {
            return new UserDTO()
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
