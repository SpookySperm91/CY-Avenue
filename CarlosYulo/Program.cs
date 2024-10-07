using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee;
using CarlosYulo.backend.test;
using CarlosYulo.database;
using Microsoft.Extensions.DependencyInjection;

namespace CarlosYulo
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();
            DependencyInjection(services);

            var serviceProvider = services.BuildServiceProvider();

            var dbConnector = serviceProvider.GetService<DatabaseConnector>();
            if (dbConnector.TestConnection())
            {
                Console.WriteLine("Connection successful!");
            }
            else
            {
                Console.WriteLine("Failed to connect to the database.");
                return; // Exit the application if the connection fails
            }

            var mainForm = serviceProvider.GetService<Form1>();
            if (mainForm != null)
            {
                Application.Run(mainForm);
            }
            else
            {
                Console.WriteLine("Failed to create the main form.");
            }
        }

        // DEPENDENCY INJECTION / INVERSION OF CONTROL... IOC 
        private static void DependencyInjection(IServiceCollection services)
        {
            // Database
            services.AddScoped<DatabaseConnector>(
                provider =>
                    new DatabaseConnector(
                        "localhost",
                        "cy",
                        "root",
                        "123456",
                        "3306"));

            // Client Concrete Classes
            services.AddScoped<ICreate<Client>, ClientCreate>();
            services.AddScoped<IUpdate<Client>, ClientUpdate>();
            services.AddScoped<IDelete<Client>, ClientDelete>();
            services.AddScoped<ISearch<Client, string>, ClientSearch>();
            services.AddScoped<IClientUpdate, ClientUpdate>();
            services.AddScoped<IClientCreate, ClientCreate>();
            services.AddScoped<IClientEmail, ClientEmail>();

            // Employee Concrete Classes
            services.AddScoped<ISearch<Employee, int?>, EmployeeSearch>();

            // SystemAccount Concrete Classes
            services.AddScoped<IDelete<SystemAccount>, SystemAccountDelete>();
            services.AddScoped<ICreate<SystemAccount>, SystemAccountCreate>();

            // Concrete Classes
            services.AddScoped<ClientSearch>();
            services.AddScoped<PasswordHashing>();
            services.AddScoped<SystemAccountSearch>();
            services.AddScoped<SystemAccountDelete>();
            services.AddScoped<SystemAccountCreate>();
            services.AddScoped<ImageViewer>();

            // Test Classes
            services.AddScoped<ClientCreateTest>();


            // Register EmailSendBase to its concrete class
            services.AddScoped<EmailSendBase, ClientEmail>();
            services.AddScoped<ClientEmail>();

            // Register your services
            services.AddScoped<ClientService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<Form1>();

            // Form 1
            services.AddScoped<Form1>();
        }
    }
}