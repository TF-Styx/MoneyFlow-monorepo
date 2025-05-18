using MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.GenderCases
{
    public class UpdateGenderUseCase : IUpdateGenderUseCase
    {
        private readonly IGendersRepository _gendersRepository;

        public UpdateGenderUseCase(IGendersRepository gendersRepository)
        {
            _gendersRepository = gendersRepository;
        }

        public async Task<int> UpdateAsyncGender(int idGender, string genderName)
        {
            if (string.IsNullOrWhiteSpace(genderName))
            {
                throw new Exception("Данного пола не существует!!");
            }

            var existGender = await _gendersRepository.GetAsync(idGender);

            if (existGender == null)
            {
                throw new Exception("Данного пола не существует!!");
            }

            return await _gendersRepository.UpdateAsync(idGender, genderName);
        }
        public int UpdateGender(int idGender, string genderName)
        {
            if (string.IsNullOrWhiteSpace(genderName))
            {
                throw new Exception("Данного пола не существует!!");
            }

            var existGender = _gendersRepository.Get(idGender);

            if (existGender == null)
            {
                throw new Exception("Данного пола не существует!!");
            }

            return _gendersRepository.Update(idGender, genderName);
        }
    }
}
