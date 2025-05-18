using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.GenderCases
{
    public class GetGenderUseCase : IGetGenderUseCase
    {
        private readonly IGendersRepository _gendersRepository;

        public GetGenderUseCase(IGendersRepository gendersRepository)
        {
            _gendersRepository = gendersRepository;
        }

        public async Task<List<GenderDTO>> GetAllAsyncGender()
        {
            var genders = await _gendersRepository.GetAllAsync();
            var gendersDTO = genders.ToListDTO();

            return gendersDTO;
        }
        public List<GenderDTO> GetAllGender()
        {
            var genders = _gendersRepository.GetAll();
            var gendersDTO = genders.ToListDTO();

            return gendersDTO;
        }

        public async Task<GenderDTO> GetAsyncGender(int idUser)
        {
            var gender = await _gendersRepository.GetAsync(idUser);

            if (gender == null) { return null; } // TODO : Сделать исключение

            var genderDTO = gender.ToDTO();

            return genderDTO.GenderDTO;
        }
        public GenderDTO GetGender(int idUser)
        {
            var gender = _gendersRepository.Get(idUser);

            if (gender == null) { return null; } // TODO : Сделать исключение

            var genderDTO = gender.ToDTO();

            return genderDTO.GenderDTO;
        }

        public async Task<GenderDTO> GetAsyncGender(string userName)
        {
            var gender = await _gendersRepository.GetAsync(userName);

            if (gender == null) { return null; } // TODO : Сделать исключение

            var genderDTO = gender.ToDTO();

            return genderDTO.GenderDTO;
        }
        public GenderDTO GetGender(string userName)
        {
            var gender = _gendersRepository.Get(userName);

            if (gender == null) { return null; } // TODO : Сделать исключение

            var genderDTO = gender.ToDTO();

            return genderDTO.GenderDTO;
        }
    }
}
