using Microsoft.Data.SqlClient;

namespace TaxiDataETL;

class Menu
{
    public static async Task DisplayMenu(string ConnectionString)
    {
        Console.Write("Enter database username: ");
        string? username = Console.ReadLine();
        if (username == null)
        {
            Console.WriteLine("Username cannot be null. Exiting...");
            return;
        }
        ConnectionString += $"{username};";

        Console.Write("Enter database password: ");
        string? password = Console.ReadLine();
        if (password == null)
        {
            Console.WriteLine("Password cannot be null. Exiting...");
            return;
        }
        ConnectionString += $"Password={password};";

        try
        {
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            Console.WriteLine("Connection established successfully.");
            await connection.CloseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to establish connection: {ex.Message}");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1. Process CSV File");
            Console.WriteLine("2. Exit");
            Console.Write("Enter choice: ");
            
            string? choice = Console.ReadLine();
            if (choice == null)
            {
                Console.WriteLine("Choice cannot be null. Exiting...");
                return;
            }
            
            switch (choice)
            {
                case "1":
                    Console.Write("Enter path to CSV file: ");
                    string? csvPath = Console.ReadLine();
                    if (csvPath == null)
                    {
                        Console.WriteLine("CSV path cannot be null. Exiting...");
                        return;
                    }
                    await CsvProcessing.ProcessCsv(csvPath, ConnectionString);
                    break;
                case "2":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }
}