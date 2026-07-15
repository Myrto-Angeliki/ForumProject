USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spComment_Upsert
    @PostId INT,
    @UserId INT,
    @Content NVARCHAR(MAX),
    @CommentId INT = NULL
AS
BEGIN
    IF NOT EXISTS(SELECT * FROM ForumAppSchema.Comments WHERE CommentId = @CommentId)
    BEGIN

        INSERT INTO ForumAppSchema.Comments(
            [PostId],
            [UserId],
            [Content],
            [CreatedAt],
            [UpdatedAt]
        ) VALUES (
            @PostId,
            @UserId,
            @Content,
            SYSUTCDATETIME(),
            SYSUTCDATETIME()
        )

    END
    ELSE
    BEGIN
        UPDATE ForumAppSchema.Comments
            SET 
                Content = @Content,
                UpdatedAt = SYSUTCDATETIME()
            WHERE CommentId = @CommentId
    END
END
GO
