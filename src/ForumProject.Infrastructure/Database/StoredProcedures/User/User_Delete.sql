CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_Delete
    @UserIdParam INT = NULL,
    @EmailParam NVARCHAR(50) = NULL,
    @UsernameParam NVARCHAR(100) = NULL
AS
BEGIN
    DECLARE @ToDeleteEmail NVARCHAR(50)  = @EmailParam;
    DECLARE @ToDeleteUserId INT = @UserIdParam;

    IF @EmailParam IS NULL 
        AND (@UserIdParam IS NOT NULL) OR (@UsernameParam IS NOT NULL)
    BEGIN
        SELECT  @ToDeleteEmail = Users.Email
            FROM  ForumAppSchema.Users AS Users
        WHERE  Users.UserId = ISNULL(@UserIdParam, Users.UserId)
            AND Users.Username = ISNULL(@UsernameParam, Users.Username);
    END

    IF @UserIdParam IS NULL 
        AND COALESCE(@EmailParam, @UsernameParam) IS NOT NULL
    BEGIN
        SELECT  @ToDeleteUserId = Users.UserId
            FROM  ForumAppSchema.Users AS Users
        WHERE  Users.Email = ISNULL(@EmailParam, Users.Email)
            AND Users.Username = ISNULL(@UsernameParam, Users.Username);
    END

    EXEC ForumAppSchema.spComment_Delete  @UserId=@ToDeleteUserId;
    EXEC ForumAppSchema.spPost_Delete @UserIdDel=@ToDeleteUserId;
    EXEC ForumAppSchema.spFriendRequest_Delete @SenderId=@ToDeleteUserId;
    EXEC ForumAppSchema.spFriendRequest_Delete @RecipientId=@ToDeleteUserId;

    DELETE FROM ForumAppSchema.Friends
    WHERE UserId1 = ISNULL(@ToDeleteUserId, UserId1) OR UserId2 = ISNULL(@ToDeleteUserId, UserId2);

    DELETE FROM ForumAppSchema.Users_Topics 
    WHERE UserId = ISNULL(@ToDeleteUserId, UserId);

    DELETE  FROM ForumAppSchema.Users
    WHERE  UserId = ISNULL(@ToDeleteUserId, UserId);

    DELETE  FROM ForumAppSchema.Auth
    WHERE  Auth.Email = ISNULL(@ToDeleteEmail, Auth.Email);
END;
GO