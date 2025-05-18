using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Application.Services.Abstraction;
using MoneyFlow.Application.Services.Realization;
using MoneyFlow.Application.UseCaseInterfaces.AccountCaseInterface;
using MoneyFlow.Application.UseCaseInterfaces.AccountTypeCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.BankCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.CatLinkSubCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.FinancialRecordViewingInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.GenderCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.SubcategoryCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.TransactionTypeCaseInterfaces;
using MoneyFlow.Application.UseCaseInterfaces.UserCaseInterfaces;
using MoneyFlow.Application.UseCases.AccountCases;
using MoneyFlow.Application.UseCases.AccountTypeCases;
using MoneyFlow.Application.UseCases.BankCases;
using MoneyFlow.Application.UseCases.CategoryCases;
using MoneyFlow.Application.UseCases.CatLinkSubCases;
using MoneyFlow.Application.UseCases.FinancialRecordCases;
using MoneyFlow.Application.UseCases.FinancialRecordViewingCases;
using MoneyFlow.Application.UseCases.GenderCases;
using MoneyFlow.Application.UseCases.SubcategoryCases;
using MoneyFlow.Application.UseCases.TransactionTypeCases;
using MoneyFlow.Application.UseCases.UserCases;

namespace MoneyFlow.Application.Extension
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateAccountUseCase,   CreateAccountUseCase>();
            services.AddScoped<IDeleteAccountUseCase,   DeleteAccountUseCase>();
            services.AddScoped<IGetAccountUseCase,      GetAccountUseCase>();
            services.AddScoped<IUpdateAccountUseCase,   UpdateAccountUseCase>();

            services.AddScoped<ICreateAccountTypeUseCase,   CreateAccountTypeUseCase>();
            services.AddScoped<IDeleteAccountTypeUseCase,   DeleteAccountTypeUseCase>();
            services.AddScoped<IGetAccountTypeUseCase,      GetAccountTypeUseCase>();
            services.AddScoped<IUpdateAccountTypeUseCase,   UpdateAccountTypeUseCase>();

            services.AddScoped<ICreateBankUseCase, CreateBankUseCase>();
            services.AddScoped<IDeleteBankUseCase, DeleteBankUseCase>();
            services.AddScoped<IGetBankUseCase,    GetBankUseCase>();
            services.AddScoped<IUpdateBankUseCase, UpdateBankUseCase>();

            services.AddScoped<ICreateCatLinkSubUseCase, CreateCatLinkSubUseCase>();
            services.AddScoped<IDeleteCatLinkSubUseCase, DeleteCatLinkSubUseCase>();
            services.AddScoped<IGetCatLinkSubUseCase,    GetCatLinkSubUseCase>();

            services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
            services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
            services.AddScoped<IGetCategoryUseCase,    GetCategoryUseCase>();
            services.AddScoped<IGetCategoryWithSubcategoriesUseCase, GetCategoryWithSubcategoriesUseCase>();
            services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();

            services.AddScoped<ICreateFinancialRecordUseCase, CreateFinancialRecordUseCase>();
            services.AddScoped<IDeleteFinancialRecordUseCase, DeleteFinancialRecordUseCase>();
            services.AddScoped<IGetFinancialRecordUseCase,    GetFinancialRecordUseCase>();
            services.AddScoped<IUpdateFinancialRecordUseCase, UpdateFinancialRecordUseCase>();

            services.AddScoped<IGetFinancialRecordViewingUseCase, GetFinancialRecordViewingUseCase>();

            services.AddScoped<ICreateGenderUseCase, CreateGenderUseCase>();
            services.AddScoped<IDeleteGenderUseCase, DeleteGenderUseCase>();
            services.AddScoped<IGetGenderUseCase,    GetGenderUseCase>();
            services.AddScoped<IUpdateGenderUseCase, UpdateGenderUseCase>();

            services.AddScoped<ICreateSubcategoryUseCase, CreateSubcategoryUseCase>();
            services.AddScoped<IDeleteSubcategoryUseCase, DeleteSubcategoryUseCase>();
            services.AddScoped<IGetSubcategoryUseCase,    GetSubcategoryUseCase>();
            services.AddScoped<IUpdateSubcategoryUseCase, UpdateSubcategoryUseCase>();

            services.AddScoped<ICreateTransactionTypeUseCase, CreateTransactionTypeUseCase>();
            services.AddScoped<IDeleteTransactionTypeUseCase, DeleteTransactionTypeUseCase>();
            services.AddScoped<IGetTransactionTypeUseCase,    GetTransactionTypeUseCase>();
            services.AddScoped<IUpdateTransactionTypeUseCase, UpdateTransactionTypeUseCase>();

            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
            services.AddScoped<IGetUserUseCase,    GetUserUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();

            services.AddSingleton<IAuthorizationService,    AuthorizationService>();
            services.AddSingleton<IRegistrationService,     RegistrationService>();
            services.AddSingleton<IRecoveryService,         RecoveryService>();
            services.AddSingleton<IAccountService,          AccountService>();
            services.AddSingleton<IAccountTypeService,      AccountTypeService>();
            services.AddSingleton<IBankService,             BankService>();
            services.AddSingleton<ICategoryService,         CategoryService>();
            services.AddSingleton<IStatisticsService,       StatisticsService>();
            services.AddSingleton<ISubcategoryService,      SubcategoryService>();
            services.AddSingleton<IFinancialRecordService,  FinancialRecordService>();
            services.AddSingleton<IGenderService,           GenderService>();
            services.AddSingleton<ITransactionTypeService,  TransactionTypeService>();
            services.AddSingleton<IUserService,             UserService>();

            return services;
        }
    }
}
