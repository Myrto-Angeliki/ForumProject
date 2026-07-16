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