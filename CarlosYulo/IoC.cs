﻿using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
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

            services.AddScoped<DatabaseConnector>(provider =>
                new DatabaseConnector("localhost", "cy", "root", "123456", "3306"));

            ClientServiceDependancies(services);
            EmployeeServiceDependancies(services);
            SystemAccountServiceDependancies(services);
            CommonClassDependancies(services);

            // Form 1
            services.AddScoped<Form1>();
        }

        private static void ClientServiceDependancies(IServiceCollection services)
        {
            // Client Concrete Classes
            services.AddScoped<ICreate<Client>, ClientCreate>();
            services.AddScoped<IUpdate<Client>, ClientUpdate>();
            services.AddScoped<ISearch<Client, string>, ClientSearch>();
            services.AddScoped<ClientCreate>();
            services.AddScoped<ClientUpdate>();
            services.AddScoped<ClientSearch>();
            services.AddScoped<ClientSearchByName>();
            services.AddScoped<ClientSearchById>();
            services.AddScoped<ClientSearchAll>();
            services.AddScoped<ClientUpdateDetails>();
            services.AddScoped<IClientCreate, ClientCreate>();
            services.AddScoped<IClientEmail, ClientEmailExpire>();

            // Experimental
            services.AddScoped<ISearchByFullName<Client, string>, ClientSearchByName>();
            services.AddScoped<ISearchById<Client, string>, ClientSearchById>();
            services.AddScoped<ISearchAll<Client, string?>, ClientSearchAll>();

            services.AddScoped<IClientUpdate, ClientUpdateMembership>();
            services.AddScoped<ClientUpdateMembership>();
            services.AddScoped<IUpdateProfilePicture<bool, Client>, ClientUpdateProfilePicture>();
            services.AddScoped<ClientUpdateProfilePicture>();
            services.AddScoped<IClientSearchChild, ClientSearchService>();
            services.AddScoped<ClientSearchService>();

            services.AddScoped<IDeleteByEntity<Client>, ClientDelete>();
            services.AddScoped<IDeleteById, ClientDelete>();
            services.AddScoped<ClientDelete>();
            services.AddScoped<ClientDeleteAllExpired>();
            services.AddScoped<ClientUpdateStatus>();


            services.AddScoped<ClientEmailWelcome>();
            services.AddScoped<ClientEmailRenewed>();

            services.AddScoped<ClientCreateMember>();
            services.AddScoped<ClientCreateWalkIn>();

            // service class
        }

        private static void EmployeeServiceDependancies(IServiceCollection services)
        {
            // Employee Concrete Classes
            services.AddScoped<ISearch<Employee, int?>, EmployeeSearch>();

            // service class
            services.AddScoped<EmployeeCreateNew>();
            services.AddScoped<EmployeeUpdateDetails>();
            services.AddScoped<EmployeeUpdateProfilePicture>();

            services.AddScoped<EmployeeSearchAll>();
            services.AddScoped<EmployeeSearchById>();
            services.AddScoped<EmployeeSearchByName>();
            services.AddScoped<EmployeeDelete>();
            services.AddScoped<EmployeeSalaryUpdate>();
            services.AddScoped<EmployeeAttendanceCreate>();
            services.AddScoped<EmployeeAttendanceSearchAll>();
            services.AddScoped<EmployeeAttendanceSearchDaily>();
            services.AddScoped<EmployeeAttendanceSearchMonthly>();
        }

        private static void SystemAccountServiceDependancies(IServiceCollection services)
        {
            // SystemAccount Concrete Classes
            services.AddScoped<IDelete<SystemAccount>, SystemAccountDelete>();
            services.AddScoped<ICreate<SystemAccount>, SystemAccountCreate>();
            services.AddScoped<SystemAccountSearchById>();
            services.AddScoped<SystemAccountSearchByEmail>();
            services.AddScoped<SystemAccountSearchAll>();
            services.AddScoped<SystemAccountCheckAccount>();
            services.AddScoped<SystemAccountUpdatePassword>();
            services.AddScoped<SystemAccountUpdateDetails>();
            services.AddScoped<SystemAccountVerification>();
        }

        private static void CommonClassDependancies(IServiceCollection services)
        {
            // Concrete Classes
            services.AddScoped<ClientSearch>();
            services.AddScoped<PasswordHashing>();
            services.AddScoped<SystemAccountSearch>();
            services.AddScoped<SystemAccountDelete>();
            services.AddScoped<SystemAccountCreate>();
            services.AddScoped<ImageViewer>();
            services.AddScoped<SystemAccountEmail>();
            services.AddScoped<SystemAccountCompareVerification>();
            services.AddScoped<SystemAccountVerificationDelete>();
        }
    }
}