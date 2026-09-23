CREATE TABLE [dbo].[Suggestion]
(
	[Id]                   INT IDENTITY,
    [User_Id]              INT NOT NULL,
    [DateSuggestion]       DATETIME2(0) NOT NULL CONSTRAINT [DF_Suggestion_DateSuggestion] DEFAULT (SYSUTCDATETIME()),
    [OriginalPlace]        NVARCHAR(128) NULL,
    [SuggestedAlternatives] NVARCHAR(MAX) NULL,
    [Reason]               NVARCHAR(MAX) NULL,
    [Active]               BIT NOT NULL CONSTRAINT [DF_Suggestion_Active] DEFAULT (1),
    [DateDeleted]          DATETIME2(0) NULL,
    [CrowdId]              INT NULL,
    [EventId]              INT NULL,
    [PlaceId]              INT NULL,
    [TrafficId]            INT NULL,
    [ForecastId]           INT NULL,
    [LocationName]         NVARCHAR(128) NULL,
    Latitude      DECIMAL(9,6) NULL,
    Longitude     DECIMAL(9,6) NULL,
    DistanceKm    FLOAT NULL,
    LocationLabel NVARCHAR(128) NULL,
    Title         NVARCHAR(256) NULL,
    Message       NVARCHAR(512) NULL,
    Context       NVARCHAR(1024) NULL

    CONSTRAINT [PK_Suggestion] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Suggestion_User] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users]([Id])
);

GO
CREATE INDEX IX_Suggestion_Active_DateSuggestion
ON dbo.Suggestion (Active, DateSuggestion DESC);

GO
CREATE TRIGGER [dbo].[OnDeleteSuggestion]
    ON [dbo].[Suggestion]
    INSTEAD OF DELETE
    AS
    BEGIN
        UPDATE Suggestion 
        SET Active = 0,
            DateDeleted = SYSUTCDATETIME()
        WHERE Id IN (SELECT Id FROM deleted)
    END
GO

CREATE INDEX IX_Suggestion_OriginalPlace ON dbo.Suggestion (OriginalPlace);
GO
