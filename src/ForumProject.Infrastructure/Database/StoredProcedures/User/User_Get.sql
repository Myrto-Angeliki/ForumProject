USE ForumDatabase
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_Friends_Get
    @UserId INT = NULL,
    @isActive BIT = NULL
AS
BEGIN
    DROP TABLE IF EXISTS #UserFriends
    CREATE TABLE #UserFriends
    (
        FriendId INT NOT NULL PRIMARY KEY
    );
    INSERT INTO #UserFriends (FriendId)
    SELECT UserId2
    FROM ForumAppSchema.Friends
    WHERE UserId1 = ISNULL(@UserId, UserId1)
    UNION
    SELECT UserId1
    FROM ForumAppSchema.Friends
    WHERE UserId2 = ISNULL(@UserId, UserId2);

    SELECT * 
    FROM ForumAppSchema.Users AS Users
        JOIN #UserFriends AS UserFriends
            ON UserFriends.FriendId = Users.UserId
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
            AND ISNULL(Users.isActive, 0) = COALESCE(@isActive, Users.isActive, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_FriendRequestsSent_Get
    @UserId INT,
    @isActive BIT = NULL
AS
BEGIN
    SELECT  [FriendRequests].[SenderId] AS SenderId,
            [FriendRequests].[RecipientId] AS RecipientId
    FROM ForumAppSchema.Users AS Users
        JOIN ForumAppSchema.FriendRequests AS FriendRequests
            ON FriendRequests.SenderId = Users.UserId 
    WHERE Users.UserId = @UserId
            AND ISNULL(Users.isActive, 0) = COALESCE(@isActive, Users.isActive, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_FriendRequestsReceived_Get
    @UserId INT,
    @isActive BIT = NULL
AS
BEGIN
    SELECT  [FriendRequests].[SenderId] AS SenderId,
            [FriendRequests].[RecipientId] AS RecipientId
    FROM ForumAppSchema.Users AS Users
        JOIN ForumAppSchema.FriendRequests AS FriendRequests
            ON FriendRequests.RecipientId = Users.UserId 
    WHERE Users.UserId = @UserId
            AND ISNULL(Users.isActive, 0) = COALESCE(@isActive, Users.isActive, 0)
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUsers_Get
/* */
    @UserId INT = NULL,
    @isActive BIT = NULL
AS
BEGIN


    DROP TABLE IF EXISTS #UserFriendRequestsReceived
    CREATE TABLE #UserFriendRequestsReceived
    (
        SenderId INT NOT NULL PRIMARY KEY
    );
    INSERT INTO #UserFriendRequestsReceived (SenderId)
    SELECT SenderId
    FROM ForumAppSchema.FriendRequests AS FriendRequests
    WHERE RecipientId = ISNULL(@UserId, RecipientId)
END