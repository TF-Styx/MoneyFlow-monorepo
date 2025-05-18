using Azure;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.PageViewModels;
using MoneyFlow.WPF.Views.Pages;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Factory.PageFactories
{
    internal class UserPageFactory : IPageFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public UserPageFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Page CreatePage(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<UserPageVM>();
            var page = new UserPage() { DataContext = viewModel, };
            page.Loaded += (sender, args) =>
            {
                viewModel.Update(parameter);
            };

            return page;
        }
    }
}
