IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'$(Username)')
BEGIN    
        PRINT 'CREATING USER $(Username) WITHOUT PASSWORD'
        CREATE LOGIN [$(Username)] FROM WINDOWS
END