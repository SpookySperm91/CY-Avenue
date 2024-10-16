using System.Data;
using MySql.Data.MySqlClient;

namespace CarlosYulo.database;

public class DatabaseConnection
{
    // Exclusive access only this class
    private string servername;
    private string databasename;
    private string username;
    private string password;
    private string port;
    private string strConnection;

    public MySqlConnection mysqlConnection { get; set; }
    public MySqlCommand mySqlCommand { get; set; }
    public MySqlDataAdapter adapter { get; set; }
    public MySqlTransaction transaction { get; set; }
    public DataTable dataTable { get; set; }


    public DatabaseConnection(string servername, string databasename, string username, string password, string port)
    {
        this.servername = servername;
        this.databasename = databasename;
        this.username = username;
        this.password = password;
        this.port = port;
        fncConnectToDatabase();
    }

    private string connectionstring()
    {
        return "Server=" + servername + "; " +
               "Database =" + databasename + "; " +
               "User=" + username + "; " +
               "Password=" + password + "; " +
               "Port =" + port + "; " +
               "Convert Zero Datetime =true";
    }


    public bool fncConnectToDatabase()
    {
        try
        {
            // instantiating connection string
            strConnection = connectionstring();

            // setting up connection
            mysqlConnection = new MySqlConnection(strConnection);
            mySqlCommand = new MySqlCommand(strConnection, mysqlConnection);
            
            if (mysqlConnection.State == ConnectionState.Closed)
            {
                mySqlCommand.Connection = mysqlConnection;
                mysqlConnection.Open();
                Console.WriteLine("Connection established");
                return true;
            }

            mysqlConnection.Close();
            Console.WriteLine("Connection already established");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            MessageBox.Show("Error database connection: " + e.Message);
        }

        return false;
    }


    public void checkDatabaseConnection()
    {
        if (!fncConnectToDatabase())
        {
            mysqlConnection.Open();
        }
    }
}