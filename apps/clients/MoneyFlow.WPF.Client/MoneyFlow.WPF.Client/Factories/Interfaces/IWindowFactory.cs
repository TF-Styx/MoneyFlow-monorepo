using System.Windows;

namespace MoneyFlow.WPF.Client.Factories.Interfaces
{
    public interface IWindowFactory
    {
        Window CreateWindow(object parameter = null);
    }
}
