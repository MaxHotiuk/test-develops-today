# SQL Server Optimization for TaxiTrips Table

To ensure optimization for the queries mentioned in the assessment file, I created indexes on columns that will be frequently searched.

Since users need to calculate trip duration, I created a computed column `TripDuration` and added an index on it to optimize queries based on trip duration.

# ETL Optimization
Safety: ensured that foreign key fields cannot be null.

Had a thought that other fields should be checked to. For example, decimal wrote wrong should not be incerted as null. However, it could lead to loses of information and I didn't do that.

Added logging: for each attemt to proccess CSV file new log file will be created and any issues will be written there.

# Handle a 10GB CSV file

To handle a 10GB CSV file efficiently, I would implement partitioning to avoid loading the entire file into memory at once. Instead of reading all records into a list, I would process smaller batches directly, minimizing memory usage. Additionally, I would use multi-threading or parallel processing to read and insert records, ensuring SQL bulk inserts run in parallel for different chunks of data. Partitioning the CSV file beforehand into smaller segments and processing them separately would also improve performance.

# Number of rows inserted: 29889
# Dublicates found: 111