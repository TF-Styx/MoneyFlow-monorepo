using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.InterfaceRepositories;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCases.Abstraction.GenderUseCases;
using MoneyFlow.Domain.Results;

namespace MoneyFlow.Application.UseCases.Realization.GenderUseCases
{
    public class GetAllGenderUseCase : IGetAllGenderUseCase
    {
        private readonly IGenderRepository _repository;

        public GetAllGenderUseCase(IGenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<GenderDTO>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();

            if (result.Success)
                return Result<List<GenderDTO>>.SuccessResult(result.Value!.Select(domain => domain.ToDTO()).ToList());
            else
                return Result<List<GenderDTO>>.FailureResult(result.ErrorDetails.ToArray());
        }
    }
}
