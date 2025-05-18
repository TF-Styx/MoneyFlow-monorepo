using Microsoft.EntityFrameworkCore;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Domain.DomainModels;
using MoneyFlow.AuthenticationService.Infrastructure.Data;
using MoneyFlow.AuthenticationService.Infrastructure.Data.Entities;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Infrastructure.Repositories
{
    public class RoleRepository(Context context) : IRoleRepository
    {
        private readonly Context _context = context;

        public async Task<RoleDomain> CreateAsync(RoleDomain roleDomain)
        {
            var entity = new Role { IdRole = 0, RoleName = roleDomain.RoleName };

            var (Role, Message) = RoleDomain.Create(0, entity.RoleName);

            if (Role is not null)
            {
                await _context.Roles.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            return Role;
        }

        public async IAsyncEnumerable<RoleDomain> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var entitiesStream = _context.Roles.AsNoTracking().AsAsyncEnumerable().WithCancellation(cancellationToken);

            await foreach (var entity in entitiesStream)
            {
                var (Role, Message) = RoleDomain.Create(entity.IdRole, entity.RoleName);

                if (Role is not null)
                    yield return Role;
            }
        }

        public async Task<RoleDomain?> GetByIdAsync(int idRole)
        {
            var entity = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.IdRole == idRole);

            var (Role, Message) = RoleDomain.Create(entity.IdRole, entity.RoleName);

            return Role;
        }

        public async Task<RoleDomain?> UpdateAsync(RoleDomain roleDomain)
        {
            var entity = await _context.Roles.FirstOrDefaultAsync(x => x.IdRole == roleDomain.IdRole);

            entity.RoleName = roleDomain.RoleName;

            var (Role, Message) = RoleDomain.Create(entity.IdRole, entity.RoleName);

            await _context.SaveChangesAsync();

            return Role;
        }

        public async Task<int> DeleteAsync(int idRole)
        {
            await _context.Roles.Where(x => x.IdRole == idRole).ExecuteDeleteAsync();

            await _context.SaveChangesAsync();

            return idRole;
        }

        public async Task<bool> ExistAsync(int idRole) => await _context.Roles.AnyAsync(x => x.IdRole == idRole);
    }
}
