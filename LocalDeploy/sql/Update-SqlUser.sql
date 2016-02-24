IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = '$(Username)')
BEGIN
    RAISERROR ('Login name $(Username) does not exist', 11, 1);
    RETURN
END

USE [$(DatabaseName)]

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$(Username)')
BEGIN
    PRINT 'CREATING USER $(Username) FOR LOGIN $(Username)'
    CREATE USER [$(Username)] FOR LOGIN [$(Username)]
END
ELSE
BEGIN
    PRINT 'REMAPPING USER $(Username) TO LOGIN $(Username)'
    ALTER USER [$(Username)] WITH LOGIN = [$(Username)]
END
