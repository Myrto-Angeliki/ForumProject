USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spTopic_Upsert
    @TopicName NVARCHAR(200),
    @TopicId INT = NULL
AS
BEGIN
    IF NOT EXISTS(SELECT * FROM ForumAppSchema.Topics WHERE TopicId = @TopicId)
    BEGIN
        IF NOT EXISTS(SELECT * FROM ForumAppSchema.Topics WHERE TopicName = @TopicName)
        BEGIN
            INSERT INTO ForumAppSchema.Topics(
                [TopicName],
                [CreatedAt],
                [UpdatedAt]
            ) VALUES (
                @TopicName,
                SYSUTCDATETIME(),
                SYSUTCDATETIME()
            )
        END
    END
    ELSE
    BEGIN
        UPDATE ForumAppSchema.Topics
            SET 
                TopicName = @TopicName,
                UpdatedAt = SYSUTCDATETIME()
            WHERE TopicId = @TopicId
    END
END
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
        IF NOT EXISTS(SELECT * FROM ForumAppSchema.Posts WHERE @Title = @Title)
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
            [PostId],
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

CREATE OR ALTER PROCEDURE ForumAppSchema.spFriendRequest_Insert
    @SenderID INT,
    @RecipientId INT
AS
BEGIN
    INSERT INTO ForumAppSchema.FriendRequests
    (
        [SenderId],
        [RecipientId],
        [CreatedAt],
        [UpdatedAt]
    )
    VALUES
    (
        @SenderID,
        @RecipientId,
        SYSUTCDATETIME(),
        SYSUTCDATETIME()
    )
END
GO