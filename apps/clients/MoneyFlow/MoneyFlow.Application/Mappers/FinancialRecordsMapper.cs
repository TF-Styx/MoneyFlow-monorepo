using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    internal static class FinancialRecordsMapper
    {
        public static (FinancialRecordDTO FinancialRecordDTO, string Message) ToDTO(this FinancialRecordDomain financialRecord)
        {
            string message = string.Empty;

            if (financialRecord == null) { return (null, "Данной финансовой записи нет!!"); }

            var dto = new FinancialRecordDTO()
            {
                IdFinancialRecord = financialRecord.IdFinancialRecord,
                RecordName = financialRecord.RecordName,
                Amount = financialRecord.Amount,
                Description = financialRecord.Description,
                IdTransactionType = financialRecord.IdTransactionType,
                IdUser = financialRecord.IdUser,
                IdCategory = financialRecord.IdCategory,
                IdSubcategory = financialRecord.IdSubcategory,
                IdAccount = financialRecord.IdAccount,
                Date = financialRecord.Date,
            };

            return (dto, message);
        }

        public static List<FinancialRecordDTO> ToListDTO(this IEnumerable<FinancialRecordDomain> financialRecords)
        {
            var list = new List<FinancialRecordDTO>();

            foreach (var item in financialRecords)
            {
                list.Add(item.ToDTO().FinancialRecordDTO);
            }
            return list;
        }


        public static (FinancialRecordViewingDTO FinancialRecordViewingDTO, string Message) ToDTO(this FinancialRecordViewingDomain financialRecordViewing)
        {
            string message = string.Empty;

            if (financialRecordViewing == null) { return (null, "Данной финансовой записи нет!!"); }

            var dto = new FinancialRecordViewingDTO()
            {
                IdFinancialRecord = financialRecordViewing.IdFinancialRecord,
                RecordName = financialRecordViewing.RecordName,
                Amount = financialRecordViewing.Amount,
                Description = financialRecordViewing.Description,
                IdTransactionType = financialRecordViewing.IdTransactionType,
                TransactionTypeName = financialRecordViewing.TransactionTypeName,
                IdUser = financialRecordViewing.IdUser,
                IdCategory = financialRecordViewing.IdCategory,
                CategoryName = financialRecordViewing.CategoryName,
                IdSubcategory = financialRecordViewing.IdSubcategory,
                SubcategoryName = financialRecordViewing.SubcategoryName,
                AccountNumber = financialRecordViewing.AccountNumber,
                Date = financialRecordViewing.Date,
            };

            return (dto, message);
        }

        public static List<FinancialRecordViewingDTO> ToListDTO(this IEnumerable<FinancialRecordViewingDomain> financialRecordViewings)
        {
            var list = new List<FinancialRecordViewingDTO>();

            foreach (var item in financialRecordViewings)
            {
                list.Add(item.ToDTO().FinancialRecordViewingDTO);
            }
            return list;
        }


        public static FinancialRecordFilterDomain ToDomain(this FinancialRecordFilterDTO filter)
        {
            return new FinancialRecordFilterDomain()
            {
                AmountStart = filter.AmountStart,
                AmountEnd = filter.AmountEnd,
                IsConsiderAmount = filter.IsConsiderAmount,

                IdTransactionType = filter.IdTransactionType,
                IdCategory = filter.IdCategory,
                IdSubcategory = filter.IdSubcategory,
                IdAccount = filter.IdAccount,

                DateStart = filter.DateStart,
                DateEnd = filter.DateEnd,
                IsConsiderDate = filter.IsConsiderDate,
            };
        }
    }
}
