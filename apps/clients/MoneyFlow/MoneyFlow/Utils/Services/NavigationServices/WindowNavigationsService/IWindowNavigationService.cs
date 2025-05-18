namespace MoneyFlow.Utils.Services.NavigationServices.WindowNavigationsService
{
    public interface IWindowNavigationService
    {
        void NavigateTo(string nameWin, object parameter = null);
        void RefreshData(string nameWin, object parameter = null);
        void CloseWindow(string nameWin);
    }
}
