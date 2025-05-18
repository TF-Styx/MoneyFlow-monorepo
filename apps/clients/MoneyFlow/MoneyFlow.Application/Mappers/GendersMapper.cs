using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    internal static class GendersMapper
    {
        public static (GenderDTO GenderDTO, string Message) ToDTO(this GenderDomain gender)
        {
            string message = string.Empty;

            if (gender == null)
            {
                return (null, "Пол ноль");
            }

            var dto = new GenderDTO()
            {
                IdGender = gender.IdGender,
                GenderName = gender.GenderName
            };

            return (dto, message);
        }

        public static List<GenderDTO> ToListDTO(this IEnumerable<GenderDomain> genders)
        {
            var list = new List<GenderDTO>();

            foreach (var item in genders)
            {
                list.Add(item.ToDTO().GenderDTO);
            }
            return list;
        }
    }
}
