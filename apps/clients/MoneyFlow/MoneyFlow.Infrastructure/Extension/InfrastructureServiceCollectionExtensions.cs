using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Domain.Interfaces.Repositories;
using MoneyFlow.Infrastructure.Context;
using MoneyFlow.Infrastructure.Repositories;

namespace MoneyFlow.Infrastructure.Extension
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ContextMF>();
            services.AddSingleton<Func<ContextMF>>(provider => () => provider.GetRequiredService<ContextMF>());


            services.AddScoped<IAccountRepository,          AccountRepository>();
            services.AddScoped<IAccountTypeRepository,      AccountTypeRepository>();
            services.AddScoped<IBanksRepository,            BanksRepository>();
            services.AddScoped<ICategoryRepository,         CategoryRepository>();
            services.AddScoped<ICatLinkSubRepository,       CatLinkSubRepository>();
            services.AddScoped<IFinancialRecordRepository,  FinancialRecordRepository>();
            services.AddScoped<IGendersRepository,          GendersRepository>();
            services.AddScoped<ISubcategoryRepository,      SubcategoryRepository>();
            services.AddScoped<ITransactionTypeRepository,  TransactionTypeRepository>();
            services.AddScoped<IUsersRepository,            UsersRepository>();

            return services;
        }
    }
}
