using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    public static class TransactionTypeMapper
    {
        public static (TransactionTypeDTO TransactionTypeDTO, string Message) ToDTO(this TransactionTypeDomain transactionType)
        {
            string message = string.Empty;

            if (transactionType == null) { return (null, "Тип транзакции не найден!!"); }

            var dto = new TransactionTypeDTO()
            {
                IdTransactionType = transactionType.IdTransactionType,
                TransactionTypeName = transactionType.TransactionTypeName,
                Description = transactionType.Description
            };

            return (dto, message);
        }

        public static List<TransactionTypeDTO> ToListDTO(this IEnumerable<TransactionTypeDomain> transactionTypes)
        {
            var list = new List<TransactionTypeDTO>();

            foreach (var item in transactionTypes)
            {
                list.Add(item.ToDTO().TransactionTypeDTO);
            }

            return list;
        }
    }
}
