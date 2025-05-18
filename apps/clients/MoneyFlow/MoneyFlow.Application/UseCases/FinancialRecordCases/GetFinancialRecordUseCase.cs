using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.FinancialRecordCases
{
    public class GetFinancialRecordUseCase : IGetFinancialRecordUseCase
    {
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public GetFinancialRecordUseCase(IFinancialRecordRepository financialRecordRepository)
        {
            _financialRecordRepository = financialRecordRepository;
        }

        public async Task<List<FinancialRecordDTO>> GetAllAsync(int idUser)
        {
            var financialRecords = await _financialRecordRepository.GetAllAsync(idUser);
            var financialRecordsDTO = financialRecords.ToListDTO();

            return financialRecordsDTO;
        }
        public List<FinancialRecordDTO> GetAll(int idUser)
        {
            var financialRecords = _financialRecordRepository.GetAll(idUser);
            var financialRecordsDTO = financialRecords.ToListDTO();

            return financialRecordsDTO;
        }

        public async Task<FinancialRecordDTO> GetAsync(int idFinancialRecord)
        {
            var financialRecord = await _financialRecordRepository.GetAsync(idFinancialRecord);

            if (financialRecord == null) { return null; }

            var financialRecordDTO = financialRecord.ToDTO();

            return financialRecordDTO.FinancialRecordDTO;
        }
        public FinancialRecordDTO Get(int idFinancialRecord)
        {
            var financialRecord = _financialRecordRepository.Get(idFinancialRecord);

            if (financialRecord == null) { return null; }

            var financialRecordDTO = financialRecord.ToDTO();

            return financialRecordDTO.FinancialRecordDTO;
        }

        //public async Task<FinancialRecordDTO> GetAsyncFinancialRecord(string recordName)
        //{
        //    var financialRecord = await _financialRecordRepository.GetAsync(recordName);

        //    if (financialRecord == null) { return null; }

        //    var financialRecordDTO = financialRecord.ToDTO();

        //    return financialRecordDTO.FinancialRecordDTO;
        //}
        //public FinancialRecordDTO GetFinancialRecord(string recordName)
        //{
        //    var financialRecord = _financialRecordRepository.Get(recordName);

        //    if (financialRecord == null) { return null; }

        //    var financialRecordDTO = financialRecord.ToDTO();

        //    return financialRecordDTO.FinancialRecordDTO;
        //}
    }
}
