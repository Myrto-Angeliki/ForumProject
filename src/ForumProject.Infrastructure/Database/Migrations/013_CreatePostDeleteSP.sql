USE ForumDatabase;
GO

CREATE OR ALTER PROCEDURE ForumAppSchema.spPost_Delete
    @PostIdDel INT = NULL,
    @UserIdDel INT = NULL
AS
BEGIN
    CREATE TABLE #Posts (
        PostId INT PRIMARY KEY
    ) ;

    INSERT INTO #Posts SELECT PostId FROM ForumAppSchema.Posts 
        WHERE PostId=ISNULL(@PostIdDel, PostId) 
            AND UserId=ISNULL(@UserIdDel, UserId)

    DELETE FROM ForumAppSchema.Comments 
    WHERE PostId IN (SELECT PostId FROM #Posts)

    DELETE FROM ForumAppSchema.Posts_Topics
    WHERE PostId IN (SELECT PostId FROM #Posts)

    DELETE FROM ForumAppSchema.Posts 
    WHERE PostId IN (SELECT PostId FROM #Posts)
END