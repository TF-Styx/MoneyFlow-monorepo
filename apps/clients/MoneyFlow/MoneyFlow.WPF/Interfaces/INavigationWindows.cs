using MoneyFlow.WPF.Enums;

namespace MoneyFlow.WPF.Interfaces
{
    internal interface INavigationWindows
    {
        void OpenWindow(WindowType nameWindow, object parameter = null, ParameterType typeParameter = ParameterType.None);

        void TransitObject(WindowType nameWindow, object parameter, ParameterType typeParameter = ParameterType.None);

        void CloseWindow(WindowType nameWindow);

        void MinimizeWindow(WindowType nameWindow);

        void MaximizeWindow(WindowType nameWindow);

        void RestoreWindow(WindowType nameWindow);

        void Shutdown();
    }
}
