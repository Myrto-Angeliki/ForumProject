USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spFriendRequest_Insert
    @SenderID INT,
    @RecipientId INT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM ForumAppSchema.FriendRequests WHERE SenderID = @SenderID AND RecipientId=@RecipientId)
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
END
GO
