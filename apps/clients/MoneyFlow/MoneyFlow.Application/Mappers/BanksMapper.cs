using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;
using System.Collections.ObjectModel;

namespace MoneyFlow.Application.Mappers
{
    internal static class BanksMapper
    {
        public static (BankDTO BankDTO, string Message) ToDTO(this BankDomain bank)
        {
            string message = string.Empty;

            if (bank == null) { return (null, "Банк не найден!!"); }

            var dto = new BankDTO()
            {
                IdBank = bank.IdBank,
                BankName = bank.BankName,
            };

            return (dto, message);
        }

        public static UserBanksDTO ToDTO(this UserBanksDomain userBanks)
        {
            var obs = new ObservableCollection<BankDTO>();

            foreach (var item in userBanks.Banks.ToListDTO())
            {
                obs.Add(item);
                var index = obs.IndexOf(item);
                item.Index = index + 1;
            }

            var dto = new UserBanksDTO()
            {
                IdUser = userBanks.IdUser,
                Banks = obs,
            };

            return dto;
        }

        public static List<BankDTO> ToListDTO(this IEnumerable<BankDomain> banks)
        {
            var list = new List<BankDTO>();

            foreach (var item in banks)
            {
                list.Add(item.ToDTO().BankDTO);
            }

            return list;
        }

        public static (BankDomain BankDomain, string Message) ToDomain(this BankDTO bank)
        {
            if (bank == null) { return (null, "Банк не найден!!"); }

            return BankDomain.Create(bank.IdBank, bank.BankName);
        }

        public static List<BankDomain> ToListDomain(this IEnumerable<BankDTO> banks)
        {
            var list = new List<BankDomain>();

            foreach (var item in banks)
            {
                list.Add(item.ToDomain().BankDomain);
            }

            return list;
        }
    }
}
