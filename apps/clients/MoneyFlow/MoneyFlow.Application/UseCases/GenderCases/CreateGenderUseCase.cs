using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces;
using MoneyFlow.Domain.DomainModels;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.GenderCases
{
    public class CreateGenderUseCase : ICreateGenderUseCase
    {
        private readonly IGendersRepository _genderRepository;

        public CreateGenderUseCase(IGendersRepository genderRepository)
        {
            _genderRepository = genderRepository;
        }

        public async Task<(GenderDTO GenderDTO, string Message)> CreateAsyncGender(string genderName) // Передача данных для создания записи в БД
        {
            var (CreatedGenderDomain, Message) = GenderDomain.Create(0, genderName); // Проверка валидность данных, путем создания DomainModel

            if (CreatedGenderDomain is null) { return (null, Message); }

            var existGender = await _genderRepository.GetAsync(CreatedGenderDomain.GenderName);

            if (existGender != null)
            {
                return (null, "Пол с таким именем уже есть!!");
            }

            var idGender = await _genderRepository.CreateAsync(genderName);
            var genderDomain = await _genderRepository.GetAsync(idGender);

            return (genderDomain.ToDTO().GenderDTO, Message);
        }
        public (GenderDTO GenderDTO, string Message) CreateGender(string genderName)
        {
            var (CreatedGenderDomain, Message) = GenderDomain.Create(0, genderName); // Проверка валидность данных, путем создания DomainModel

            if (CreatedGenderDomain is null) { return (null, Message); }

            var existGender = _genderRepository.Get(CreatedGenderDomain.GenderName);

            if (existGender != null)
            {
                return (null, "Пол с таким именем уже есть!!");
            }

            var idGender = _genderRepository.Create(genderName);
            var genderDomain = _genderRepository.Get(idGender);

            return (genderDomain.ToDTO().GenderDTO, Message);
        }
    }
}
