namespace TaxiDataETL;

public class Program
{
    private static string ConnectionString = "Server=localhost;Database=csharptest;Trusted_Connection=False;TrustServerCertificate=True;User ID=";

    public static async Task Main(string[] args)
    {
        await Menu.DisplayMenu(ConnectionString);
    }
}