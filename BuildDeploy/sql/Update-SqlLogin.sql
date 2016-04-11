IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'$(Username)')
BEGIN    
        PRINT 'CREATING LOGIN $(Username) WITH PASSWORD $(Password)'
        CREATE LOGIN [$(Username)] WITH PASSWORD = '$(Password)'
END