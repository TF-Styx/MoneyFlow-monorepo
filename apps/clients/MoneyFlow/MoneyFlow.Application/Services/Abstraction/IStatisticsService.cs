using MoneyFlow.Application.ApplicationModel;
using MoneyFlow.Application.DTOs;

namespace MoneyFlow.Application.Services.Abstraction
{
    public interface IStatisticsService
    {
        IEnumerable<NameValue> DetailingTransaction<TCollectionItem>(List<FinancialRecordViewingDTO> records, Func<FinancialRecordViewingDTO, TCollectionItem, bool> recordSelector, Func<FinancialRecordViewingDTO, bool> transactionTypeSelector, List<TCollectionItem> collectionItem, Func<TCollectionItem, string> collectionItemSelector);
    }
}