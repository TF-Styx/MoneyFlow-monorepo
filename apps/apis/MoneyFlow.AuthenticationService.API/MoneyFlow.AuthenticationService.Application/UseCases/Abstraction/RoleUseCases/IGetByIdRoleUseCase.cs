using MoneyFlow.AuthenticationService.Application.DTOs;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases
{
    public interface IGetByIdRoleUseCase
    {
        Task<(RoleDTO? RoleDTO, string Message)> GetByIdAsync(int idRole);
    }
}