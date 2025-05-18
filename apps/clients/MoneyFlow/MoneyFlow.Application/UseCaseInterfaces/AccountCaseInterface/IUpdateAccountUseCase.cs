using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface
{
    public interface IUpdateAccountUseCase
    {
        Task<int> UpdateAsync(int idAccount, int? numberAccount, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);
        int Update(int idAccount, int? numberAccount, BankDTO bankDTO, AccountTypeDTO accountTypeDTO, decimal? balance);
    }
}