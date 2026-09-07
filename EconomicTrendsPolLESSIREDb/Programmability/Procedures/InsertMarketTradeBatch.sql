CREATE PROCEDURE [dbo].[InsertMarketTradeBatch]
    @Trades [dbo].[MarketTradeBatch] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[MarketTrade]
    (
        [InstrumentId],
        [ProviderId],
        [TimestampUtc],
        [ReceivedAtUtc],
        [Price],
        [Quantity],
        [SequenceNumber]
    )
    SELECT
        [InstrumentId],
        [ProviderId],
        [TimestampUtc],
        [ReceivedAtUtc],
        [Price],
        [Quantity],
        [SequenceNumber]
    FROM @Trades;
END;
GO