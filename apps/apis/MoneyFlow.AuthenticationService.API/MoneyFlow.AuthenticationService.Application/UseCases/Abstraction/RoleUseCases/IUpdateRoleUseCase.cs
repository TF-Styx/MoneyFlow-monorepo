using MoneyFlow.AuthenticationService.Application.DTOs;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases
{
    public interface IUpdateRoleUseCase
    {
        Task<(RoleDTO? RoleDTO, string Message)> UpdateAsync(RoleDTO roleDTO);
    }
}