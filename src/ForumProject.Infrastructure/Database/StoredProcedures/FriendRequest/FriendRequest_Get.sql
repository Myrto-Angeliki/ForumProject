USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spFriendRequest_Get
    @SenderId INT = NULL,
    @RecipientId INT = NULL
AS
BEGIN
    SELECT  [Users1].[UserId] AS SenderUId,
            [Users1].[Username] AS SenderUsername,
            [Users1].[Email] AS SenderEmail,
            [Users1].[IsActive] AS SenderIsActive,
            [Users1].[CreatedAt] AS SenderCreatedAt,
            [Users1].[UpdatedAt] AS SenderUpdatedAt,
            [Users1].[DeactivatedAt] AS SenderDeactivatedAt,
            [Users2].[UserId] AS RecipientUId,
            [Users2].[Username] AS RecipientUsername,
            [Users2].[Email] AS RecipientEmail,
            [Users2].[IsActive] AS RecipientIsActive,
            [Users2].[CreatedAt] AS RecipientCreatedAt,
            [Users2].[UpdatedAt] AS RecipientUpdatedAt,
            [Users2].[DeactivatedAt]  AS RecipientDeactivatedAt 
    FROM ForumAppSchema.FriendRequests AS FrRequests
    INNER JOIN ForumAppSchema.Users AS Users1
        ON Users1.UserId = FrRequests.SenderId
    INNER JOIN ForumAppSchema.Users AS Users2
        ON Users2.UserId = FrRequests.RecipientId
    WHERE SenderId = ISNULL(@SenderId, SenderId)
     AND RecipientId = ISNULL(@RecipientId, RecipientId)
END
GO
