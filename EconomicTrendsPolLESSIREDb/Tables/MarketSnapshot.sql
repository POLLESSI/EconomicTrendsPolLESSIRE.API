CREATE TABLE [dbo].[MarketSnapshot]
(
	[InstrumentId] BIGINT NOT NULL,
	[LastPrice]           DECIMAL(19,8) NULL,

    [BidPrice]            DECIMAL(19,8) NULL,
    [AskPrice]            DECIMAL(19,8) NULL,

    [OpenPrice]           DECIMAL(19,8) NULL,
    [HighPrice]           DECIMAL(19,8) NULL,
    [LowPrice]            DECIMAL(19,8) NULL,

    [PreviousClose]       DECIMAL(19,8) NULL,

    [Volume]              DECIMAL(28,8) NULL,

    [LastProviderId]      SMALLINT NULL,

    [MarketTimestampUtc]  DATETIME2(3) NULL,
    [ReceivedAtUtc]       DATETIME2(3) NULL,
    [Active] BIT DEFAULT 1

    CONSTRAINT PK_MarketSnapshot PRIMARY KEY (InstrumentId)
)

GO

CREATE TRIGGER [dbo].[OnDeleteMarketSnapshot]
    ON [dbo].[MarketSnapshot]
    INSTEAD OF DELETE
    AS
    BEGIN
        UPDATE [MarketSnapshot] SET Active = 0
        WHERE [InstrumentId] IN (SELECT InstrumentId FROM deleted)
    END