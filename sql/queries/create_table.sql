CREATE TABLE TaxiTrips (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    VendorID INT NOT NULL,
    TpepPickupDateTime DATETIME NOT NULL,
    TpepDropoffDateTime DATETIME NOT NULL,
    PassengerCount INT NOT NULL,
    TripDistance DECIMAL(10,2) NOT NULL,
    StoreAndForwardFlag CHAR NOT NULL CHECK (StoreAndForwardFlag='N' OR StoreAndForwardFlag='Y'),
    PULocationID INT NOT NULL,
    DOLocationID INT NOT NULL,
    FareAmount DECIMAL(10,2) NOT NULL,
    TipAmount DECIMAL(10,2) NOT NULL,

    INDEX IX_TaxiTrips_PULocationID (PULocationID),
    INDEX IX_TaxiTrips_TripDistance (TripDistance DESC),
    INDEX IX_TaxiTrips_PickupDropoff (TpepPickupDateTime, TpepDropoffDateTime)
);