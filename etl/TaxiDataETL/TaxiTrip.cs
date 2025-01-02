namespace TaxiDataETL;

public class TaxiTrip
{
    public DateTime PickupDateTime { get; set; }
    public DateTime DropoffDateTime { get; set; }
    public int? PassengerCount { get; set; }
    public decimal? TripDistance { get; set; }
    public string? StoreAndForwardFlag { get; set; } = "";
    public int? PULocationID { get; set; }
    public int? DOLocationID { get; set; }
    public decimal? FareAmount { get; set; }
    public decimal? TipAmount { get; set; }

    public override string ToString() =>
        $"{PickupDateTime}|{DropoffDateTime}|{PassengerCount}|{TripDistance}|{StoreAndForwardFlag}|{PULocationID}|{DOLocationID}|{FareAmount}|{TipAmount}";
}
