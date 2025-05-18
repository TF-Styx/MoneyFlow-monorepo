using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.WindowViewModels;
using MoneyFlow.WPF.Views.Windows;
using System.Windows;

namespace MoneyFlow.WPF.Factory.WindowFactories
{
    class AccountWindowFactory : IWindowFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public AccountWindowFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Window CreateWindow(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<AccountWindowVM>();
            viewModel.Update(parameter);

            return new AccountWindow() { DataContext = viewModel };
        }
    }
}
