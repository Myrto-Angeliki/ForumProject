-- 001_InitialSchema.sql

CREATE DATABASE ForumDatabase;
GO

USE ForumDatabase;
GO

CREATE SCHEMA ForumAppSchema;
GO

CREATE TABLE ForumAppSchema.Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    Email NVARCHAR(200) UNIQUE NOT NULL,
    IsActive BIT,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    DeactivatedAt DATETIME2 NULL 
);
GO

CREATE TABLE ForumAppSchema.Friends
(
    UserId1 INT NOT NULL,
    UserId2 INT NOT NULL,

    CONSTRAINT PK_Friends
        PRIMARY KEY (UserId1, UserId2),

    CONSTRAINT CK_Friends_UserOrder
        CHECK (UserId1 < UserId2),

    CONSTRAINT FK_Users1_Users2
        FOREIGN KEY (UserId2)
        REFERENCES ForumAppSchema.Users(UserId),

    CONSTRAINT FK_Users2_Users1
        FOREIGN KEY (UserId1)
        REFERENCES ForumAppSchema.Users(UserId)
);
GO

CREATE TABLE ForumAppSchema.Posts
(
    PostId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NULL,
    FeaturedImage NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Posts_Users
        FOREIGN KEY (UserId)
        REFERENCES ForumAppSchema.Users(UserId)
);
GO

CREATE TABLE ForumAppSchema.Comments
(
    CommentId INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Comments_Posts
        FOREIGN KEY (PostId)
        REFERENCES ForumAppSchema.Posts(PostId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Comments_Users
        FOREIGN KEY (UserId)
        REFERENCES ForumAppSchema.Users(UserId)
);
GO

CREATE TABLE ForumAppSchema.Topics
(
    TopicId INT IDENTITY(1,1) PRIMARY KEY,
    TopicName NVARCHAR(200) UNIQUE,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

CREATE TABLE ForumAppSchema.Users_Topics
(
    UserId INT NOT NULL,
    TopicId INT NOT NULL,

    CONSTRAINT PK_Users_Topics
        PRIMARY KEY (UserId, TopicId),

    CONSTRAINT FK_Users_Topics
        FOREIGN KEY (TopicId)
        REFERENCES ForumAppSchema.Topics(TopicId),

    CONSTRAINT FK_Topics_Users
        FOREIGN KEY (UserId)
        REFERENCES ForumAppSchema.Users(UserId)
);
GO

CREATE TABLE ForumAppSchema.Posts_Topics
(
    PostId INT NOT NULL,
    TopicId INT NOT NULL,

    CONSTRAINT PK_Posts_Topics
        PRIMARY KEY (PostId, TopicId),

    CONSTRAINT FK_Posts_Topics
        FOREIGN KEY (TopicId)
        REFERENCES ForumAppSchema.Topics(TopicId),

    CONSTRAINT FK_Topics_Posts
        FOREIGN KEY (PostId)
        REFERENCES ForumAppSchema.Posts(PostId)
);
GO

CREATE TABLE ForumAppSchema.FriendRequests
(
    SenderId INT NOT NULL,
    RecipientId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT PK_FriendRequests
        PRIMARY KEY (SenderId, RecipientId),

    CONSTRAINT CK_Friends_NoSelfRequest
        CHECK (SenderId <> RecipientId),

    CONSTRAINT FK_FriendRequests_Sender
        FOREIGN KEY (SenderId)
        REFERENCES ForumAppSchema.Users(UserId),

    CONSTRAINT FK_FriendRequests_Recipient
        FOREIGN KEY (RecipientId)
        REFERENCES ForumAppSchema.Users(UserId)
);

CREATE INDEX IX_Posts_UserId
ON ForumAppSchema.Posts(UserId);
GO

CREATE INDEX IX_Comments_PostId
ON ForumAppSchema.Comments(PostId);
GO

CREATE INDEX IX_Comments_UserId
ON ForumAppSchema.Comments(UserId);
GO

CREATE INDEX IX_Friends_UserId2
ON ForumAppSchema.Friends(UserId2);
GO

CREATE INDEX IX_Users_Topics_TopicId
ON ForumAppSchema.Users_Topics(TopicId);
GO

CREATE INDEX IX_Posts_Topics_TopicId
ON ForumAppSchema.Posts_Topics(TopicId);
GO

CREATE INDEX IX_FriendRequests_RecipientId
ON ForumAppSchema.FriendRequests(RecipientId);
GO