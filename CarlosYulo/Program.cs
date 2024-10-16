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
            ApplicationConfiguration.Initialize();

            // DEPENDENCY INJECTION / INVERSION OF CONTROL... IOC 
            var serviceProvider = IoC.ConfigureServices();

            // Application run
            Application.Run(serviceProvider.GetService<Form1>());
        }
    }
}