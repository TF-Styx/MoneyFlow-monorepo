using MoneyFlow.Application.DTOs.BaseDTOs;
using System.Collections.ObjectModel;

namespace MoneyFlow.Application.DTOs
{
    public class UserBanksDTO : BaseDTO<UserBanksDTO>
    {
        public int IdUser { get; set; }
        public ObservableCollection<BankDTO> Banks { get; set; } = [];
    }
}
