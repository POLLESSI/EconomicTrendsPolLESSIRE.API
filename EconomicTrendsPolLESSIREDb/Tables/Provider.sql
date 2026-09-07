CREATE TABLE [dbo].[Provider]
(
	[Id] SMALLINT IDENTITY (1,1) NOT NULL,
	[Code] NVARCHAR(32) NOT NULL,
	[Name] NVARCHAR(128) NOT NULL,
	[CreatedAtUtc] DATETIME2(3) NOT NULL CONSTRAINT [DF_Provider_CreatedAtUtc] DEFAULT SYSUTCDATETIME(),
	[Active] BIT NOT NULL CONSTRAINT [DF_Provider_Active] DEFAULT (1)

	CONSTRAINT [PK_Provider] PRIMARY KEY CLUSTERED ([Id])
	CONSTRAINT [UQ_Provider_Code] UNIQUE ([Code])
)

GO

CREATE TRIGGER [dbo].[OnDeleteProvider]
    ON [dbo].[Provider]
    INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE P
    SET [Active] = 0
    FROM [dbo].[Provider] AS P
    INNER JOIN deleted AS D
        ON D.[Id] = P.[Id];
END;
GO

CREATE UNIQUE INDEX [UX_Provider_Code]
ON [dbo].[Provider] ([Code])
WHERE [Active] = 1;
GO
