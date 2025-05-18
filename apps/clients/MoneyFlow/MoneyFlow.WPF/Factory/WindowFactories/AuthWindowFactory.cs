using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.WindowViewModels;
using MoneyFlow.WPF.Views.Windows;
using System.Windows;

namespace MoneyFlow.WPF.Factory.WindowFactories
{
    internal class AuthWindowFactory : IWindowFactory
    {
        //private readonly Lazy<AuthWindowVM> _viewModel;

        //public AuthWindowFactory(Lazy<AuthWindowVM> viewModel)
        //{
        //    _viewModel = viewModel;
        //}

        //public Window CreateWindow(object parameter = null)
        //{
        //    return new AuthWindow()
        //    {
        //        DataContext = _viewModel.Value
        //    };
        //}

        /// <summary>
        /// Без Lazy<> будет цикличная зависимость
        /// </summary>
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public AuthWindowFactory(Lazy<IServiceProvider> serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Window CreateWindow(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<AuthWindowVM>();
            viewModel.Update(parameter);

            return new AuthWindow()
            {
                DataContext = viewModel,
            };
        }
    }
}
