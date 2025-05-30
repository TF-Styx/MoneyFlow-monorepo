using MoneyFlow.WPF.Client.Factories.Interfaces;
using MoneyFlow.WPF.Client.ViewModels.WindowViewModels;
using MoneyFlow.WPF.Client.Views.Windows;
using System.Windows;

namespace MoneyFlow.WPF.Client.Factories.WindowFactories
{
    public class AuthWindowFactory : IWindowFactory
    {
        private readonly Func<AuthWindowVM> _viewModel;

        public AuthWindowFactory(Func<AuthWindowVM> viewModel)
        {
            _viewModel = viewModel;
        }

        public Window CreateWindow(object parameter = null)
        {
            //var viewModel = _viewModel();
            //var window = new AuthWindow();

            //window.DataContext = viewModel;

            return new AuthWindow
            {
                DataContext = _viewModel(),
            };
        }
    }
}
