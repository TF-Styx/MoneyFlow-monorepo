using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.MVVM.Models.DB_MSSQL;
using MoneyFlow.Utils.Helpers;
using MoneyFlow.Utils.Services.AuthorizationVerificationServices;
using MoneyFlow.Utils.Services.DataBaseServices;
using MoneyFlow.Utils.Services.DialogServices.OpenFileDialogServices;
using MoneyFlow.Utils.Services.NavigationServices.PageNavigationsService;
using MoneyFlow.Utils.Services.NavigationServices.WindowNavigationsService;
using System.Windows;

namespace MoneyFlow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Свойство хранения сервисов
        public IServiceProvider ServiceProvider { get; private set; }

        // Перегруженные метод запуска приложения
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Создали коллекцию сервисов
            var services = new ServiceCollection();
            // Добавили сервисы в коллекцию
            ConfigureServices(services);
            // Сконфигурировали ServiceProvider
            ServiceProvider = services.BuildServiceProvider();

            var authorizationService = ServiceProvider.GetService<IAuthorizationVerificationService>();
            var dataBaseService = ServiceProvider.GetService<IDataBaseService>();
            var windowNavigationService = ServiceProvider.GetService<IWindowNavigationService>();

            if (authorizationService.CheckAuthorization())
            {
                if (await dataBaseService.ExistsAsync<User>(x => x.Login.ToLower() == authorizationService.CurrentUser.Login.ToLower()))
                {
                    windowNavigationService.NavigateTo("MainWindow", authorizationService.CurrentUser);
                }
                else
                {
                    windowNavigationService.NavigateTo("AuthWnd");
                }
            }
            else
            {
                windowNavigationService.NavigateTo("AuthWnd");
            }

            // Остальная реализация взята из оригинального класса
            base.OnStartup(e);
        }
        
        // Добавляем сервисы в коллекцию сервисов 
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IWindowNavigationService, WindowNavigationService>();
            services.AddSingleton<IPageNavigationService, PageNavigationService>();
            services.AddSingleton<IAuthorizationVerificationService, AuthorizationVerificationService>();
            services.AddSingleton<IOpenFileDialogService, OpenFileDialogService>();

            services.AddTransient<MoneyFlowDbContext>();
            services.AddSingleton<Func<MoneyFlowDbContext>>(provider => () => provider.GetRequiredService<MoneyFlowDbContext>());
            services.AddSingleton<IDataBaseService, DataBaseService>();
            services.AddSingleton<LastRecordHelper>();
        }
    }
}
