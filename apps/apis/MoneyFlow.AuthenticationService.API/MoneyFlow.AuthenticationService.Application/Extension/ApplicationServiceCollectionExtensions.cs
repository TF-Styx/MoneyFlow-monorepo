using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.AuthenticationService.Application.Interfaces.Abstraction;
using MoneyFlow.AuthenticationService.Application.Interfaces.Realization;
using MoneyFlow.AuthenticationService.Application.Providers.Abstraction;
using MoneyFlow.AuthenticationService.Application.Providers.Realization;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.GenderUseCases;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.RoleUseCases;
using MoneyFlow.AuthenticationService.Application.UseCases.Abstraction.UserUseCases;
using MoneyFlow.AuthenticationService.Application.UseCases.Realization.GenderUseCases;
using MoneyFlow.AuthenticationService.Application.UseCases.Realization.RoleUseCases;
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
            //services.AddTransient<IGetAllStreamingUserUseCase,  GetAllStreamingUserUseCase>();
            //services.AddTransient<IGetByIdUserUseCase,          GetByIdUserUseCase>();
            //services.AddTransient<IUpdateUserUseCase,           UpdateUserUseCase>();
            //services.AddTransient<IDeleteUserUseCase,           DeleteUserUseCase>();

            services.AddScoped<ICreateGenderUseCase,             CreateGenderUseCase>();
            services.AddScoped<IGetAllStreamingGenderUseCase,    GetAllStreamingGenderUseCase>();
            services.AddScoped<IUpdateGenderUseCase,             UpdateGenderUseCase>();
            services.AddScoped<IGetByIdGenderUseCase,            GetByIdGenderUseCase>();
            services.AddScoped<IDeleteGenderUseCase,             DeleteGenderUseCase>();

            services.AddScoped<ICreateRoleUseCase,           CreateRoleUseCase>();
            services.AddScoped<IGetAllStreamingRoleUseCase,  GetAllStreamingRoleUseCase>();
            services.AddScoped<IUpdateRoleUseCase,           UpdateRoleUseCase>();
            services.AddScoped<IGetByIdRoleUseCase,          GetByIdRoleUseCase>();
            services.AddScoped<IDeleteRoleUseCase,           DeleteRoleUseCase>();

            services.AddSingleton<IPasswordHasher,              ArgonPasswordHasher>();

            services.AddSingleton<IDefaultErrorMessageProvider, DefaultRegistrationErrorMessageProvider>();
            services.AddSingleton<IDefaultErrorMessageProvider, DefaultAuthenticateErrorMessageProvider>();

            return services;
        }
    }
}
