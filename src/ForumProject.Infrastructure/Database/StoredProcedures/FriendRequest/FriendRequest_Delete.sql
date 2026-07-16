USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spFriendRequest_Delete
    @SenderId INT = NULL,
    @RecipientId INT = NULL
AS
BEGIN
    DELETE FROM ForumAppSchema.FriendRequests
    WHERE SenderId = ISNULL(@SenderId, SenderId)
     AND RecipientId = ISNULL(@RecipientId, RecipientId)
END