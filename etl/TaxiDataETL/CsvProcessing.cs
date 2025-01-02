using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace TaxiDataETL;

public class CsvProcessing
{
    private const int BatchSize = 1000;
    public static async Task ProcessCsv(string csvPath, string ConnectionString)
    {
        var duplicates = new HashSet<string>();
        var processedRecords = new HashSet<string>();
        var logFile = Path.Combine("Logs", $"log_{DateTime.Now:yyyyMMddHHmmss}.txt");
        try
        {
            await ProcessCsvFile(csvPath, duplicates, processedRecords, ConnectionString, logFile);
            await WriteDuplicatesToFile(duplicates);
            Console.WriteLine($"Processing complete. Total records inserted: {processedRecords.Count}");
            File.AppendAllText(logFile, $"Processing complete. Total records inserted: {processedRecords.Count}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    public static async Task ProcessCsvFile(string csvPath, HashSet<string> duplicates, HashSet<string> processedRecords, string ConnectionString, string logFile)
    {
        int count = 0;

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, config);

        await csv.ReadAsync();
        csv.ReadHeader();

        var records = new List<TaxiTrip>();

        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        while (await csv.ReadAsync())
        {
            try
            {
                var record = new TaxiTrip
                {
                    PickupDateTime = Helpers.ConvertToUtc(DateTime.ParseExact(Helpers.RemoveWhitespaces(csv.GetField("tpep_pickup_datetime")!), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)),
                    DropoffDateTime = Helpers.ConvertToUtc(DateTime.ParseExact(Helpers.RemoveWhitespaces(csv.GetField("tpep_dropoff_datetime")!), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)),
                    PassengerCount = int.TryParse(Helpers.RemoveWhitespaces(csv.GetField("passenger_count")), out var passengerCount) ? passengerCount : null,
                    TripDistance = decimal.TryParse(Helpers.RemoveWhitespaces(csv.GetField("trip_distance")), NumberStyles.Any, CultureInfo.InvariantCulture, out var tripDistance) ? tripDistance : null,
                    StoreAndForwardFlag = Helpers.ConvertFlag(Helpers.RemoveWhitespaces(csv.GetField("store_and_fwd_flag")?.Trim())),
                    PULocationID = int.TryParse(Helpers.RemoveWhitespaces(csv.GetField("PULocationID")), out var puLocationId) ? puLocationId : throw new Exception("PULocationID cannot be null"),
                    DOLocationID = int.TryParse(Helpers.RemoveWhitespaces(csv.GetField("DOLocationID")), out var doLocationId) ? doLocationId : throw new Exception("DOLocationID cannot be null"),
                    FareAmount = decimal.TryParse(Helpers.RemoveWhitespaces(csv.GetField("fare_amount")), NumberStyles.Any, CultureInfo.InvariantCulture, out var fareAmount) ? fareAmount : null,
                    TipAmount = decimal.TryParse(Helpers.RemoveWhitespaces(csv.GetField("tip_amount")), NumberStyles.Any, CultureInfo.InvariantCulture, out var tipAmount) ? tipAmount : null
                };

                string key = $"{record.PickupDateTime}|{record.DropoffDateTime}|{record.PassengerCount}";

                if (processedRecords.Add(key))
                {
                    records.Add(record);
                }
                else
                {
                    duplicates.Add(record.ToString());
                }

                if (records.Count >= BatchSize)
                {
                    await BulkInsertRecords(connection, records);
                    records.Clear();
                }
                count++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing record #{count} : '{ex.Message}'");
                await File.AppendAllTextAsync(logFile, $"Error processing record #{count} : '{ex.Message}'{Environment.NewLine}");
            }
        }

        if (records.Any())
        {
            await BulkInsertRecords(connection, records);
        }
    }

    public static async Task WriteDuplicatesToFile(HashSet<string> duplicates)
    {
        Console.WriteLine($"Total duplicates found: {duplicates.Count}");
        await File.WriteAllLinesAsync("duplicates.csv", duplicates);
    }

    private static async Task BulkInsertRecords(SqlConnection connection, List<TaxiTrip> records)
    {
        using var bulkCopy = new SqlBulkCopy(connection)
        {
            DestinationTableName = "TaxiTrips",
            BatchSize = BatchSize
        };

        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.PickupDateTime), "TpepPickupDateTime");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.DropoffDateTime), "TpepDropoffDateTime");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.PassengerCount), "PassengerCount");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.TripDistance), "TripDistance");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.StoreAndForwardFlag), "StoreAndForwardFlag");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.PULocationID), "PULocationID");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.DOLocationID), "DOLocationID");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.FareAmount), "FareAmount");
        bulkCopy.ColumnMappings.Add(nameof(TaxiTrip.TipAmount), "TipAmount");

        var dataTable = records.ToDataTable();
        await bulkCopy.WriteToServerAsync(dataTable);
    }
}