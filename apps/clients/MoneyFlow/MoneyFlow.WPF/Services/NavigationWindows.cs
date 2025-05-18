using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Services
{
    internal class NavigationWindows : INavigationWindows
    {
        private Dictionary<WindowType, Window> _windows = [];
        private readonly Dictionary<string, IWindowFactory> _windowFactories = []; // Хранит в себе методы на создание окон и VM

        public NavigationWindows(IEnumerable<IWindowFactory> windowFactories, INavigationPages navigationPages)
        {
            _windowFactories = windowFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);
        }

        public void OpenWindow(WindowType nameWindow, object parameter = null, ParameterType typeParameter = ParameterType.None)
        {
            if (_windows.TryGetValue(nameWindow, out var windowExist))  // Безопасно получает значение
            {
                if (windowExist.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update(parameter, typeParameter);
                }

                windowExist.Activate();
                return;
            }

            Open(nameWindow, parameter, typeParameter);
        }

        public void TransitObject(WindowType nameWindow, object parameter, ParameterType typeParameter = ParameterType.None)
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

        private void Open(WindowType nameWindow, object parameter = null, ParameterType typeParameter = ParameterType.None)
        {
            if (_windowFactories.TryGetValue(nameWindow.ToString(), out var factory))
            {
                var window = factory.CreateWindow(parameter);

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

        public void CloseWindow(WindowType nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                window.Close();
            }
        }

        public void MinimizeWindow(WindowType nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                SystemCommands.MinimizeWindow(window);
            }
        }

        public void MaximizeWindow(WindowType nameWindow)
        {
            if (_windows.TryGetValue(nameWindow, out var window))
            {
                SystemCommands.MaximizeWindow(window);
            }
        }

        public void RestoreWindow(WindowType nameWindow)
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
