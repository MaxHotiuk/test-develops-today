using System.Data;

namespace TaxiDataETL;

public static class Extensions
{
    public static DataTable ToDataTable<T>(this List<T> items)
    {
        var dataTable = new DataTable(typeof(T).Name);

        var properties = typeof(T).GetProperties();
        foreach (var prop in properties)
        {
            dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var item in items)
        {
            var values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(item) ?? DBNull.Value;
            }
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }
}