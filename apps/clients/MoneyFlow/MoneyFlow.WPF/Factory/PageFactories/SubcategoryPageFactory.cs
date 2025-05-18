using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.PageViewModels;
using MoneyFlow.WPF.Views.Pages;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Factory.PageFactories
{
    internal class SubcategoryPageFactory : IPageFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public SubcategoryPageFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Page CreatePage(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<SubcategoryPageVM>();
            var page = new SubcategoryPage() { DataContext = viewModel };
            viewModel.Update(parameter);

            return page;
        }
    }
}
