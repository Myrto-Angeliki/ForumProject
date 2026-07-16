USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spPost_Get
    @PostId INT = NULL,
    @UserId INT = NULL
AS
BEGIN
    SELECT * FROM ForumAppSchema.Posts
    WHERE PostId = ISNULL(@PostId, PostId)
        and UserId = ISNULL(@UserId, UserId)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spPost_GetComments
    @PostId INT,
    @CommentId INT = NULL
AS
BEGIN
    SELECT * FROM ForumAppSchema.Comments
    WHERE PostId = @PostId
        and CommentId = ISNULL(@CommentId, CommentId)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spPost_GetTopics
    @PostId INT,
    @TopicId INT = NULL
AS
BEGIN
    SELECT  [Topics].[TopicId],
            [Topics].[TopicName],
            [Topics].[CreatedAt],
            [Topics].[UpdatedAt]
    FROM ForumAppSchema.Posts as Posts
    INNER JOIN ForumAppSchema.Posts_Topics AS PT
        ON PT.PostId = Posts.PostId
    INNER JOIN ForumAppSchema.Topics AS Topics
        ON Topics.TopicId = PT.TopicId
    WHERE Posts.PostId = @PostId
        AND Topics.TopicId = ISNULL(@TopicId, Topics.TopicId)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spTopic_Get
    @TopicId INT = NULL
AS
BEGIN
    SELECT * FROM ForumAppSchema.Topics
    WHERE TopicId = ISNULL(@TopicId, TopicId)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spTopic_GetPosts
    @TopicId INT,
    @PostId INT = NULL
AS
BEGIN
    SELECT  [Posts].[PostId],
            [Posts].[UserId],
            [Posts].[Title],
            [Posts].[Content],
            [Posts].[FeaturedImage],
            [Posts].[CreatedAt],
            [Posts].[UpdatedAt]
    FROM ForumAppSchema.Topics AS Topics
    INNER JOIN ForumAppSchema.Posts_Topics AS PT
        ON PT.TopicId = Topics.TopicId
    INNER JOIN ForumAppSchema.Posts AS Posts
        ON Posts.PostId = PT.PostId
    WHERE Topics.TopicId = @TopicId
        AND Posts.PostId = ISNULL(@PostId, Posts.PostId)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spTopic_GetUsers
    @TopicId INT,
    @UserId INT = NULL
AS
BEGIN
    SELECT  [Users].[UserId],
            [Users].[Username],
            [Users].[Email],
            [Users].[IsActive],
            [Users].[CreatedAt],
            [Users].[UpdatedAt],
            [Users].[DeactivatedAt]
    FROM ForumAppSchema.Topics AS Topics
    INNER JOIN ForumAppSchema.Users_Topics AS UT
        ON UT.TopicId = Topics.TopicId
    INNER JOIN ForumAppSchema.Users AS Users
        ON Users.UserId = UT.UserId
    WHERE Topics.TopicId = @TopicId
        AND Users.UserId = ISNULL(@UserId, Users.UserId)
END