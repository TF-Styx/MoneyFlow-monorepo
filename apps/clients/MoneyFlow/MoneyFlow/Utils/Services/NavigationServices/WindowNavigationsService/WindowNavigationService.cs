using MoneyFlow.MVVM.View.Windows;
using MoneyFlow.MVVM.ViewModels.WindowVM;
using System.Windows;

namespace MoneyFlow.Utils.Services.NavigationServices.WindowNavigationsService
{
    public class WindowNavigationService(IServiceProvider serviceProvider) : IWindowNavigationService
    {
        // Создан словарь с парой ключ - значение.
        // string(ключ) = имя страницы, window(значение) = объект окна.
        // Используется для проверки, открыто ли окно.
        private Dictionary<string, Window> _windows = [];
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        // Метод для открытия окна
        public void NavigateTo(string nameWin, object parameter = null)
        {
            // Проверяем есть ли окно в словаре
            if (_windows.TryGetValue(nameWin, out var win))
            {
                // Если окно открыто, то извлекаем ViewModel из свойства DataContext
                if (win.DataContext is IUpdatable viewModel)
                {
                    // Выводим окно на передний план
                    win.Activate();
                    // Передаем значение для обновления данных во ViewModel
                    viewModel.Update(parameter);

                    return;
                }
            }
            // Если окно не открыто, то создаем и открываем его
            OpenWindow(nameWin, parameter);
        }

        // Тоже самое что и NavigateTo, но без открытия окна
        public void RefreshData(string nameWin, object parameter = null)
        {
            if (_windows.TryGetValue(nameWin, out var win))
            {
                if (win.DataContext is IUpdatable viewModel)
                {
                    win.Activate();
                    viewModel.Update(parameter);

                    return;
                }
            }
        }

        // Определяем какое окно нужно открыть
        private void OpenWindow(string nameWin, object parameter)
        {
            Action action = nameWin switch
            {
                "AuthWnd" => () =>
                {
                    AuthWndVM vM = new AuthWndVM(_serviceProvider);
                    AuthWnd authWnd = new AuthWnd { DataContext = vM };

                    _windows.TryAdd(nameWin, authWnd);

                    vM.Update(parameter);

                    authWnd.Closed += (c, e) => _windows.Remove(nameWin);
                    authWnd.Show();
                },

                "FinancialRecordAdd" => () =>
                {
                    FinancialRecordAddVM vM = new FinancialRecordAddVM(_serviceProvider);
                    FinancialRecordAddWindow wnd = new FinancialRecordAddWindow { DataContext = vM };

                    _windows.TryAdd(nameWin, wnd);

                    vM.Update(parameter);

                    wnd.Closed += (c, e) => _windows.Remove(nameWin);
                    wnd.Show();
                },

                "MainWindow" => () =>
                {
                    MainWindowVM vM = new MainWindowVM(_serviceProvider);
                    MainWindow wnd = new MainWindow(_serviceProvider) { DataContext = vM };

                    _windows.TryAdd(nameWin, wnd);

                    vM.Update(parameter);

                    wnd.Closed += (c, e) => _windows.Remove(nameWin);
                    wnd.Show();
                }
                ,

                _ => () =>
                {

                }
            };
            action?.Invoke();
        }

        public void CloseWindow(string nameWin)
        {
            if (_windows.TryGetValue(nameWin, out var win))
            {
                win.Close();
            }
        }
    }
}
