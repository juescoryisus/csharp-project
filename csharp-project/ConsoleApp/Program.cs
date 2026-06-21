using Microsoft.Data.SqlClient;
using MySqlConnector;

namespace ConsoleApp;

// ======================================================================
// Aplicacion de consola .NET Core que prueba el acceso a AMBOS contenedores:
//   1) SQL Server  (puerto 1433)
//   2) MariaDB     (puerto 3306)
// ======================================================================
public static class Program
{
    // Cadenas de conexion hacia los contenedores Docker locales.
    private const string SqlServerConn =
        "Server=localhost,1433;Database=TiendaDB;User Id=sa;Password=Passw0rd!2024;TrustServerCertificate=True;";

    private const string MariaDbConn =
        "Server=localhost;Port=3306;Database=TiendaDB;User Id=root;Password=Passw0rd!2024;";

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("==============================================");
        Console.WriteLine(" Prueba de acceso a SQL Server y MariaDB");
        Console.WriteLine("==============================================\n");

        try
        {
            LeerClientesSqlServer();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SQL Server] Error: {ex.Message}\n");
        }

        try
        {
            LeerClientesMariaDb();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MariaDB] Error: {ex.Message}\n");
        }

        Console.WriteLine("Presione una tecla para salir...");
        Console.ReadKey();
    }

    // -------- SQL Server: SELECT * FROM Clientes --------
    private static void LeerClientesSqlServer()
    {
        Console.WriteLine(">> Conectando a SQL Server (localhost:1433)...");
        using var conn = new SqlConnection(SqlServerConn);
        conn.Open();
        Console.WriteLine("   Conexion exitosa.\n");

        const string sql = "SELECT ClienteId, Nombre, Email, Ciudad FROM Clientes;";
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        Console.WriteLine("   CLIENTES EN SQL SERVER:");
        Console.WriteLine("   {0,-4} {1,-20} {2,-30} {3,-15}", "ID", "Nombre", "Email", "Ciudad");
        Console.WriteLine("   " + new string('-', 70));
        while (reader.Read())
        {
            Console.WriteLine("   {0,-4} {1,-20} {2,-30} {3,-15}",
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? "" : reader.GetString(3));
        }
        Console.WriteLine();
    }

    // -------- MariaDB: SELECT * FROM Clientes --------
    private static void LeerClientesMariaDb()
    {
        Console.WriteLine(">> Conectando a MariaDB (localhost:3306)...");
        using var conn = new MySqlConnection(MariaDbConn);
        conn.Open();
        Console.WriteLine("   Conexion exitosa.\n");

        const string sql = "SELECT ClienteId, Nombre, Email, Ciudad FROM Clientes;";
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        Console.WriteLine("   CLIENTES EN MARIADB:");
        Console.WriteLine("   {0,-4} {1,-20} {2,-30} {3,-15}", "ID", "Nombre", "Email", "Ciudad");
        Console.WriteLine("   " + new string('-', 70));
        while (reader.Read())
        {
            Console.WriteLine("   {0,-4} {1,-20} {2,-30} {3,-15}",
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? "" : reader.GetString(3));
        }
        Console.WriteLine();
    }
}
