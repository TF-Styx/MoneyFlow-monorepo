using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;

namespace MoneyFlow.AuthenticationService.Application.UseCases.Realization.GenderUseCases
{
    public class DeleteGenderUseCase(IGenderRepository genderRepository) : IDeleteGenderUseCase
    {
        private readonly IGenderRepository _genderRepository = genderRepository;

        public async Task<int> DeleteAsync(int idGender)
        {
            var exist = await _genderRepository.ExistAsync(idGender);

            if (exist is true)
                await _genderRepository.DeleteAsync(idGender);

            return idGender;
        }
    }
}
