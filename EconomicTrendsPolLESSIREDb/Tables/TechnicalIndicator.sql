CREATE TABLE [dbo].[TechnicalIndicator]
(
	[InstrumentId] BIGINT NOT NULL,
 	[IntervalCode] SMALLINT NOT NULL,
	[TimestampUtc] DATETIME2(0) NOT NULL,
	[IndicatorType] SMALLINT NOT NULL,
	[Value1] DECIMAL(19,8) NULL,
	[Value2] DECIMAL(19,8) NULL, 
	[Value3] DECIMAL(19,8) NULL,

	[ParameterHash] BINARY(16) NULL,
	[Active] BIT DEFAULT 1

	CONSTRAINT PK_TechnicalIndicator PRIMARY KEY ([InstrumentId], [IntervalCode], [IndicatorType], [TimestampUtc])
 )

 GO

 CREATE TRIGGER [dbo].[OnDeleteTechnicalIndicator]
	ON [dbo].[TechnicalIndicator]
	INSTEAD OF DELETE
	AS 
	BEGIN
		UPDATE [TechnicalIndicator] SET Active = 0
		WHERE [InstrumentId] IN (SELECT InstrumentId FROM deleted)
	END
GO
