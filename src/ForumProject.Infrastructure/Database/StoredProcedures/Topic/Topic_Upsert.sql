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
