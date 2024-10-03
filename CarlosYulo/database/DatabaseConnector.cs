using System.Data;
using MySql.Data.MySqlClient;

namespace CarlosYulo.database;

public class DatabaseConnector
    {
        private readonly string connectionString;

        public DatabaseConnector(string serverName, string databaseName, string username, string password, string port)
        {
            connectionString = CreateConnectionString(serverName, databaseName, username, password, port);
        }

        // Build connection string in a separate method
        private string CreateConnectionString(string serverName, string databaseName, string username, string password, string port)
        {
            return $"Server={serverName};Database={databaseName};User={username};Password={password};Port={port};Convert Zero Datetime=True;";
        }

        // Method to create a new connection
        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // Method to check the connection, with early return to avoid nesting
        public bool TestConnection()
        {
            using var connection = CreateConnection();
            return TryOpenConnection(connection);
        }

        // Open connection and handle exceptions
        private bool TryOpenConnection(MySqlConnection connection)
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection successful!");
                return true;
            }
            catch (MySqlException ex)
            {
                HandleException("Error connecting to the database", ex);
                return false;
            }
        }

        // Example query execution method
        public DataTable ExecuteQuery(string query)
        {
            using var connection = CreateConnection();
            if (!TryOpenConnection(connection))
                return null;

            return ExecuteQueryInternal(connection, query);
        }

        // Internal method to execute a query, separated from connection logic
        private DataTable ExecuteQueryInternal(MySqlConnection connection, string query)
        {
            using var command = new MySqlCommand(query, connection);
            using var adapter = new MySqlDataAdapter(command);
            var dataTable = new DataTable();

            try
            {
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (MySqlException ex)
            {
                HandleException("Error executing query", ex);
                return null;
            }
        }

        // Centralized exception handling method
        private void HandleException(string message, Exception ex)
        {
            // Log the error to console or a proper logging framework
            Console.WriteLine($"{message}: {ex.Message}");
        }
    }