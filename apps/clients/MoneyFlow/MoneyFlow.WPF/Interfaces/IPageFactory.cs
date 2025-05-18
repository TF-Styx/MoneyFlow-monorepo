using System.Windows.Controls;

namespace MoneyFlow.WPF.Interfaces
{
    internal interface IPageFactory
    {
        Page CreatePage(object parameter = null);
    }
}
