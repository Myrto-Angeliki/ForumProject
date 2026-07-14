USE ForumDatabase
GO

CREATE TABLE ForumAppSchema.Auth(
	Email NVARCHAR(50) PRIMARY KEY,
	PasswordHash VARBINARY(MAX),
	PasswordSalt VARBINARY(MAX)
)