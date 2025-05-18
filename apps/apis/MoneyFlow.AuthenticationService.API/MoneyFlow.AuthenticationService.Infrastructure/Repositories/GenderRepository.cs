using Microsoft.EntityFrameworkCore;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Domain.DomainModels;
using MoneyFlow.AuthenticationService.Infrastructure.Data;
using MoneyFlow.AuthenticationService.Infrastructure.Data.Entities;
using System.Runtime.CompilerServices;

namespace MoneyFlow.AuthenticationService.Infrastructure.Repositories
{
    public class GenderRepository(Context context) : IGenderRepository
    {
        private readonly Context _context = context;

        public async Task<GenderDomain> CreateAsync(GenderDomain genderDomain)
        {
            var entity = new Gender { IdGender = 0, GenderName = genderDomain.GenderName };

            var (Gender, Message) = GenderDomain.Create(entity.IdGender, entity.GenderName);

            if (Gender is not null)
            {
                await _context.Genders.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            return Gender;
        }

        public async IAsyncEnumerable<GenderDomain> GetAllStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var entitiesStreaming = _context.Genders.AsNoTracking().AsAsyncEnumerable().WithCancellation(cancellationToken);

            await foreach (var entity in entitiesStreaming)
            {
                var (Gender, Message) = GenderDomain.Create(entity.IdGender, entity.GenderName);

                if (Gender is not null)
                    yield return Gender;
            }
        }

        public async Task<GenderDomain> GetByIdAsync(int idGender)
        {
            var entity = await _context.Genders.AsNoTracking().FirstOrDefaultAsync(x => x.IdGender == idGender);

            var (Gender, Message) = GenderDomain.Create(entity.IdGender, entity.GenderName);

            return Gender;
        }

        public async Task<GenderDomain?> UpdateAsync(GenderDomain genderDomain)
        {
            var entity = await _context.Genders.FirstOrDefaultAsync(x => x.IdGender == genderDomain.IdGender);

            entity.GenderName = genderDomain.GenderName;

            var (Gender, Message) = GenderDomain.Create(entity.IdGender, entity.GenderName);

            await _context.SaveChangesAsync();

            return Gender;
        }

        public async Task<int> DeleteAsync(int idGender)
        {
            await _context.Genders.Where(x => x.IdGender == idGender).ExecuteDeleteAsync();

            await _context.SaveChangesAsync();

            return idGender;
        }

        public async Task<bool> ExistAsync(int idGender) => await _context.Genders.AnyAsync(x => x.IdGender == idGender);
    }
}
