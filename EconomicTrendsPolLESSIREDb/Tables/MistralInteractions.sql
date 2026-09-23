CREATE TABLE [dbo].[MistralInteractions]
(
	[Id] INT IDENTITY,
	[Prompt] NVARCHAR(MAX) NOT NULL,
	[PromptHash] NVARCHAR(64) NOT NULL,
	[Response] NVARCHAR(MAX) NOT NULL,

	[CreatedAt] DATETIME2(0) NOT NULL CONSTRAINT [DF_MistralInteractions_CreatedAt] DEFAULT SYSUTCDATETIME(),
	
	[DateDeleted] DATETIME2(0) NULL,
	[Model] NVARCHAR(64) NULL, 
	[Temperature] FLOAT NULL, 
	[TokenCount] INT NULL, 

	[Active] BIT CONSTRAINT [DF_MistralInteractions_Active] DEFAULT 1,

	-- =====================================================
    -- SOURCE / MARKET CONTEXT
    -- =====================================================

    [SourceType] NVARCHAR(32) NULL,

    -- dbo.Instrument.Id = BIGINT
    [InstrumentId] BIGINT NULL,

    -- dbo.Provider.Id = SMALLINT
    [ProviderId] SMALLINT NULL,

    -- dbo.MarketQuote.Id = BIGINT
    [MarketQuoteId] BIGINT NULL,

    -- dbo.MarketTrade.Id = BIGINT
    [MarketTradeId] BIGINT NULL,

    -- MarketCandle composite key
    [MarketCandleIntervalCode] SMALLINT NULL,
    [MarketCandleOpenTimeUtc] DATETIME2(0) NULL,

    -- TechnicalIndicator composite key
    [TechnicalIndicatorIntervalCode] SMALLINT NULL,
    [TechnicalIndicatorType] SMALLINT NULL,
    [TechnicalIndicatorTimestampUtc] DATETIME2(0) NULL,

    -- Optional position snapshot
    [Latitude] FLOAT NULL,
    [Longitude] FLOAT NULL,

	CONSTRAINT [PK_MistralInteractions] PRIMARY KEY CLUSTERED ([Id]),

    -- =====================================================
    -- SIMPLE FOREIGN KEYS
    -- =====================================================

    CONSTRAINT [FK_MistralInteractions_Instrument] FOREIGN KEY ([InstrumentId]) REFERENCES [dbo].[Instrument] ([Id]),
    CONSTRAINT [FK_MistralInteractions_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider] ([Id]),
    CONSTRAINT [FK_MistralInteractions_MarketQuote] FOREIGN KEY ([MarketQuoteId]) REFERENCES [dbo].[MarketQuote] ([Id]),
    CONSTRAINT [FK_MistralInteractions_MarketTrade] FOREIGN KEY ([MarketTradeId]) REFERENCES [dbo].[MarketTrade] ([Id]),

    -- =====================================================
    -- COMPOSITE FOREIGN KEY : MarketCandle
    --
    -- PK MarketCandle =
    -- InstrumentId + IntervalCode + OpenTimeUtc
    -- =====================================================

    CONSTRAINT [FK_MistralInteractions_MarketCandle]
        FOREIGN KEY
        (
            [InstrumentId],
            [MarketCandleIntervalCode],
            [MarketCandleOpenTimeUtc]
        )
        REFERENCES [dbo].[MarketCandle]
        (
            [InstrumentId],
            [IntervalCode],
            [OpenTimeUtc]
        ),

    -- =====================================================
    -- COMPOSITE FOREIGN KEY : ProviderInstrument
    --
    -- PK ProviderInstrument =
    -- ProviderId + InstrumentId
    -- =====================================================

    CONSTRAINT [FK_MistralInteractions_ProviderInstrument]
        FOREIGN KEY
        (
            [ProviderId],
            [InstrumentId]
        )
        REFERENCES [dbo].[ProviderInstrument]
        (
            [ProviderId],
            [InstrumentId]
        ),

    -- =====================================================
    -- COMPOSITE FOREIGN KEY : TechnicalIndicator
    --
    -- PK TechnicalIndicator =
    -- InstrumentId
    -- + IntervalCode
    -- + IndicatorType
    -- + TimestampUtc
    -- =====================================================

    CONSTRAINT [FK_MistralInteractions_TechnicalIndicator]
        FOREIGN KEY
        (
            [InstrumentId],
            [TechnicalIndicatorIntervalCode],
            [TechnicalIndicatorType],
            [TechnicalIndicatorTimestampUtc]
        )
        REFERENCES [dbo].[TechnicalIndicator]
        (
            [InstrumentId],
            [IntervalCode],
            [IndicatorType],
            [TimestampUtc]
        ),

    CONSTRAINT [CK_MistralInteractions_SourceType]
        CHECK
        (
            [SourceType] IS NULL
            OR [SourceType] IN
            (
                'Instrument',
                'MarketCandle',
                'MarketQuote',
                'MarketSnapshot',
                'MarketTrade',
                'Provider',
                'ProviderInstrument',
                'TechnicalIndicator'
            )
        )
);

GO

CREATE TRIGGER [dbo].[OnDeleteMistralInteractions]
    ON [dbo].[MistralInteractions]
    INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE MI
    SET
        [Active] = 0,
        [DateDeleted] = SYSUTCDATETIME()
    FROM [dbo].[MistralInteractions] AS MI
    INNER JOIN deleted AS D
        ON D.[Id] = MI.[Id];
END;
GO