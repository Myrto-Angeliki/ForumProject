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

CREATE OR ALTER PROCEDURE ForumAppSchema.spPost_InsertTopic
    @PostToAddTopicTo_Id INT,
    @TopicToAddToPost_Id INT
AS
BEGIN
    INSERT INTO ForumAppSchema.Posts_Topics(
        PostId,
        TopicId
    ) VALUES(
        @PostToAddTopicTo_Id,
        @TopicToAddToPost_Id
    );
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spPost_DeleteTopic
    @PostToDeleteTopicFrom_Id INT,
    @TopicToDeleteFromPost_Id INT
AS
BEGIN
    DELETE FROM ForumAppSchema.Posts_Topics
    WHERE PostId = @PostToDeleteTopicFrom_Id
        AND TopicId =  @TopicToDeleteFromPost_Id;
END
GO