using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    internal static class AccountsMapper
    {
        public static (AccountDTO AccountDTO, string Message) ToDTO(this AccountDomain account)
        {
            string message = string.Empty;

            if (account == null)
            {
                return (null, "Данного счета нет!!");
            }

            var dto = new AccountDTO()
            {
                IdAccount = account.IdAccount,
                NumberAccount = account.NumberAccount,
                Bank = account.Bank.ToDTO().BankDTO,
                AccountType = account.AccountType.ToDTO().AccountTypeDTO,
                Balance = account.Balance
            };

            return (dto, message);
        }

        public static List<AccountDTO> ToListDTO(this IEnumerable<AccountDomain> accounts)
        {
            var list = new List<AccountDTO>();

            foreach (var item in accounts)
            {
                list.Add(item.ToDTO().AccountDTO);
            }

            return list;
        }

        public static (AccountDomain AccountDomain, string Message) ToDomain(this AccountDTO account)
        {
            if (account == null)
            {
                return (null, "Данного счета нет!!");
            }
            return AccountDomain.Create(account.IdAccount,
                                        account.NumberAccount,
                                        account.Bank.ToDomain().BankDomain,
                                        account.AccountType.ToDomain().AccountTypeDomain,
                                        account.Balance);
        }
    }
}
