using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces;

namespace MoneyFlow.Application.Services.Realization
{
    public class GenderService : IGenderService
    {
        private readonly ICreateGenderUseCase _createGenderUseCase;
        private readonly IDeleteGenderUseCase _deleteGenderUseCase;
        private readonly IGetGenderUseCase    _getGenderUseCase;
        private readonly IUpdateGenderUseCase _updateGenderUseCase;

        public GenderService(ICreateGenderUseCase createGenderUseCase, IDeleteGenderUseCase deleteGenderUseCase, IGetGenderUseCase getGenderUseCase, IUpdateGenderUseCase updateGenderUseCase)
        {
            _createGenderUseCase = createGenderUseCase;
            _deleteGenderUseCase = deleteGenderUseCase;
            _getGenderUseCase    = getGenderUseCase;
            _updateGenderUseCase = updateGenderUseCase;
        }

        public async Task<(GenderDTO GenderDTO, string Message)> CreateAsyncGender(string genderName)
        {
            return await _createGenderUseCase.CreateAsyncGender(genderName);
        }
        public (GenderDTO GenderDTO, string Message) CreateGender(string genderName)
        {
            return _createGenderUseCase.CreateGender(genderName);
        }

        public async Task<List<GenderDTO>> GetAllAsyncGender()
        {
            return await _getGenderUseCase.GetAllAsyncGender();
        }
        public List<GenderDTO> GetAllGender()
        {
            return _getGenderUseCase.GetAllGender();
        }

        public async Task<GenderDTO> GetAsyncGender(int idGender)
        {
            return await _getGenderUseCase.GetAsyncGender(idGender);
        }
        public GenderDTO GetGender(int idGender)
        {
            return _getGenderUseCase.GetGender(idGender);
        }

        public async Task<GenderDTO> GetAsyncGender(string genderName)
        {
            return await _getGenderUseCase.GetAsyncGender(genderName);
        }
        public GenderDTO GetGender(string genderName)
        {
            return _getGenderUseCase.GetGender(genderName);
        }

        public async Task<int> UpdateAsyncGender(int idGender, string genderName)
        {
            return await _updateGenderUseCase.UpdateAsyncGender(idGender, genderName);
        }
        public int UpdateGender(int idGender, string genderName)
        {
            return _updateGenderUseCase.UpdateGender(idGender, genderName);
        }

        public async Task DeleteAsyncGender(int idGender)
        {
            await _deleteGenderUseCase.DeleteAsyncGender(idGender);
        }
        public void DeleteGender(int idGender)
        {
            _deleteGenderUseCase.DeleteGender(idGender);
        }
    }
}
