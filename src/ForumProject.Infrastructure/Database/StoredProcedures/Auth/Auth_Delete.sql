CREATE OR ALTER PROCEDURE ForumAppSchema.spRegistration_Delete
    @Email NVARCHAR(50) = NULL
AS
BEGIN
    DELETE  FROM ForumAppSchema.Auth
    WHERE  Auth.Email = ISNULL(@Email, Auth.Email);
END
GO