namespace TaxiDataETL;

public class Helpers
{
    public static DateTime ConvertToUtc(DateTime dateTime) =>
        TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

    public static string RemoveWhitespaces(string value) =>
        value.Trim();

    public static string? ConvertFlag(string flag) =>
    flag.Trim().ToUpper() switch
    {
        "Y" => "Yes",
        "N" => "No",
        _ => null
    };
}