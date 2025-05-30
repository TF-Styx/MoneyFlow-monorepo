using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyFlow.Application.InterfaceRepositories;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.Services.Realization;
using MoneyFlow.Application.UseCases.Abstraction.GenderUseCases;
using MoneyFlow.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.Application.UseCases.Realization.GenderUseCases;
using MoneyFlow.Application.UseCases.Realization.UserUseCases;
using MoneyFlow.Infrastructure.Repositories;
using MoneyFlow.WPF.Client.Enums;
using MoneyFlow.WPF.Client.Factories.Interfaces;
using MoneyFlow.WPF.Client.Factories.WindowFactories;
using MoneyFlow.WPF.Client.Services.Interfaces;
using MoneyFlow.WPF.Client.Services.NavigationPages;
using MoneyFlow.WPF.Client.Services.NavigationWindows;
using MoneyFlow.WPF.Client.ViewModels.WindowViewModels;
using System.Windows;

namespace MoneyFlow.WPF.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public IServiceProvider ServiceProvider { get; private set; } = null!;
        public static IHost ApplicationHost { get; private set; } = null!;
        private static string? _baseURL;

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceCollection services = new ServiceCollection();

            var jsonConf = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                                                     .AddJsonFile("IPhttpConf.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configurationRoot = jsonConf.Build();
            _baseURL = configurationRoot.GetValue<string>("AuthURl")!;

            ConfigureRepository(services);
            ConfigureUseCase(services);
            ConfigureService(services);
            ConfigureWindow(services);
            ConfigurePage(services);

            ServiceProvider = services.BuildServiceProvider();

            ServiceProvider.GetService<INavigationWindow>().OpenWindow(WindowName.AuthWindow);

            base.OnStartup(e);
        }

        public static void ConfigureRepository(ServiceCollection services)
        {
            string authName = "AuthAPIClient";
            services.AddHttpClient(authName, client => client.BaseAddress = new Uri(_baseURL!));

            services.AddHttpClient<IActionUserProfileRepository, ActionUserProfileRepository>(authName);
            services.AddHttpClient<IGenderRepository, GenderRepository>(authName);
        }
        public static void ConfigureUseCase(ServiceCollection services)
        {
            services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
            services.AddScoped<IRegistrationUserUseCase, RegistrationUserUseCase>();
            services.AddScoped<IRecoveryAccessUserUseCase, RecoveryAccessUserUseCase>();
            services.AddScoped<IGetAllGenderUseCase, GetAllGenderUseCase>();
        }
        public static void ConfigureService(ServiceCollection services)
        {
            services.AddSingleton<INavigationWindow, NavigationWindow>();
            services.AddSingleton<INavigationPage, NavigationPage>();

            services.AddSingleton<IAuthorizationService, AuthorizationService>();
        }
        public static void ConfigureWindow(ServiceCollection services)
        {
            services.AddTransient<IWindowFactory, AuthWindowFactory>();
            services.AddTransient<AuthWindowVM>();
            services.AddSingleton<Func<AuthWindowVM>>(x => () => x.GetRequiredService<AuthWindowVM>());

            services.AddTransient<IWindowFactory, MainWindowFactory>();
            services.AddTransient<MainWindowVM>();
            services.AddSingleton<Func<MainWindowVM>>(x => () => x.GetRequiredService<MainWindowVM>());
        }
        public static void ConfigurePage(ServiceCollection services)
        {
            
        }
    }
}
