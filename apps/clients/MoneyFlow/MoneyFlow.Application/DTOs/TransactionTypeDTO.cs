using MoneyFlow.Application.DTOs.BaseDTOs;

namespace MoneyFlow.Application.DTOs
{
    public class TransactionTypeDTO : BaseDTO<TransactionTypeDTO>
    {
        public int IdTransactionType { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? Description { get; set; }
    }
}
