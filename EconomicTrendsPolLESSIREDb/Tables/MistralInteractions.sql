CREATE TABLE [dbo].[MistralInteractions]
(
	[Id] INT IDENTITY,
	[Prompt] NVARCHAR(MAX) NOT NULL,
	[PromptHash] NVARCHAR(64) NOT NULL,
	[Response] NVARCHAR(MAX) NOT NULL,
	[CreatedAt] DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateDeleted] DATETIME2(0) NULL,
	[Model] NVARCHAR(64) NULL, 
	[Temperature] FLOAT NULL, 
	[TokenCount] INT NULL, 
	[Active] BIT DEFAULT 1

	CONSTRAINT [PK_MistralInteractions] PRIMARY KEY ([Id] ASC)
)

GO

CREATE TRIGGER [dbo].[OnDeleteMistralInteractions]
    ON [dbo].[MistralInteractions]
    INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE G
    SET Active      = 0,
        DateDeleted = SYSUTCDATETIME()
    FROM dbo.MistralInteractions AS G
    INNER JOIN deleted d ON d.Id = G.Id;
END;
GO