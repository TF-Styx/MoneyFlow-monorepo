using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Infrastructure.Repositories;

namespace MoneyFlow.AuthenticationService.Infrastructure.Extension
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository,     UserRepository>();
            services.AddScoped<IGenderRepository,   GenderRepository>();
            services.AddScoped<IRoleRepository,     RoleRepository>();

            return services;
        }
    }
}
