CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_Upsert
    @Username NVARCHAR(50) = NULL,
    @Email NVARCHAR(50) = NULL,
    @IsActive BIT = 1,
    @DeactivatedAt DATETIME2 = NULL,
    @UserId INT = NULL 
AS
BEGIN
    IF NOT EXISTS(SELECT * FROM ForumAppSchema.Users WHERE UserId = @UserId)
    BEGIN
        IF NOT EXISTS (SELECT * FROM ForumAppSchema.Users 
            WHERE Email = ISNULL(@Email, Email)) AND
                NOT EXISTS (SELECT * FROM ForumAppSchema.Users 
                    WHERE Username = ISNULL(@Username, Username))
        BEGIN

            INSERT INTO ForumAppSchema.Users (
                [Username],
                [Email],
                [IsActive],
                [CreatedAt],
                [UpdatedAt]
            ) VALUES (
                @Username,
                @Email,
                1,
                SYSUTCDATETIME(),
                SYSUTCDATETIME()
            )
        END
    END
    ELSE
    BEGIN
        EXEC ForumAppSchema.spUser_UpdateUsername 
            @UserIdParam = @UserId, @UsernameParam = @Username;

        EXEC ForumAppSchema.spUser_UpdateEmail
            @UserIdParam = @UserId, @NewEmailParam = @Email;

        UPDATE ForumAppSchema.Users
            SET IsActive = @IsActive,
                UpdatedAt = SYSUTCDATETIME(),
                DeactivatedAt = @DeactivatedAt
            WHERE UserId = @UserId
    END
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_UpdateUsername
    @UserIdParam INT,
    @UsernameParam NVARCHAR(50)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM ForumAppSchema.Users 
        WHERE Username = ISNULL(@UsernameParam, Username))
    BEGIN
        UPDATE ForumAppSchema.Users
        SET Username = @UsernameParam
        WHERE UserId = @UserIdParam
    END
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_UpdateEmail
    @UserIdParam INT,
    @NewEmailParam NVARCHAR(50)
AS
BEGIN
    DECLARE @EmailToUpdate NVARCHAR(50);

    IF NOT EXISTS (SELECT * FROM ForumAppSchema.Users 
        WHERE Email = ISNULL(@NewEmailParam, Email))
    BEGIN
        SELECT  @EmailToUpdate = Users.Email
            FROM  ForumAppSchema.Users AS Users
        WHERE  Users.UserId = @UserIdParam

        UPDATE ForumAppSchema.Users
        SET Email = @NewEmailParam
        WHERE UserId = @UserIdParam

        EXEC ForumAppSchema.spAuth_UpdateEmail
            @CurrentEmail = @EmailToUpdate, @NewEmail = @NewEmailParam;
    END
END
GO


CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_UpsertPost
    @PostToUpsertUserId INT,
    @PostToUpsertTitle NVARCHAR(200),
    @PostToUpsertContent NVARCHAR(MAX),
    @PostToUpsertFeaturedImage NVARCHAR(200),
    @PostToUpsertPostId INT = NULL
AS
BEGIN
    EXEC ForumAppSchema.spPost_Upsert 
        @UserId = @PostToUpsertUserId,
        @Title = @PostToUpsertTitle,
        @Content = @PostToUpsertContent,
        @FeaturedImage = @PostToUpsertFeaturedImage,
        @PostId = @PostToUpsertPostId;
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_InsertTopicToPost
    @PostId INT,
    @TopicId INT
AS
BEGIN
    EXEC ForumAppSchema.spPost_InsertTopic
        @PostToAddTopicTo_Id = @PostId,
        @TopicToAddToPost_Id = @TopicId;
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_DeleteTopicFromPost
    @PostId INT,
    @TopicId INT
AS
BEGIN
    EXEC ForumAppSchema.spPost_DeleteTopic
        @PostToDeleteTopicFrom_Id = @PostId,
        @TopicToDeleteFromPost_Id = @TopicId;
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_DeletePost
    @PostId INT
AS
BEGIN
    EXEC ForumAppSchema.spPost_Delete @PostIdDel = @PostId;
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_UpsertComment
    @CommentToUpsertPostId INT,
    @CommentToUpsertUserId INT,
    @CommentToUpsertContent NVARCHAR(MAX),
    @CommentToUpsertCommentId INT = NULL
AS
BEGIN
    EXEC ForumAppSchema.spComment_Upsert 
        @PostId = @CommentToUpsertPostId,
        @UserId = @CommentToUpsertUserId,
        @Content = @CommentToUpsertContent,
        @CommentId = @CommentToUpsertCommentId
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_DeleteComment
    @CommentToDeleteCommentId INT
AS
BEGIN
    EXEC ForumAppSchema.spComment_Delete @CommentId = @CommentToDeleteCommentId ;
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_InsertFriendRequest
    @FriendRequestToInsertSenderId INT,
    @FriendRequestToInsertRecipientId INT
AS
BEGIN
    EXEC ForumAppSchema.spFriendRequest_Insert 
        @SenderId = @FriendRequestToInsertSenderId,
        @RecipientId = @FriendRequestToInsertRecipientId

END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_DeleteFriendRequest
    @FriendRequestToDeleteSenderId INT,
    @FriendRequestTodeleteRecipientId INT
AS
BEGIN
    EXEC ForumAppSchema.spFriendRequest_Delete
        @SenderId = @FriendRequestToDeleteSenderId,
        @RecipientId = @FriendRequestTodeleteRecipientId ;
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_InsertFriend
    @FriendshipMember1Id INT,
    @FriendshipMember2Id INT
AS
BEGIN
    INSERT INTO ForumAppSchema.Friends(
        UserId1,
        UserId2
    ) VALUES (
        @FriendshipMember1Id,
        @FriendshipMember2Id
    );
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_FollowTopic
    @FollowerId INT,
    @TopicToFollowId INT
AS
BEGIN
    INSERT INTO ForumAppSchema.Users_Topics(
        UserId,
        TopicId
    ) VALUES (
        @FollowerId,
        @TopicToFollowId
    );
END
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spUser_UnfollowTopic
    @FollowerId INT,
    @TopicToUnfollowId INT
AS
BEGIN
    DELETE FROM ForumAppSchema.Users_Topics
    WHERE UserId = @FollowerId
        AND TopicId = @TopicToUnfollowId;
END
GO