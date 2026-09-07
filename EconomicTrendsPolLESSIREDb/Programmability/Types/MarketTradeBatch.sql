CREATE TYPE [dbo].[MarketTradeBatch] AS TABLE
(
    [InstrumentId] BIGINT NOT NULL,
    [ProviderId] SMALLINT NOT NULL,

    [TimestampUtc] DATETIME2(3) NOT NULL,
    [ReceivedAtUtc] DATETIME2(3) NOT NULL,

    [Price] DECIMAL(19,8) NOT NULL,
    [Quantity] DECIMAL(19,8) NULL,

    [SequenceNumber] BIGINT NULL
);
GO