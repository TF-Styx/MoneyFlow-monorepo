using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.PageViewModels;
using MoneyFlow.WPF.Views.Pages;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Factory.PageFactories
{
    internal class BankPageFactory : IPageFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public BankPageFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Page CreatePage(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<BankPageVM>();
            viewModel.Update(parameter);

            return new BankPage() { DataContext = viewModel, };
        }
    }
}
