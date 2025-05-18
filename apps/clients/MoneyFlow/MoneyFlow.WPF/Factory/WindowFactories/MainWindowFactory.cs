using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.ViewModels.WindowViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlow.WPF.Factory.WindowFactories
{
    internal class MainWindowFactory : IWindowFactory
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;
        private readonly INavigationPages _navigationPages;

        public MainWindowFactory(Lazy<IServiceProvider> serviceProvider, INavigationPages navigationPages)
        {
            _serviceProvider = serviceProvider;
            _navigationPages = navigationPages;
        }

        public Window CreateWindow(object parameter = null)
        {
            var viewModel = _serviceProvider.Value.GetRequiredService<MainWindowVM>();
            viewModel.Update(parameter);

            var mainWindow = new MainWindow() { DataContext = viewModel };
            var mainFrame = (Frame)mainWindow.FindName(FrameType.MainFrame.ToString());
            
            _navigationPages.RegisterFrame(FrameType.MainFrame, mainFrame);
            _navigationPages.OpenPage(PageType.UserPage, FrameType.MainFrame);

            return mainWindow;
        }
    }
}
