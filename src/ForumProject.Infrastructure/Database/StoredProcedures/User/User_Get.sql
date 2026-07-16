USE ForumDatabase
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_Get
    @UserId INT = NULL,
    @Email NVARCHAR(50) = NULL,
    @Username NVARCHAR(50) = NULL
AS 
BEGIN
    SELECT  [Users].[UserId],
            [Username],
            [Email],
            [IsActive],
            [CreatedAt],
            [UpdatedAt],
            [DeactivatedAt]
    FROM ForumAppSchema.Users AS Users
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND Email = ISNULL(@Email, Email)
        AND Username = ISNULL(@Username, Username)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_GetPosts
    @UserId INT = NULL,
    @IsActive INT = NULL
AS
BEGIN
    SELECT  [Posts].[PostId],
            [Posts].[UserId],
            [Posts].[Title],
            [Posts].[Content],
            [Posts].[FeaturedImage],
            [Posts].[CreatedAt],
            [Posts].[UpdatedAt]
    FROM ForumAppSchema.Users AS Users
    INNER JOIN ForumAppSchema.Posts AS Posts
        ON Posts.UserId = Users.UserId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.IsActive, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_GetTopics
    @UserId INT = NULL,
    @IsActive INT = NULL
AS
BEGIN
    SELECT  [Users].[UserId],
            [Topics].[TopicId],
            [TopicName],
            [Topics].[CreatedAt],
            [Topics].[UpdatedAt]
    FROM ForumAppSchema.Users AS Users
    INNER JOIN ForumAppSchema.Users_Topics AS FollowingTopics
        ON FollowingTopics.UserId = Users.UserId
    INNER JOIN ForumAppSchema.Topics AS Topics
        ON Topics.TopicId = FollowingTopics.TopicId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.IsActive, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_GetComments
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
        AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.IsActive, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_GetFriendRequests
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
            AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.IsActive, 0)
    END
    ELSE
    BEGIN
        SELECT  [SenderId],
                [RecipientId]
        FROM ForumAppSchema.Users AS Users
        INNER JOIN ForumAppSchema.FriendRequests AS FriendRequestsReceived
            ON FriendRequestsReceived.RecipientId = Users.UserId
        WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
            AND ISNULL(Users.IsActive, 0) = COALESCE(@IsActive, Users.IsActive, 0)
    END
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_GetFriends
    @UserId INT = NULL,
    @IsActive INT = NULL
AS 
BEGIN
    SELECT  [Users2].[UserId] AS FriendId,
            [Users2].[Username] AS FriendUsername,
            [Users2].[Email] AS FriendEmail,
            [Users2].[IsActive] AS FriendIsActive,
            [Users2].[CreatedAt] AS FriendCreatedAt,
            [Users2].[UpdatedAt] AS FriendUpdatedAt,
            [Users2].[DeactivatedAt] AS FriendDeactivatedAt
    FROM ForumAppSchema.Users AS Users1
    INNER JOIN ForumAppSchema.Friends AS Friends
        ON Friends.UserId1 = Users1.UserId
    INNER JOIN ForumAppSchema.Users AS Users2
        ON Users2.UserId = Friends.UserId2
    WHERE Users1.UserId = ISNULL(@UserId, Users1.UserId)
        AND ISNULL(Users1.IsActive, 0) = COALESCE(@IsActive, Users1.IsActive, 0)
    UNION
    SELECT [Users2].[UserId] AS FriendId,
            [Users2].[Username] AS FriendUsername,
            [Users2].[Email] AS FriendEmail,
            [Users2].[IsActive] AS FriendIsActive,
            [Users2].[CreatedAt] AS FriendCreatedAt,
            [Users2].[UpdatedAt] AS FriendUpdatedAt,
            [Users2].[DeactivatedAt] AS FriendDeactivatedAt
    FROM ForumAppSchema.Users AS Users1
    INNER JOIN ForumAppSchema.Friends AS Friends
        ON Friends.UserId2 = Users1.UserId
    INNER JOIN ForumAppSchema.Users AS Users2
        ON Users2.UserId = Friends.UserId1
    WHERE Users1.UserId = ISNULL(@UserId, Users1.UserId)
        AND ISNULL(Users1.IsActive, 0) = COALESCE(@IsActive, Users1.IsActive, 0)
END
GO