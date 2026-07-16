USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spTopic_Delete
    @TopicId INT = NULL
AS
BEGIN

    DELETE FROM ForumAppSchema.Posts_Topics 
    WHERE TopicId = ISNULL(@TopicId, TopicId)

    DELETE FROM ForumAppSchema.Users_Topics 
    WHERE TopicId = ISNULL(@TopicId, TopicId)

    DELETE FROM ForumAppSchema.Topics
    WHERE TopicId = ISNULL(@TopicId, TopicId)

END