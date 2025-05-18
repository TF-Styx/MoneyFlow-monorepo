using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Mapper;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.RoleUseCases
{
    public class GetAllStreamingRoleUseCase(IRoleRepository roleRepository) : IGetAllStreamingRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public IAsyncEnumerable<RoleDTO> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return _roleRepository.GetAllStreamingAsync(cancellationToken).Select(domain => domain.ToDTO());
        }
    }
}
