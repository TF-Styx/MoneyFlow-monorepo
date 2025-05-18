using MoneyFlow.WPF.Enums;

namespace MoneyFlow.WPF.Interfaces
{
    internal interface IUpdatable
    {
        void Update(object parameter, ParameterType typeParameter = ParameterType.None);
    }
}
