using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases;
using MoneyFlow.AuthenticationService.Domain.DomainModels;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.RoleUseCases
{
    public class CreateRoleUseCase(IRoleRepository roleRepository) : ICreateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<(RoleDTO? RoleDTO, string Message)> CreateAsync(RoleDTO roleDTO)
        {
            var (Domain, Message) = RoleDomain.Create
                (
                    roleDTO.IdRole,
                    roleDTO.RoleName
                );

            if (Domain is null)
                return (null, Message);

            var domain = await _roleRepository.CreateAsync(Domain);

            return (domain.ToDTO(), Message);
        }
    }
}
