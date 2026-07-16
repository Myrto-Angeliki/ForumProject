CREATE OR ALTER PROCEDURE ForumAppSchema.spLoginConfirmation_Get
    @Email NVARCHAR(50) = NULL
AS
BEGIN
    SELECT  [PasswordHash],
            [PasswordSalt] 
    FROM ForumAppSchema.Auth AS Auth
        WHERE Auth.Email = ISNULL(@Email, Auth.Email)
END;
GO