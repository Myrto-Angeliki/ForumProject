USE ForumDatabase
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_Delete
    @ToDeleteUserId INT = NULL,
    @ToDeleteEmail INT = NULL,
    @ToDeleteUsername INT = NULL
AS
BEGIN
    DECLARE @Email NVARCHAR(50) = @ToDeleteEmail;

    IF(@ToDeleteEmail = NULL AND (@ToDeleteUserId <> NULL OR @ToDeleteUsername <> NULL))
    BEGIN
        SELECT  @Email = Users.Email
            FROM  ForumAppSchema.Users AS Users
        WHERE  Users.UserId = ISNULL(@ToDeleteUserId, Users.UserId)
            AND Users.Username = ISNULL(@ToDeleteUsername, Users.Username);
    END

    EXEC ForumAppSchema.spComment_Delete  @UserId=@ToDeleteUserId;
    EXEC ForumAppSchema.spPost_Delete @UserIdDel=@ToDeleteUserId;
    EXEC ForumAppSchema.spFriendRequest_Delete @SenderId=@ToDeleteUserId;
    EXEC ForumAppSchema.spFriendRequest_Delete @RecipientId=@ToDeleteUserId;

    DELETE FROM ForumAppSchema.Friends
    WHERE UserId1 = ISNULL(@ToDeleteUserId, UserId1) OR UserId2 = ISNULL(@ToDeleteUserId, UserId2);

    DELETE FROM ForumAppSchema.Users_Topics 
    WHERE UserId = @ToDeleteUserId;

    DELETE  FROM ForumAppSchema.Users
    WHERE  UserId = @ToDeleteUserId;

    DELETE  FROM ForumAppSchema.Auth
    WHERE  Auth.Email = ISNULL(@Email, Auth.Email);
END;
GO