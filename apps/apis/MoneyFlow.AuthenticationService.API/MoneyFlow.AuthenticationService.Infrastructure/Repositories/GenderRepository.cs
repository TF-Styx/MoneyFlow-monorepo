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
    }
}
