USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spPost_Upsert
    @UserId INT,
    @Title NVARCHAR(200),
    @Content NVARCHAR(MAX),
    @FeaturedImage NVARCHAR(200),
    @PostId INT = NULL
AS
BEGIN
    IF NOT EXISTS(SELECT * FROM ForumAppSchema.Posts WHERE PostId = @PostId)
    BEGIN
        IF NOT EXISTS(SELECT * FROM ForumAppSchema.Posts WHERE Title = @Title)
        BEGIN
            INSERT INTO ForumAppSchema.Posts(
                [UserId],
                [Title],
                [Content],
                [FeaturedImage],
                [CreatedAt],
                [UpdatedAt]
            ) VALUES (
                @UserId,
                @Title,
                @Content,
                @FeaturedImage,
                SYSUTCDATETIME(),
                SYSUTCDATETIME()
            )
        END
    END
    ELSE
    BEGIN
        UPDATE ForumAppSchema.Posts
            SET 
                Title = @Title,
                Content = @Content,
                FeaturedImage = @FeaturedImage,
                UpdatedAt = SYSUTCDATETIME()
            WHERE PostId = @PostId
    END
END
GO