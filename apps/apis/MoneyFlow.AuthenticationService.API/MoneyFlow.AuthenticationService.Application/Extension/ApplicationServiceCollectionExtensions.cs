using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using MoneyFlow.AuthenticationService.Application.Interfaces.Realization;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;
using MoneyFlow.AuthenticationService.Application.Providers.Realization;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.AuthenticationService.Application.UseCases.Realization.GenderUseCases;
using MoneyFlow.AuthenticationService.Application.UseCases.Realization.UserUseCases;

namespace MoneyFlow.AuthenticationService.Application.Extension
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase,        RegisterUserUseCase>();
            services.AddScoped<IAuthenticateUserUseCase,    AuthenticateUserUseCase>();
            services.AddScoped<IGetUserByLoginUseCase,      GetUserByLoginUseCase>();
            services.AddScoped<IRecoveryAccessUserUseCase,  RecoveryAccessUserUseCase>();

            services.AddScoped<IGetAllStreamingGenderUseCase,    GetAllStreamingGenderUseCase>();

            services.AddSingleton<IPasswordHasher,              ArgonPasswordHasher>();

            services.AddSingleton<IDefaultErrorMessageProvider, DefaultRegistrationErrorMessageProvider>();
            services.AddSingleton<IDefaultErrorMessageProvider, DefaultAuthenticateErrorMessageProvider>();

            return services;
        }
    }
}
