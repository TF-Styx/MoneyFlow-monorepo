using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IGenderService
    {
        Task<(GenderDTO GenderDTO, string Message)> CreateAsyncGender(string genderName);
        (GenderDTO GenderDTO, string Message) CreateGender(string genderName);
        
        Task<List<GenderDTO>> GetAllAsyncGender();
        List<GenderDTO> GetAllGender();

        Task<GenderDTO> GetAsyncGender(int idGender);
        GenderDTO GetGender(int idGender);

        Task<GenderDTO> GetAsyncGender(string genderName);
        GenderDTO GetGender(string genderName);

        Task<int> UpdateAsyncGender(int idGender, string genderName);
        int UpdateGender(int idGender, string genderName);

        Task DeleteAsyncGender(int idGender);
        void DeleteGender(int idGender);
    }
}