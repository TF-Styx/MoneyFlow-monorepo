using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.InterfaceRepositories
{
    public interface IGenderRepository
    {
        Task<Result<List<GenderDomain>>> GetAllAsync();
    }
}