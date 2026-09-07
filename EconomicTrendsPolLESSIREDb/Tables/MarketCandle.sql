CREATE TABLE [dbo].[MarketCandle]
(
	[InstrumentId] BIGINT NOT NULL,
	[IntervalCode] SMALLINT NOT NULL,
	[OpenTimeUtc] DATETIME2(0) NOT NULL,
	[OpenPrice] DECIMAL(19,8) NOT NULL,
	[HighPrice] DECIMAL(19,8) NOT NULL,
	[LowPrice] DECIMAL(19,8) NOT NULL,
	[ClosePrice] DECIMAL(19,8) NOT NULL,
	[Volume] DECIMAL(28,8) NULL,
	[VWAP] DECIMAL(19,8) NULL,
	[TradeCount] INT NULL,
	[IsFinal] BIT NOT NULL DEFAULT 0,
	[Active] BIT DEFAULT 1

	CONSTRAINT PK_MarketCandle PRIMARY KEY CLUSTERED (InstrumentId, IntervalCode, OpenTimeUtc)
)

GO
