ALTER TABLE TaxiTrips
ADD TripDuration AS DATEDIFF(SECOND, TpepPickupDateTime, TpepDropoffDateTime) PERSISTED;