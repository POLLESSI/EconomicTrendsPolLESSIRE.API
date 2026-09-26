CREATE TABLE [dbo].[Users] (

	[Id] INT IDENTITY(1,1) NOT NULL,
	[Email] NVARCHAR(64) NOT NULL,
	[PasswordHashV2] NVARCHAR(512) NULL,
	[SecurityStamp] UNIQUEIDENTIFIER NOT NULL,
	[Role] INT NOT NULL CONSTRAINT DF_Users_Role DEFAULT 0,
	[Status] INT NOT NULL,
	[Active] BIT NOT NULL CONSTRAINT DF_Users_Active DEFAULT 1,

	CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
	CONSTRAINT [UQ_Users_Email] UNIQUE ([Email]),
	CONSTRAINT [UQ_Users_SecurityStamp] UNIQUE ([SecurityStamp]),
	CONSTRAINT [CK_Users_Role] CHECK ([Role] IN (0, 1, 2, 4))

);
GO

CREATE TRIGGER [dbo].[OnDeleteUser]
ON [dbo].[Users]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Users]
	SET Active = 0
	WHERE Id IN (SELECT Id FROM deleted);
END

GO

