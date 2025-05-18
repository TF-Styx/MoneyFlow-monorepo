using System.Windows;

namespace MoneyFlow.WPF.Interfaces
{
    internal interface IWindowFactory
    {
        Window CreateWindow(object parameter = null);
    }
}
