using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Application.Extension;
using MoneyFlow.Infrastructure.Extension;
using MoneyFlow.WPF.Enums;
using MoneyFlow.WPF.Factory.PageFactories;
using MoneyFlow.WPF.Factory.WindowFactories;
using MoneyFlow.WPF.Interfaces;
using MoneyFlow.WPF.Services;
using MoneyFlow.WPF.ViewModels.PageViewModels;
using MoneyFlow.WPF.ViewModels.WindowViewModels;
using MoneyFlow.WPF.Views.Pages;
using System.Windows;

namespace MoneyFlow.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        // Свойство хранения сервисов
        public IServiceProvider ServiceProvider { get; private set; }

        // Перегруженные метод запуска приложения
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Создали коллекцию сервисов
            var services = new ServiceCollection();

            // Добавили в коллекцию
            services.AddInfrastructureServices();
            services.AddApplicationServices();

            ConfigureServices(services);
            ConfigureWindow(services);
            ConfigurePage(services);

            // Сконфигурировали ServiceProvider
            ServiceProvider = services.BuildServiceProvider();

            var navigationWindows = ServiceProvider.GetService<INavigationWindows>();
            navigationWindows.OpenWindow(WindowType.AuthWindow);

            // Пример дефолтной загрузки фрейма 
            //var navigationPage = ServiceProvider.GetService<INavigationPages>();
            //navigationPage.OpenPage(PageType.UserPage);

            // Остальная реализация взята из оригинального класса
            base.OnStartup(e);
        }

        // Добавляем окна и их VM в коллекцию сервисов 
        private void ConfigureWindow(ServiceCollection services)
        {
            services.AddTransient<IWindowFactory, AuthWindowFactory>();
            services.AddTransient<IWindowFactory, MainWindowFactory>();

            services.AddTransient<IWindowFactory, AccountWindowFactory>();
            services.AddTransient<IWindowFactory, AccountTypeWindowFactory>();
            services.AddTransient<IWindowFactory, BankWindowFactory>();
            services.AddTransient<IWindowFactory, CatAndSubWindowFactory>();
            services.AddTransient<IWindowFactory, FinancialRecordWindowFactory>();

            services.AddTransient<IWindowFactory, AddBaseInformationWindowFactory>();
            services.AddTransient<IWindowFactory, InteractionWithDataWindowFactory>();

            services.AddTransient<IPageFactory, AccountPageFactory>();
            services.AddTransient<IPageFactory, AccountTypePageFactory>();
            services.AddTransient<IPageFactory, BankPageFactory>();
            services.AddTransient<IPageFactory, CatAndSubPageFactory>();
            services.AddTransient<IPageFactory, CategoryPageFactory>();
            services.AddTransient<IPageFactory, SubcategoryPageFactory>();
            services.AddTransient<IPageFactory, FinancialRecordPageFactory>();
            services.AddTransient<IPageFactory, UserPageFactory>();

            services.AddTransient(typeof(Lazy<>), typeof(LazyService<>));

            services.AddTransient<AuthWindowVM>();
            services.AddTransient<MainWindowVM>();

            services.AddTransient<AccountWindowVM>();
            services.AddTransient<AccountTypeWindowVM>();
            services.AddTransient<BankWindowVM>();
            services.AddTransient<CatAndSubWindowVM>();
            services.AddTransient<FinancialRecordWindowVM>();
            services.AddTransient<AddBaseInformationVM>();
        }

        // Добавляем страницы и их VM в коллекцию сервисов 
        private void ConfigurePage(ServiceCollection services)
        {
            services.AddSingleton<AccountPage>();
            services.AddSingleton<AccountPageVM>();

            services.AddSingleton<AccountTypePage>();
            services.AddSingleton<AccountTypePageVM>();

            services.AddSingleton<BankPage>();
            services.AddSingleton<BankPageVM>();

            services.AddSingleton<CatAndSubPage>();
            services.AddSingleton<CatAndSubPageVM>();

            services.AddSingleton<CategoryPage>();
            services.AddSingleton<CategoryPageVM>();

            services.AddSingleton<SubcategoryPage>();
            services.AddSingleton<SubcategoryPageVM>();

            services.AddSingleton<FinancialRecordPage>();
            services.AddSingleton<FinancialRecordPageVM>();

            services.AddSingleton<GenderPage>();
            services.AddSingleton<GenderPageVM>();

            services.AddSingleton<UserPage>();
            services.AddSingleton<UserPageVM>();
        }

        // Добавляем страницы и их VM в коллекцию сервисов 
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<INavigationPages, NavigationPages>();
            services.AddSingleton<INavigationWindows, NavigationWindows>();
        }
    }
}
