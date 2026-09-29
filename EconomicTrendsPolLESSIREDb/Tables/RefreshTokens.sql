CREATE TABLE [dbo].[RefreshTokens]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Token] NVARCHAR(512) NULL,
	[Email] NVARCHAR(256) NOT NULL,
	[ExpiryDate] DATETIME2 NOT NULL,
	[Status] INT NOT NULL, 
	[IsRevoked] BIT NOT NULL CONSTRAINT [DF_RefreshTokens_IsRevoked] DEFAULT (0),
	[CreatedAt] DATETIME2(3) NOT NULL CONSTRAINT [DF_RefreshTokens_CreatedAt] DEFAULT (SYSUTCDATETIME()),
	[TokenHash] VARBINARY(32) NOT NULL,
	[TokenSalt] VARBINARY(16) NOT NULL,

	CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id])
);

GO

CREATE INDEX [IX_RefreshTokens_Email_Status_ExpiryDate] 
ON [dbo].[RefreshTokens] 
(
	[Email],
	[Status],
	[ExpiryDate]
);

GO