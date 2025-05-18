using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Domain.DomainModels;

namespace MoneyFlow.AuthenticationService.Application.Mapper
{
    public static class RoleMapper
    {
        public static RoleDTO ToDTO(this RoleDomain roleDomain)
        {
            return new RoleDTO
            {
                IdRole = roleDomain.IdRole,
                RoleName = roleDomain.RoleName,
            };
        }
    }
}
