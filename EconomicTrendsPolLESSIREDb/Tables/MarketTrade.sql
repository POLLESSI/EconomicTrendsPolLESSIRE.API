CREATE TABLE [dbo].[MarketTrade]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL,
	[InstrumentId] BIGINT NOT NULL,
	[ProviderId] SMAllINT NOT NULL,
	[TimestampUtc] DATETIME2 NOT NULL,
	[ReceivedAtUtc] DATETIME2 NOT NULL,
	[Price] DECIMAL(19,8) NOT NULL,
	[Quantity] DECIMAL(19,8) NULL,
	[SequenceNumber] BIGINT NULL,
	[Active] BIT DEFAULT 1,

	CONSTRAINT PK_MarketTrade PRIMARY KEY CLUSTERED (Id)
)

GO

CREATE INDEX [IX_MarketTrade_Instrument_Timestamp]
ON [dbo].[MarketTrade]
(
    [InstrumentId],
    [TimestampUtc] DESC
)
INCLUDE
(
    [Price],
    [Quantity],
    [ProviderId]
);
GO
