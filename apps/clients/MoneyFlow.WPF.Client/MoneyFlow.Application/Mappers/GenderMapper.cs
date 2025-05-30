using MoneyFlow.Application.DTOs;
using MoneyFlow.Domain.DomainModels;

namespace MoneyFlow.Application.Mappers
{
    public static class GenderMapper
    {
        public static GenderDTO ToDTO(this GenderDomain domain)
        {
            return new GenderDTO()
            {
                IdGender = domain.IdGender,
                GenderName = domain.GenderName,
            };
        }
    }
}
