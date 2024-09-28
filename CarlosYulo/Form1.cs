using CarlosYulo.backend.monolith;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    private DatabaseConnector db;
    IClientService clientService;
    
    public Form1()
    {
        InitializeComponent();
        db = new DatabaseConnector();
        
        clientService = new ClientService(
            new ClientCreate(db),
            new ClientUpdate(db),
            new ClientDelete(db),
            new ClientSearch(db));
    }
}