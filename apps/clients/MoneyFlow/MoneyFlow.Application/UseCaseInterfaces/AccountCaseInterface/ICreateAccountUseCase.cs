using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface
{
    public interface ICreateAccountUseCase
    {
        Task<(AccountDTO AccountDTO, string Message)> CreateAsync(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);
        (AccountDTO AccountDTO, string Message) Create(int? numberAccount, int idUser, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);
    }
}