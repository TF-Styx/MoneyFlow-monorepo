using MoneyFlow.WPF.Client.Enums;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Client.Services.Interfaces
{
    internal interface INavigationPage
    {
        void OpenPage(PageName pageName, FrameName frameName);
        void RegisterFrame(FrameName frameName, Frame frame);
        void TransitObject(PageName pageName, FrameName frameName, object parameter = null, ParameterType parameterType = ParameterType.None);
    }
}