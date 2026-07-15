USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spComment_Upsert
    @UserId INT,
    @PostId INT,
    @Content NVARCHAR(MAX),
    @CommentId INT = NULL
AS
BEGIN
    IF NOT EXISTS(SELECT * FROM ForumAppSchema.Comments WHERE CommentId = @CommentId)
    BEGIN

        INSERT INTO ForumAppSchema.Comments(
            [UserId],
            [PostId]
            [Content],
            [CreatedAt],
            [UpdatedAt]
        ) VALUES (
            @UserId,
            @PostId,
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
