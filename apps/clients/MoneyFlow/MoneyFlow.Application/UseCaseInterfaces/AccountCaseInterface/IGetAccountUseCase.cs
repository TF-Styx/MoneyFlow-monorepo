using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface
{
    public interface IGetAccountUseCase
    {
        Task<List<AccountDTO>> GetAllAsync(int idUser);
        List<AccountDTO> GetAll(int idUser);

        Task<AccountDTO> GetAsync(int idAccount);
        AccountDTO Get(int idAccount);

        Task<AccountDTO> GetAsync(int? numberAccount);
        AccountDTO Get(int? numberAccount);
    }
}