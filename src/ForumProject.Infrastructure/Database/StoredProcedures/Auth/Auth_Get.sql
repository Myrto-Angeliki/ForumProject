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