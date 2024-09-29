using CarlosYulo.database;

namespace CarlosYulo;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Initialize and check the database connection before launching the form
        var dbConnector = new DatabaseConnector(
            "localhost",
            "cy",
            "root",
            "123456",
            "3306");
        
        // Test the database connection
        if (dbConnector.TestConnection())
        {
            Console.WriteLine("Connection successful!");
        }
        else
        {
            Console.WriteLine("Failed to connect to the database.");
        }

        // Initialize application configurations and run the form
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1(dbConnector));
    }
}