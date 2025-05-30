using MoneyFlow.WPF.Client.Enums;
using System.Windows.Controls;
using System.Windows;
using MoneyFlow.WPF.Client.Factories.Interfaces;
using MoneyFlow.WPF.Client.Services.Interfaces;

namespace MoneyFlow.WPF.Client.Services.NavigationWindows
{
    public class NavigationWindow : INavigationWindow
    {
        private Dictionary<WindowName, Window> _windows = [];
        private readonly Dictionary<string, IWindowFactory> _windowFactories = []; // Хранит в себе методы на создание окон и VM

        public NavigationWindow(IEnumerable<IWindowFactory> windowFactories)
        {
            _windowFactories = windowFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);
        }

        public void OpenWindow(WindowName nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var windowExist))  // Безопасно получает значение
            {
                windowExist.Activate();
                return;
            }
            Open(nameWindow);
        }

        public void TransitObject(WindowName nameWindow, object parameter, ParameterType typeParameter = ParameterType.None)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                if (window.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update(parameter, typeParameter);
                    window.Activate();
                }
            }
        }

        private void Open(WindowName nameWindow)
        {
            if (_windowFactories.TryGetValue(nameWindow.ToString(), out var factory))
            {
                var window = factory.CreateWindow();

                _windows[nameWindow] = window;

                window.Closed += (c, e) => _windows.Remove(nameWindow);
                window.StateChanged += MainWindowStateChangeRaised;

                window.Show();
            }
            else
            {
                throw new Exception($"Такое окно не зарегистрировано {nameWindow}");
            }
        }

        public void CloseWindow(WindowName nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                window.Close();
            }
        }

        public void MinimizeWindow(WindowName nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                SystemCommands.MinimizeWindow(window);
            }
        }

        public void MaximizeWindow(WindowName nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                SystemCommands.MaximizeWindow(window);
            }
        }

        public void RestoreWindow(WindowName nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                SystemCommands.RestoreWindow(window);
            }
        }

        public void Shutdown()
        {
            App.Current.Shutdown();
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                var mainWindowBorder = (Border)window.FindName("MainWindowBorder");
                var restoreButton = (Button)window.FindName("RestoreButton");
                var maximizeButton = (Button)window.FindName("MaximizeButton");

                if (window.WindowState == WindowState.Maximized)
                {
                    mainWindowBorder.BorderThickness = new Thickness(8);
                    restoreButton.Visibility = Visibility.Visible;
                    maximizeButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mainWindowBorder.BorderThickness = new Thickness(0);
                    restoreButton.Visibility = Visibility.Collapsed;
                    maximizeButton.Visibility = Visibility.Visible;
                }
            }
        }

    }
}
