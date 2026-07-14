USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_Upsert
    @Username NVARCHAR(50),
    @Email NVARCHAR(50),
    @IsActive BIT = 1,
    @DeactivatedAt DATETIME2 = NULL,
    @UserId INT = NULL 
AS
BEGIN
    IF NOT EXISTS(SELECT * FROM ForumAppSchema.Users WHERE UserId = @UserId)
    BEGIN
        IF NOT EXISTS (SELECT * FROM ForumAppSchema.Users WHERE Email = @Email) AND
            NOT EXISTS (SELECT * FROM ForumAppSchema.Users WHERE Username = @Username)
        BEGIN
            DECLARE @OutputUserId INT

            INSERT INTO ForumAppSchema.Users (
                [Username],
                [Email],
                [IsActive],
                [CreatedAt],
                [UpdatedAt]
            ) VALUES (
                @Username,
                @Email,
                @IsActive,
                SYSUTCDATETIME(),
                SYSUTCDATETIME()
            )
        END
    END
    ELSE
    BEGIN
        UPDATE ForumAppSchema.Users
            SET Username = @Username,
                Email = @Email,
                IsActive = @IsActive,
                UpdatedAt = SYSUTCDATETIME(),
                DeactivatedAt = @DeactivatedAt
            WHERE UserId = @UserId
    END
END
GO