USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spFriendRequest_Get
    @SenderId INT = NULL,
    @RecipientId INT = NULL
AS
BEGIN
    SELECT * FROM ForumAppSchema.FriendRequests
    WHERE SenderId = ISNULL(@SenderId, SenderId)
     AND RecipientId = ISNULL(@RecipientId, RecipientId)
END