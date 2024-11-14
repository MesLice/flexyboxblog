PRINT 'Starting database restoration process...';

IF DB_ID('BlogPostsDb') IS NULL
BEGIN
    PRINT 'BlogPostsDb does not exist. Attempting to restore...';
    BEGIN TRY
        RESTORE DATABASE BlogPostsDb
        FROM DISK = '/var/opt/mssql/backup/BlogPostsDb.bak'
        WITH MOVE 'BlogPostsDb' TO '/var/opt/mssql/data/BlogPostsDb.mdf',
             MOVE 'BlogPostsDb_Log' TO '/var/opt/mssql/data/BlogPostsDb.ldf',
             REPLACE;
        PRINT 'Database BlogPostsDb restored successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred during database restoration:';
        PRINT 'Message: ' + ERROR_MESSAGE();
        PRINT 'Severity: ' + CAST(ERROR_SEVERITY() AS NVARCHAR(10));
        PRINT 'State: ' + CAST(ERROR_STATE() AS NVARCHAR(10));
        PRINT 'Line: ' + CAST(ERROR_LINE() AS NVARCHAR(10));
    END CATCH
END
ELSE
BEGIN
    PRINT 'Database BlogPostsDb already exists. Skipping restore.';
END;