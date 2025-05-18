using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.RoleUseCases
{
    public class DeleteRoleUseCase(IRoleRepository roleRepository) : IDeleteRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<int> DeleteAsync(int idRole)
        {
            var exist = await _roleRepository.ExistAsync(idRole);

            if (exist is true)
                await _roleRepository.DeleteAsync(idRole);

            return idRole;
        }
    }
}
