using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces
{
    public interface IGetBankUseCase
    {
        Task<List<BankDTO>> GetAllAsync();
        List<BankDTO> GetAll();

        Task<BankDTO> GetAsync(int idBank);
        BankDTO Get(int idBank);

        Task<BankDTO> GetAsync(string nameBank);
        BankDTO Get(string nameBank);

        Task<UserBanksDTO> GetByIdUserAsync(int idUser);
        UserBanksDTO GetByIdUser(int idUser);

    }
}