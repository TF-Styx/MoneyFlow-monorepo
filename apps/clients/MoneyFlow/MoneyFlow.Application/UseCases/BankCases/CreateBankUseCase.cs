using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.BankCases
{
    public class CreateBankUseCase : ICreateBankUseCase
    {
        private readonly IBanksRepository _banksRepository;

        public CreateBankUseCase(IBanksRepository banksRepository)
        {
            _banksRepository = banksRepository;
        }

        public async Task<(BankDTO BankDTO, string Message)> CreateAsync(string bankName)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(bankName))
            {
                return (null, "Вы не указали название банка!!");
            }

            var existBank = await _banksRepository.GetAsync(bankName);

            if (existBank != null)
            {
                return (null, "Банк с таким именем уже есть!!");
            }

            var idBank = await _banksRepository.CreateAsync(bankName);
            var bankDomain = await _banksRepository.GetAsync(idBank);

            return (bankDomain.ToDTO().BankDTO, message);
        }
        public (BankDTO BankDTO, string Message) Create(string bankName)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(bankName))
            {
                return (null, "Вы не указали название банка!!");
            }

            var existBank = _banksRepository.Get(bankName);

            if (existBank != null)
            {
                return (null, "Банк с таким именем уже есть!!");
            }

            var idBank = _banksRepository.Create(bankName);
            var bankDomain = _banksRepository.Get(idBank);

            return (bankDomain.ToDTO().BankDTO, message);
        }
    }
}
