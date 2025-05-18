namespace MoneyFlow.Application.DTOs
{
    public class UserTotalInfoDTO
    {
        public string? GenderName { get; set; }
        public decimal? TotalBalance { get; set; }
        public int AccountCount { get; set; }
        public int AccountTypeCount { get; set; }
        public int BankCount { get; set; }
        public int CategoryCount { get; set; }
        public int SubcategoryCount { get; set; }
        public int FinancialRecordCount { get; set; }
    }
}
