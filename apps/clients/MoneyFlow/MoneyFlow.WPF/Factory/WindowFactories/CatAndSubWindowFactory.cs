using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.WindowViewModels;
using MoneyFlow.WPF.Views.Windows;
using System.Windows;

namespace MoneyFlow.WPF.Factory.WindowFactories
{
    class CatAndSubWindowFactory : IWindowFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public CatAndSubWindowFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Window CreateWindow(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<CatAndSubWindowVM>();
            viewModel.Initialize(parameter, Enums.ParameterType.None);
            //viewModel.Update(parameter);
            var window = new CatAndSubWindow()
            {
                DataContext = viewModel,
            };

            return window;
        }
    }
}
