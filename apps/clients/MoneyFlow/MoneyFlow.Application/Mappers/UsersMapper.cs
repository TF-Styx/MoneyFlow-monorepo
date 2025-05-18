using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    internal static class UsersMapper
    {
        public static (UserDTO UserDTO, string Message) ToDTO(this UserDomain user)
        {
            string message = string.Empty;

            if (user == null)
            {
                return (null, "Данного пользователя нет!!");
            }
            
            var dto = new UserDTO()
            {
                IdUser = user.IdUser,
                UserName = user.UserName,
                Avatar = user.Avatar,
                Login = user.Login,
                Password = user.Password,
                IdGender = user.IdGender,
            };

            return (dto, message);
        }

        public static (UserTotalInfoDTO UserTotalInfoDTO, string Message) ToDTO(this UserTotalInfoDomain userTotalInfo)
        {
            string message = string.Empty;

            if (userTotalInfo == null) { return (null, "Нет данных!!"); }

            var dto = new UserTotalInfoDTO()
            {
                GenderName = userTotalInfo.GenderName,
                TotalBalance = userTotalInfo.TotalBalance,
                AccountCount = userTotalInfo.AccountCount,
                AccountTypeCount = userTotalInfo.AccountTypeCount,
                BankCount = userTotalInfo.BankCount,
                CategoryCount = userTotalInfo.CategoryCount,
                SubcategoryCount = userTotalInfo.SubcategoryCount,
                FinancialRecordCount = userTotalInfo.FinancialRecordCount,
            };

            return (dto, message);
        }

        public static List<UserDTO> ToListDTO(this IEnumerable<UserDomain> users)
        {
            var list = new List<UserDTO>();

            foreach (var item in users)
            {
                list.Add(item.ToDTO().UserDTO);
            }
            return list;
        }
    }
}
