# SQL Server Optimization for TaxiTrips Table

To ensure optimization for the queries mentioned in the assessment file, I created indexes on columns that will be frequently searched.

Since users need to calculate trip duration, I created a computed column `TripDuration` and added an index on it to optimize queries based on trip duration.
