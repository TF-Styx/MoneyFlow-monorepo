namespace MoneyFlow.Domain.DomainModels
{
    public class FinancialRecordFilterDomain
    {
        public decimal? AmountStart { get; set; }
        public decimal? AmountEnd { get; set; }
        public bool IsConsiderAmount {  get; set; }

        public int? IdTransactionType { get; set; }
        public int? IdCategory { get; set; }
        public int? IdSubcategory { get; set; }
        public int? IdAccount { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool IsConsiderDate { get; set; }
    }
}
