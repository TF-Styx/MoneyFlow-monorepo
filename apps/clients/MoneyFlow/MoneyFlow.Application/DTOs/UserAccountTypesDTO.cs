using MoneyFlow.Application.DTOs.BaseDTOs;
using System.Collections.ObjectModel;

namespace MoneyFlow.Application.DTOs
{
    public class UserAccountTypesDTO : BaseDTO<UserAccountTypesDTO>
    {
        public int IdUser { get; set; }
        public ObservableCollection<AccountTypeDTO> AccountTypes { get; set; } = [];
    }
}
