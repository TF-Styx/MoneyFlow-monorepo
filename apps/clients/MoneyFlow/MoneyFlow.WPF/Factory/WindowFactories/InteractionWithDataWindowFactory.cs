using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.Views.Windows;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Factory.WindowFactories
{
    internal class InteractionWithDataWindowFactory : IWindowFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;
        private readonly INavigationPages _navigationPages;

        public InteractionWithDataWindowFactory(Lazy<IServiceProvider> serviceProvider, INavigationPages navigationPages)
        {
            _serviceProvider = serviceProvider;
            _navigationPages = navigationPages;
        }

        public Window CreateWindow(object parameter = null)
        {
            var window = new InteractionWithDataWindow(_navigationPages);



            //window.Loaded += (a, b) =>
            //{
            //    var frame = (Frame)window.FindName(FrameType.InteractionWithDataFrame.ToString());
            //    _navigationPages.RegisterFrame(FrameType.InteractionWithDataFrame, frame);
            //};

            return window;
        }
    }
}
