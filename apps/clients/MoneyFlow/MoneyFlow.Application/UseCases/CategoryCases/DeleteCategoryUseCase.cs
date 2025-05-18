using MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CategoryCases
{
    public class DeleteCategoryUseCase : IDeleteCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICatLinkSubRepository _catLinkSubRepository;
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public DeleteCategoryUseCase(ICategoryRepository categoryRepository, ICatLinkSubRepository catLinkSubRepository, IFinancialRecordRepository financialRecordRepository)
        {
            _categoryRepository = categoryRepository;
            _catLinkSubRepository = catLinkSubRepository;
            _financialRecordRepository = financialRecordRepository;
        }

        public async Task<string?> ExistRelatedDataAsync(int idCategory)
        {
            string? messageFinancialRecord = null;

            var (idUsedCatLinkSub, idUsedFinancialRecord) = await _categoryRepository.ExistRelatedDataAsync(idCategory);

            if (idUsedFinancialRecord)
            {
                messageFinancialRecord = "У вас есть связанные данные!!\nХотите их удалить?";
            }

            return messageFinancialRecord;
        }

        public async Task<int?> DeleteAsync(int idUser, int id, bool isDeleteByIdCategory)
        {
            int? idReturn = null;

            var (idUsedCatLinkSub, idUsedFinancialRecord) = await _categoryRepository.ExistRelatedDataAsync(id);

            if (idUsedCatLinkSub && !idUsedFinancialRecord)
            {
                await _catLinkSubRepository.DeleteAsync(idUser, id, isDeleteByIdCategory);
                idReturn = await _categoryRepository.DeleteAsync(id);
            }
            if (idUsedFinancialRecord && idUsedCatLinkSub)
            {
                await _catLinkSubRepository.DeleteAsync(idUser, id, isDeleteByIdCategory);
                await _financialRecordRepository.DeleteListAsync(id, isDeleteByIdCategory);
                idReturn = await _categoryRepository.DeleteAsync(id);
            }
            if (!idUsedCatLinkSub && !idUsedFinancialRecord)
            {
                idReturn = await _categoryRepository.DeleteAsync(id);
            }

            return idReturn;
        }
        public int? Delete(int idUser, int idCategory, bool isDeleteByIdCategory)
        {
            return Task.Run(() => DeleteAsync(idUser, idCategory, isDeleteByIdCategory)).Result;
        }
    }
}
