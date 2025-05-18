using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordViewingInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.FinancialRecordViewingCases
{
    public class GetFinancialRecordViewingUseCase : IGetFinancialRecordViewingUseCase
    {
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public GetFinancialRecordViewingUseCase(IFinancialRecordRepository financialRecordRepository)
        {
            _financialRecordRepository = financialRecordRepository;
        }

        public async Task<List<FinancialRecordViewingDTO>> GetAllViewingAsync(int idUser, FinancialRecordFilterDTO filter)
        {
            var financialRecords = await _financialRecordRepository.GetAllViewingAsync(idUser, filter.ToDomain());
            var financialRecordsDTO = financialRecords.ToListDTO();

            return financialRecordsDTO;
        }
        public List<FinancialRecordViewingDTO> GetAllViewing(int idUser, FinancialRecordFilterDTO filter)
        {
            var financialRecords = _financialRecordRepository.GetAllViewing(idUser, filter.ToDomain());
            var financialRecordsDTO = financialRecords.ToListDTO();

            return financialRecordsDTO;
        }

        public async Task<FinancialRecordViewingDTO> GetViewingAsync(int idFinancialRecord)
        {
            var financialRecord = await _financialRecordRepository.GetViewingAsync(idFinancialRecord);
            var financialRecordDTO = financialRecord.ToDTO().FinancialRecordViewingDTO;

            return financialRecordDTO;
        }
        public FinancialRecordViewingDTO GetViewing(int idFinancialRecord)
        {
            return Task.Run(() => GetViewingAsync(idFinancialRecord)).Result;
        }

        public async Task<FinancialRecordViewingDTO> GetByIdAsync(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory)
        {
            var financialRecord = await _financialRecordRepository.GetByIdAsync(idUser, idFinancialRecord, idCategory, idSubcategory);
            var financialRecordDTO = financialRecord.ToDTO().FinancialRecordViewingDTO;

            return financialRecordDTO;
        }
        public FinancialRecordViewingDTO GetById(int idUser, int idFinancialRecord, int? idCategory, int? idSubcategory)
        {
            var financialRecord = _financialRecordRepository.GetById(idUser, idFinancialRecord, idCategory, idSubcategory);
            var financialRecordDTO = financialRecord.ToDTO().FinancialRecordViewingDTO;

            return financialRecordDTO;
        }
    }
}
