using MoneyFlow.AuthenticationService.Application.DTOs;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases
{
    public interface ICreateRoleUseCase
    {
        Task<(RoleDTO? RoleDTO, string Message)> CreateAsync(RoleDTO roleDTO);
    }
}