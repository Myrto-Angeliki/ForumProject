USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spComment_Get
    @CommentId INT = NULL
AS
BEGIN
    SELECT * FROM ForumAppSchema.Comments
    WHERE CommentId = ISNULL(@CommentId, CommentId)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spComment_Delete
    @CommentId INT = NULL,
    @PostId INT = NULL,
    @UserId INT = NULL
AS
BEGIN
    SELECT * FROM ForumAppSchema.Comments
    WHERE CommentId = ISNULL(@CommentId, CommentId)
        AND PostId = ISNULL(@PostId, PostId)
        AND UserId = ISNULL(@UserId, UserId)
END