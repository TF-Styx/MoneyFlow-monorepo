using MoneyFlow.Application.UseCaseInterfaces.SubcategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.SubcategoryCases
{
    public class DeleteSubcategoryUseCase : IDeleteSubcategoryUseCase
    {
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly ICatLinkSubRepository _catLinkSubRepository;
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public DeleteSubcategoryUseCase(ISubcategoryRepository subcategoryRepository, ICatLinkSubRepository catLinkSubRepository, IFinancialRecordRepository financialRecordRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _catLinkSubRepository = catLinkSubRepository;
            _financialRecordRepository = financialRecordRepository;
        }

        public async Task<string?> ExistRelatedDataAsync(int idSubcategory)
        {
            string? messageFinancialRecord = null;

            var (idUsedCatLinkSub, idUsedFinancialRecord) = await _subcategoryRepository.ExistRelatedDataAsync(idSubcategory);

            if (idUsedFinancialRecord)
            {
                messageFinancialRecord = "У вас есть связанные данные!!\nХотите их удалить?";
            }

            return messageFinancialRecord;
        }

        public async Task<List<int>> DeleteAsync(int idUser, int id, bool isDeleteByIdCategory)
        {
            var ids = new List<int>();

            var (idUsedCatLinkSub, idUsedFinancialRecord) = await _subcategoryRepository.ExistRelatedDataAsync(id);

            if (idUsedCatLinkSub && !idUsedFinancialRecord)
            {
                await _catLinkSubRepository.DeleteAsync(idUser, id, isDeleteByIdCategory);
                ids.AddRange(await _subcategoryRepository.DeleteAsync(id));
            }
            else if (idUsedFinancialRecord && idUsedCatLinkSub)
            {
                await _catLinkSubRepository.DeleteAsync(idUser, id, isDeleteByIdCategory);
                await _financialRecordRepository.DeleteListAsync(id, isDeleteByIdCategory);
                ids.AddRange(await _subcategoryRepository.DeleteAsync(id));
            }

            return ids;
        }
        public List<int> Delete(int idUser, int idSubcategory, bool isDeleteByIdCategory)
        {
            return Task.Run(() => DeleteAsync(idUser, idSubcategory, isDeleteByIdCategory)).Result;
        }
    }
}
