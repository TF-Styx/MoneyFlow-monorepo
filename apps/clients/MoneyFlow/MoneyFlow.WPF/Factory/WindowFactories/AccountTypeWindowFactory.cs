using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.WindowViewModels;
using MoneyFlow.WPF.Views.Windows;
using System.Windows;

namespace MoneyFlow.WPF.Factory.WindowFactories
{
    class AccountTypeWindowFactory : IWindowFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public AccountTypeWindowFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Window CreateWindow(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<AccountTypeWindowVM>();
            viewModel.Update(parameter);

            return new AccountTypeWindow() { DataContext = viewModel};
        }
    }
}
