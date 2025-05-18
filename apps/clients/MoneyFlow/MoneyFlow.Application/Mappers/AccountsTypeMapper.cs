using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;
using System.Collections.ObjectModel;

namespace MoneyFlow.Application.Mappers
{
    internal static class AccountsTypeMapper
    {
        public static (AccountTypeDTO AccountTypeDTO, string Message) ToDTO(this AccountTypeDomain accountType)
        {
            string message = string.Empty;

            if (accountType == null) { return (null, "Тип счета не найден!!"); }

            var dto = new AccountTypeDTO()
            {
                IdAccountType = accountType.IdAccountType,
                AccountTypeName = accountType.AccountTypeName,
            };

            return (dto, message);
        }

        public static UserAccountTypesDTO ToDTO(this UserAccountTypesDomain userAccountTypes)
        {
            var accountTypeList = new ObservableCollection<AccountTypeDTO>();

            foreach (var item in userAccountTypes.AccountTypes.ToListDTO())
            {
                accountTypeList.Add(item);
                var index = accountTypeList.IndexOf(item);
                item.Index = index + 1;
            }

            var dto = new UserAccountTypesDTO()
            {
                IdUser = userAccountTypes.IdUser,
                AccountTypes = accountTypeList
            };

            return dto;
        }

        public static List<AccountTypeDTO> ToListDTO(this IEnumerable<AccountTypeDomain> accountsType)
        {
            var list = new List<AccountTypeDTO>();

            foreach (var item in accountsType)
            {
                list.Add(item.ToDTO().AccountTypeDTO);
            }

            return list;
        }

        public static (AccountTypeDomain AccountTypeDomain, string Message) ToDomain(this AccountTypeDTO accountType)
        {
            if (accountType == null) { return (null, "Тип счета не найден!!"); }

            return AccountTypeDomain.Create(accountType.IdAccountType, accountType.AccountTypeName);
        }

        public static List<AccountTypeDomain> ToListDomain(this IEnumerable<AccountTypeDTO> accountsType)
        {
            var list = new List<AccountTypeDomain>();

            foreach (var item in accountsType)
            {
                list.Add(item.ToDomain().AccountTypeDomain);
            }

            return list;
        }
    }
}
