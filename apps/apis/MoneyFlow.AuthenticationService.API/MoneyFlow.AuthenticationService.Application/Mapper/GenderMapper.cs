using MoneyFlow.AuthenticationService.Application.DTOs;
using MoneyFlow.AuthenticationService.Domain.DomainModels;

namespace MoneyFlow.AuthenticationService.Application.Mapper
{
    public static class GenderMapper
    {
        public static GenderDTO ToDTO(this GenderDomain genderDomain)
        {
            return new GenderDTO
            {
                IdGender = genderDomain.IdGender,
                GenderName = genderDomain.GenderName,
            };
        }
    }
}
