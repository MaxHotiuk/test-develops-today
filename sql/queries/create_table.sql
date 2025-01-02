CREATE TABLE TaxiTrips (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TpepPickupDateTime DATETIME NOT NULL,
    TpepDropoffDateTime DATETIME NOT NULL,
    PassengerCount INT,
    TripDistance DECIMAL(10,2),
    StoreAndForwardFlag VARCHAR(3) CHECK (StoreAndForwardFlag='No' OR StoreAndForwardFlag='Yes'),
    PULocationID INT NOT NULL,
    DOLocationID INT NOT NULL,
    FareAmount DECIMAL(10,2),
    TipAmount DECIMAL(10,2),

    INDEX IX_TaxiTrips_PULocationID (PULocationID),
    INDEX IX_TaxiTrips_TripDistance (TripDistance DESC),
    INDEX IX_TaxiTrips_PickupDropoff (TpepPickupDateTime, TpepDropoffDateTime)
);