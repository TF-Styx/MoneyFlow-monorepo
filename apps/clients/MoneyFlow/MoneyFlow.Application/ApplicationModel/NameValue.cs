namespace MoneyFlow.Application.ApplicationModel
{
    public class NameValue
    {
        public string Name { get; set; } = null!;
        public decimal? Value { get; set; }

        public double ConvertValue { get; set; }
    }
}
