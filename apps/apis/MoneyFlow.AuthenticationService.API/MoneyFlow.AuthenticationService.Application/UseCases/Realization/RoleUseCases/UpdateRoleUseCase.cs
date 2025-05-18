using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases;
using MoneyFlow.AuthenticationService.Domain.DomainModels;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.RoleUseCases
{
    public class UpdateRoleUseCase(IRoleRepository roleRepository) : IUpdateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<(RoleDTO? RoleDTO, string Message)> UpdateAsync(RoleDTO roleDTO)
        {
            var (Domain, Message) = RoleDomain.Create
                (
                    roleDTO.IdRole,
                    roleDTO.RoleName
                );

            if (Domain is null)
                return (null, Message);

            var exist = await _roleRepository.ExistAsync(Domain.IdRole);

            if (exist is false)
                return (null, "Роль не найдена!!");

            var newDomain = await _roleRepository.UpdateAsync(Domain);

            return (newDomain!.ToDTO(), "Данные успешно изменены!!");
        }
    }
}
