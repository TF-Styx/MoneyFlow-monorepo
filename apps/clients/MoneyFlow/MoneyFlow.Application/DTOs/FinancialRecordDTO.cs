using MoneyFlow.Application.DTOs.BaseDTOs;
using MoneyFlow.Shared.Constants;

namespace MoneyFlow.Application.DTOs
{
    public class FinancialRecordDTO : BaseDTO<FinancialRecordDTO>
    {
        public int IdFinancialRecord { get; set; }
        public string? RecordName { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public int? IdTransactionType { get; set; }
        public int? IdUser { get; set; }
        public int? IdCategory { get; set; }
        public int? IdSubcategory { get; set; }
        public int? IdAccount { get; set; }
        public DateTime? Date { get; set; }
    }
}
