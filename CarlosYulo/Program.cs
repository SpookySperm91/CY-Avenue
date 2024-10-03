using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee;
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
            // Initialize application configurations and run the form
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

            // Concrete classes
            services.AddScoped<ICreate<Client>, ClientCreate>();
            services.AddScoped<IUpdate<Client>, ClientUpdate>();
            services.AddScoped<IDelete<Client>, ClientDelete>();
            services.AddScoped<ISearch<Client, string>, ClientSearch>();
            services.AddScoped<IClientUpdate, ClientUpdate>();
            services.AddScoped<IClientCreate, ClientCreate>();
            
            // services.AddScoped<ICreate<Employee>, ClientCreate>();
            // services.AddScoped<IUpdate<Employee>, ClientUpdate>();
            // services.AddScoped<IDelete<Employee>, EmployeeDelete>();
            services.AddScoped<ISearch<Employee, int?>, EmployeeSearch>();
            
            // ClientService class
            //services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ClientService>();
            
            // EmployeeService class
            services.AddScoped<EmployeeService>();

            // Email class
            services.AddScoped<EmailMessage>();

            
            services.AddScoped<Form1>();
        }
    }
}