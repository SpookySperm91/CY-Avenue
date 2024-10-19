using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.backend.monolith.client;
using CarlosYulo.backend.monolith.client.c_create;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.create;
using CarlosYulo.backend.monolith.delete;
using CarlosYulo.backend.monolith.employee;
using CarlosYulo.backend.monolith.employee.attendance;
using CarlosYulo.backend.monolith.employee.create;
using CarlosYulo.backend.monolith.employee.delete;
using CarlosYulo.backend.monolith.employee.salary;
using CarlosYulo.backend.monolith.employee.search;
using CarlosYulo.backend.monolith.employee.update;
using CarlosYulo.backend.monolith.item;
using CarlosYulo.backend.monolith.revenue;
using CarlosYulo.backend.monolith.revenue.i_liability;
using CarlosYulo.backend.monolith.shop;
using CarlosYulo.backend.monolith.shop.i_revenue;
using CarlosYulo.backend.monolith.systemAccount;
using CarlosYulo.backend.monolith.systemAccount.sy_login;
using CarlosYulo.database;
using Microsoft.Extensions.DependencyInjection;

namespace CarlosYulo
{
    public static class IoC
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            RegisterServices(services);
            return services.BuildServiceProvider();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Database
            services.AddScoped<DatabaseConnection>(provider =>
                new DatabaseConnection("localhost", "cy", "root", "123456", "3306"));
            
            // Common
            CommonClassDependencies(services);
            
            // Controller / Services
            ClientController(services);
            ClientConcreteDependencies(services);

            EmployeeController(services);
            EmployeeConcreteDependencies(services);

            SystemAccountController(services);
            SystemAccountConcreteDependencies(services);

            ItemController(services);
            ItemConcreteDependencies(services);

            RevenueController(services);
            RevenueConcreteDependencies(services);

            // Form 1
            services.AddScoped<Form1>();
        }


        
        // // // // // // // // // //
        private static void CommonClassDependencies(IServiceCollection services)
        {
            // common
            services.AddScoped<EmailSendBase>();
            services.AddScoped<ErrorMessageBox>();
            services.AddScoped<ImageViewer>();
            services.AddScoped<PasswordHashing>();
        }

        private static void ClientController(IServiceCollection services)
        {
            // services
            services.AddScoped<ClientCreateServices>();
            services.AddScoped<ClientUpdateServices>();
            services.AddScoped<ClientSearchServices>();
            services.AddScoped<ClientDeleteServices>();
            services.AddScoped<ClientEmailService>();

            // controller
            services.AddScoped<ClientController>();
        }

        private static void ClientConcreteDependencies(IServiceCollection services)
        {
            // create
            services.AddScoped<ClientCreateMember>();
            services.AddScoped<ClientCreateWalkIn>();
            // update
            services.AddScoped<ClientUpdateDetails>();
            services.AddScoped<ClientUpdateMembership>();
            services.AddScoped<ClientUpdateProfilePicture>();
            services.AddScoped<ClientUpdateStatus>();
            // search
            services.AddScoped<ClientSearchAll>();
            services.AddScoped<ClientSearchById>();
            services.AddScoped<ClientSearchByName>();
            // delete
            services.AddScoped<ClientDelete>();
            services.AddScoped<ClientDeleteAllExpired>();
            // email
            services.AddScoped<ClientEmailWelcome>();
            services.AddScoped<ClientEmailExpire>();
            services.AddScoped<ClientEmailRenewed>();
        }

        private static void EmployeeController(IServiceCollection services)
        {
            // services
            services.AddScoped<EmployeeOtherServices>();
            services.AddScoped<EmployeeAttendanceServices>();
            services.AddScoped<EmployeeSearchServices>();
            services.AddScoped<EmployeeUpdateServices>();

            // controller
            services.AddScoped<EmployeeController>();
        }

        private static void EmployeeConcreteDependencies(IServiceCollection services)
        {
            // others
            services.AddScoped<EmployeeCreateNew>();
            services.AddScoped<EmployeeDelete>();
            services.AddScoped<EmployeeSalaryUpdate>();
            // attendance
            services.AddScoped<EmployeeAttendanceCreate>();
            services.AddScoped<EmployeeAttendanceSearchAll>();
            services.AddScoped<EmployeeAttendanceSearchDaily>();
            services.AddScoped<EmployeeAttendanceSearchMonthly>();
            // search
            services.AddScoped<EmployeeSearchAll>();
            services.AddScoped<EmployeeSearchById>();
            services.AddScoped<EmployeeSearchByName>();
            // update
            services.AddScoped<EmployeeUpdateDetails>();
            services.AddScoped<EmployeeUpdateProfilePicture>();
        }

        private static void SystemAccountController(IServiceCollection services)
        {
            // services
            services.AddScoped<SystemAccountOtherServices>();
            services.AddScoped<SystemAccountLoginServices>();
            services.AddScoped<SystemAccountSearchServices>();

            // controller
            services.AddScoped<SystemAccountController>();
        }

        private static void SystemAccountConcreteDependencies(IServiceCollection services)
        {
            // other
            services.AddScoped<SystemAccountCreate>();
            services.AddScoped<SystemAccountDelete>();
            services.AddScoped<SystemAccountEmail>();
            // login
            services.AddScoped<SystemAccountCheckAccount>();
            services.AddScoped<SystemAccountCompareVerification>();
            services.AddScoped<SystemAccountVerification>();
            services.AddScoped<SystemAccountVerificationDelete>();
            // search
            services.AddScoped<SystemAccountSearchAll>();
            services.AddScoped<SystemAccountSearchByEmail>();
            services.AddScoped<SystemAccountSearchById>();
        }
        
        private static void ItemController(IServiceCollection services)
        {
            // services
            services.AddScoped<ItemCreateServices>();
            services.AddScoped<ItemSearchServices>();
            services.AddScoped<ItemUpdateServices>();

            // controller
            services.AddScoped<ItemController>();
        }

        private static void ItemConcreteDependencies(IServiceCollection services)
        {
            // create
            services.AddScoped<ItemCreate>();
            // search
            services.AddScoped<ItemSearchAll>();
            services.AddScoped<ItemSearchByCategory>();
            services.AddScoped<ItemSearchById>();
            // update
            services.AddScoped<ItemBuy>();
            services.AddScoped<ItemRestockQuantity>();
            services.AddScoped<ItemUpdateDetails>();
        }
        
        private static void RevenueController(IServiceCollection services)
        {
            services.AddScoped<RevenueSaleServices>();
            services.AddScoped<RevenueLiabilityServices>();

            // controller
            services.AddScoped<RevenueController>();
        }

        public static void RevenueConcreteDependencies(IServiceCollection services)
        {
            // sale
            services.AddScoped<RevenueGenerateFinalReport>();
            services.AddScoped<RevenueGeneratePartialReport>();
            services.AddScoped<RevenueGenerateItemSaleReport>();
            services.AddScoped<RevenueGenerateMembershipSalesReport>();
            // liability
            services.AddScoped<LiabilityTotalMonth>();
            services.AddScoped<LiabilityEmployeeSalary>();
            services.AddScoped<LiabilityItemRestock>();
        }
    }
}