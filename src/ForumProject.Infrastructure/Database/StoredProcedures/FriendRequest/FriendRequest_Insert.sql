USE ForumDatabase;
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
