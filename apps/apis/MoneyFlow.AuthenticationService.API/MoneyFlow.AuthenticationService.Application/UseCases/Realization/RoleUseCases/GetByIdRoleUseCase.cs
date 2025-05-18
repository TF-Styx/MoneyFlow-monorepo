using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.RoleUseCases
{
    public class GetByIdRoleUseCase(IRoleRepository roleRepository) : IGetByIdRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<(RoleDTO? RoleDTO, string Message)> GetByIdAsync(int idRole)
        {
            var domain = await _roleRepository.GetByIdAsync(idRole);

            if (domain is null)
                return (null, "Роль не найдена!!");

            return (domain.ToDTO(), string.Empty);
        }
    }
}
