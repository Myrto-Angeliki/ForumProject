USE ForumDatabase
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_Get
    @UserId INT = NULL
AS 
BEGIN
    SELECT  [UserId]
            [Username],
            [Email],
            [IsActive],
            [CreatedAt],
            [UpdatedAt],
            [DeactivatedAt]
    FROM ForumAppSchema.Users AS Users
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_GetPosts
    @UserId INT = NULL,
    @IsActive INT = NULL
AS
BEGIN
    SELECT  [Posts].[PostId],
            [Users].[UserId],
            [Posts].[Title],
            [Posts].[Content],
            [Posts].[FeaturedImage],
            [Posts].[CreatedAt],
            [Posts].[UpdatedAt]
    FROM ForumAppSchema.Users AS Users
    INNER JOIN ForumAppSchema.Posts AS Posts
        ON Posts.UserId = Users.UserId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.Active, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_GetTopics
    @UserId INT = NULL,
    @IsActive INT = NULL
AS
BEGIN
    SELECT  [Topics].[TopicId],
            [Users].[UserId],
            [TopicName],
            [Topics].[CreatedAt],
            [Topics].[UpdatedAt]
    FROM ForumAppSchema.Users AS Users
    INNER JOIN ForumAppSchema.Users_Topics AS FollowingTopics
        ON FollowingTopics.UserId = Users.UserId
    INNER JOIN ForumAppSchema.Topics AS Topics
        ON Topics.TopicId = FollowingTopics.TopicId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.Active, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_GetComments
    @UserId INT = NULL,
    @IsActive INT = NULL
AS
BEGIN
    SELECT  [Comments].[CommentId],
            [Comments].[UserId],
            [Comments].[PostId],
            [Comments].[Content],
            [Comments].[CreatedAt],
            [Comments].[UpdatedAt]
    FROM ForumAppSchema.Users AS Users
    INNER JOIN ForumAppSchema.Comments AS Comments
        ON Comments.UserId = Users.UserId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.Active, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_GetFriendRequests
    @UserId INT = NULL,
    @IsActive INT = NULL,
    @IsSender BIT = 1
AS
BEGIN
    IF(@IsSender = 1)
    BEGIN
        SELECT  [SenderId],
                [RecipientId]
        FROM ForumAppSchema.Users AS Users
        INNER JOIN ForumAppSchema.FriendRequests AS FriendRequestsSent
            ON FriendRequestsSent.SenderId = Users.UserId
        WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
            AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.Active, 0)
    END
    ELSE
    BEGIN
        SELECT  [SenderId],
                [RecipientId]
        FROM ForumAppSchema.Users AS Users
        INNER JOIN ForumAppSchema.FriendRequests AS FriendRequestsReceived
            ON FriendRequestsReceived.RecipientId = Users.UserId
        WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
            AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.Active, 0)
    END
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_GetFriends
    @UserId INT = NULL,
    @IsActive INT = NULL
AS 
BEGIN
    SELECT [Friends1].[UserId2]
    FROM ForumAppSchema.Users AS Users
    INNER JOIN ForumAppSchema.Friends AS Friends1
        ON Friends1.UserId1 = Users.UserId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.Active, 0)
    UNION ALL
    SELECT [Friends2].[UserId1]
    FROM ForumAppSchema.Users AS Users
    INNER JOIN ForumAppSchema.Friends AS Friends2
        ON Friends2.UserId2 = Users.UserId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.Active, 0)
END
GO