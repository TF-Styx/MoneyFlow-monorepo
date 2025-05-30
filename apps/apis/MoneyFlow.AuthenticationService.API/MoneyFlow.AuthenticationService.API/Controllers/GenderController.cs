using Microsoft.AspNetCore.Mvc;
using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;

namespace MoneyFlow.AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GenderController
    {
        private readonly IGetAllStreamingGenderUseCase _getAllStreamingGenderUseCase;

        public GenderController(IGetAllStreamingGenderUseCase getAllStreamingGenderUseCase)
        {
            _getAllStreamingGenderUseCase = getAllStreamingGenderUseCase;
        }

        [HttpGet("get-all")]
        public IAsyncEnumerable<GenderDTO> GetAllStreamingGender(CancellationToken cancellationToken) 
            => _getAllStreamingGenderUseCase.GetAllStreamingAsync(cancellationToken);
    }
}
