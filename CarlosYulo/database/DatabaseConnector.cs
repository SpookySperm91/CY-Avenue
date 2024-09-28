using System.Data;
using MySql.Data.MySqlClient;

namespace CarlosYulo.database;

public class DatabaseConnector
{
    private string servername { get; set; }
    private string databasename { get; set; }
    private string username { get; set; }
    private string password { get; set; }
    private string port { get; set; }

    public MySqlConnection mysqlConnection { get; set; }
    private MySqlCommand mysqlCommand { get; set; }
    private string strConnection { get; set; }

    public bool FncConnectionToDatabase()
    {
        try
        {
            servername = "localhost";
            databasename = "cy";
            username = "root";
            password = "123456";
            port = "3306";

            strConnection = "Server=" + servername + ";" +
                            "Database=" + databasename + ";" +
                            "User=" + username + ";" +
                            "Password=" + password + ";" +
                            "Port=" + port + ";" +
                            "Convert Zero Datetime=True";

            mysqlConnection = new MySqlConnection(strConnection);
            mysqlCommand = new MySqlCommand(strConnection, mysqlConnection);
            if (mysqlConnection.State == ConnectionState.Closed)
            {
                mysqlCommand.Connection = mysqlConnection;
                mysqlConnection.Open();
                return true;
            }

            mysqlConnection.Close();
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error Message" + ex.Message);
        }

        return false;
    }

    public void CheckDatabaseConnection()
    {
        if (!FncConnectionToDatabase())
        {
            Console.WriteLine("Failed to connect to the database.");
        }
        else
        {
            Console.WriteLine("Database connection is active.");
        }
    }
}