using MoneyFlow.WPF.Enums;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Interfaces
{
    internal interface INavigationPages
    {
        void OpenPage(PageType namePage, FrameType frameName, object parameter = null, ParameterType parameterType = ParameterType.None);
        void TransitObject(PageType pageName, FrameType frameName, object parameter = null, ParameterType parameterType = ParameterType.None);
        void RegisterFrame(FrameType frameName, Frame frame);
    }
}
