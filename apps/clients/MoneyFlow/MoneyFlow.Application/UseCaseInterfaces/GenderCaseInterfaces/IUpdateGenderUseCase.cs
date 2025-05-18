namespace MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces
{
    public interface IUpdateGenderUseCase
    {
        Task<int> UpdateAsyncGender(int idGender, string genderName);
        int UpdateGender(int idGender, string genderName);
    }
}