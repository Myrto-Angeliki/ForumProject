USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spFriendRequest_Insert
    @SenderId INT,
    @RecipientId INT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM ForumAppSchema.FriendRequests WHERE SenderId = @SenderId AND RecipientId=@RecipientId)
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
            @SenderId,
            @RecipientId,
            SYSUTCDATETIME(),
            SYSUTCDATETIME()
        )
    END
END
GO
