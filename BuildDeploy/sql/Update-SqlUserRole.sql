USE [$(DatabaseName)]
IF (IS_ROLEMEMBER('$(Role)', '$(Username)') = 0)
BEGIN
    PRINT 'Adding user $(Username) to database role $(Role).'
    EXEC sp_addrolemember @rolename = '$(Role)', @membername = '$(Username)'

    --This is not ok for SQL Server 2008
    --ALTER ROLE [$(Role)] ADD MEMBER [$(Username)]
END
ELSE
BEGIN
    PRINT 'User $(Username) already has database role $(Role).'
END