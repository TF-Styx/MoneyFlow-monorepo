using MoneyFlow.WPF.Client.Factories.Interfaces;
using MoneyFlow.WPF.Client.ViewModels.WindowViewModels;
using System.Windows;

namespace MoneyFlow.WPF.Client.Factories.WindowFactories
{
    public class MainWindowFactory : IWindowFactory
    {
        private readonly Func<MainWindowVM> _viewModel;

        public MainWindowFactory(Func<MainWindowVM> viewModel)
        {
            _viewModel = viewModel;
        }

        public Window CreateWindow(object parameter = null)
        {
            var viewModel = _viewModel();
            var window = new MainWindow();

            window.DataContext = viewModel;

            return window;
        }
    }
}
