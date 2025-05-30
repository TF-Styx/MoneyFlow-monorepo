using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.UseCases.Abstraction.GenderUseCases
{
    public interface IGetAllGenderUseCase
    {
        Task<Result<List<GenderDTO>>> GetAllAsync();
    }
}