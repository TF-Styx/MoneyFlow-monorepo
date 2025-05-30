using MoneyFlow.WPF.Client.Enums;

namespace MoneyFlow.WPF.Client.Services.Interfaces
{
    public interface INavigationWindow
    {
        void OpenWindow(WindowName nameWindow);
        void TransitObject(WindowName nameWindow, object parameter, ParameterType typeParameter = ParameterType.None);
        void CloseWindow(WindowName nameWindow);
        void MaximizeWindow(WindowName nameWindow);
        void MinimizeWindow(WindowName nameWindow);
        void RestoreWindow(WindowName nameWindow);
        void Shutdown();
    }
}