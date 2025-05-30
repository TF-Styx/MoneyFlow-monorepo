using MoneyFlow.WPF.Client.Enums;

namespace MoneyFlow.WPF.Client.Services.Interfaces
{
    internal interface IUpdatable
    {
        void Update(object parameter, ParameterType typeParameter = ParameterType.None);
    }
}
