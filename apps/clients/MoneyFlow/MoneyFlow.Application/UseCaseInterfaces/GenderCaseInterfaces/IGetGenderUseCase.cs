using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces
{
    public interface IGetGenderUseCase
    {
        Task<List<GenderDTO>> GetAllAsyncGender();
        List<GenderDTO> GetAllGender();

        Task<GenderDTO> GetAsyncGender(int idUser);
        GenderDTO GetGender(int idUser);

        Task<GenderDTO> GetAsyncGender(string userName);
        GenderDTO GetGender(string userName);

    }
}