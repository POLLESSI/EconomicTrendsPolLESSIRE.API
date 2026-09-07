CREATE TABLE [dbo].[Instrument]
( 
	[Id] BIGINT IDENTITY(1,1),
	[Symbol] NVARCHAR(32) NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
	[AssetClass] TINYINT NOT NULL,
	[ExchangeCode] NVARCHAR(32) NULL,
	[CurrencyCode] CHAR(3) NULL,
	[CreatedAtUtc] DATETIME2(3) DEFAULT SYSUTCDATETIME(),
	[Active] BIT DEFAULT 1,

	CONSTRAINT PK_Instrument PRIMARY KEY CLUSTERED ([Id]),
)

GO

CREATE TRIGGER [dbo].[OnDeleteInstrument]
	ON [dbo].[Instrument]
	INSTEAD OF DELETE
	AS 
	BEGIN
		SET NOCOUNT ON;

		UPDATE I
		SET [Active] = 0
		FROM [dbo].[Instrument] AS I
		INNER JOIN deleted AS D
			ON D.[Id] = I.[Id];
	END

GO

CREATE INDEX IX_Instrument_Active_CreateAtUtc
ON dbo.[Instrument] ([Active], [CreatedAtUtc] DESC);

GO

CREATE UNIQUE INDEX [UX_Instrument_Symbol_ExchangeCode]
ON [dbo].[Instrument]
(
    [Symbol],
    [ExchangeCode]
)
WHERE [Active] = 1;