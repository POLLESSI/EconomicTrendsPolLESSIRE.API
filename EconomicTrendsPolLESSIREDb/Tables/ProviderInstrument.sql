CREATE TABLE [dbo].[ProviderInstrument]
(
	[ProviderId] SMALLINT NOT NULL,
	[InstrumentId] BIGINT NOT NULL, 
	[ProviderSymbol] NVARCHAR(64) NOT NULL,
	[Realtime] BIT NOT NULL CONSTRAINT [DF_ProviderInstrument_Realtime] DEFAULT (0),
	[DelaySeconds] INT NULL,
	[Active] BIT NOT NULL CONSTRAINT [DF_ProviderInstrument_Active] DEFAULT (1),

	CONSTRAINT PK_ProviderInstrument PRIMARY KEY (ProviderId, InstrumentId),
	CONSTRAINT FK_ProviderInstrument_Provider FOREIGN KEY (ProviderId) REFERENCES dbo.Provider(Id),
	CONSTRAINT FK_ProviderInstrument_Instrument FOREIGN KEY (InstrumentId) REFERENCES dbo.Instrument(Id)
)

GO 

CREATE TRIGGER [dbo].[OnDeleteProviderInstrument]
	ON [dbo].[ProviderInstrument]
	INSTEAD OF DELETE
	AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE PI
		SET [Active] = 0
		FROM [dbo].[ProviderInstrument] AS PI
		INNER JOIN deleted AS D
			ON D.[ProviderId] = PI.[ProviderId]
		   AND D.[InstrumentId] = PI.[InstrumentId];
	END

GO

CREATE UNIQUE INDEX [UX_ProviderInstrument_Provider_ProviderSymbol]
ON [dbo].[ProviderInstrument]
(
    [ProviderId],
    [ProviderSymbol]
)
WHERE [Active] = 1;

GO