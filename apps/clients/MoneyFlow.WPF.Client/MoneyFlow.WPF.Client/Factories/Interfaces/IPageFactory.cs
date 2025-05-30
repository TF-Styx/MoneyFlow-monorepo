using System.Windows.Controls;

namespace MoneyFlow.WPF.Client.Factories.Interfaces
{
    public interface IPageFactory
    {
        Page CreatePage(object parameter = null);
    }
}
