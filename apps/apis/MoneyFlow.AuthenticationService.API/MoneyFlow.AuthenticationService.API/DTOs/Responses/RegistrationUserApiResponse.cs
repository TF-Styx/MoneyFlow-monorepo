namespace MoneyFlow.AuthenticationService.API.DTOs.Responses
{
    public class RegistrationUserApiResponse
    {
        public int IdUser { get; set; } // Может быть другое имя свойства, чем в Application DTO
        public string DisplayLogin { get; set; }
        public string FullName { get; set; }
        public string ContactEmail { get; set; }
        public string RegistrationDateString { get; set; } // API может захотеть отдать дату как строку
    }
}