USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spComment_Get
    @CommentId INT = NULL
AS
BEGIN
    SELECT * FROM ForumAppSchema.Comments
    WHERE CommentId = ISNULL(@CommentId, CommentId)
END