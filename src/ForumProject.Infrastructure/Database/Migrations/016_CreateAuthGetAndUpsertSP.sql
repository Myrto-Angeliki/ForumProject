USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spRegistration_Upsert
    @Email NVARCHAR(50),
    @PasswordHash VARBINARY(MAX),
    @PasswordSalt VARBINARY(MAX)
AS 
BEGIN
    IF NOT EXISTS (SELECT * FROM ForumAppSchema.Auth WHERE Email = @Email)
        BEGIN
            INSERT INTO ForumAppSchema.Auth(
                [Email],
                [PasswordHash],
                [PasswordSalt]
            ) VALUES (
                @Email,
                @PasswordHash,
                @PasswordSalt
            )
        END
    ELSE
        BEGIN
            UPDATE ForumAppSchema.Auth 
                SET PasswordHash = @PasswordHash,
                    PasswordSalt = @PasswordSalt
                WHERE Email = @Email
        END
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spLoginConfirmation_Get
    @Email NVARCHAR(50)
AS
BEGIN
    SELECT  [PasswordHash],
            [PasswordSalt] 
    FROM ForumAppSchema.Auth AS Auth
        WHERE Auth.Email = @Email
END;
GO