
CREATE TABLE #TempRoleUser (
	[Login] NVARCHAR (40) NOT NULL,
	[Role] NVARCHAR (100) NOT NULL
);

ALTER TABLE RoleUser NOCHECK CONSTRAINT ALL

BULK INSERT #TempRoleUser
    FROM '$(OutputPath)\Scripts\Data\Configuration\RoleUser.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.RoleUser AS T
	USING
	(
		SELECT 
		U.Id AS UserId,
		R.Id AS RoleId
		FROM #TempRoleUser temp
		JOIN [User] U ON U.ActiveDirectoryLogin = temp.[Login]
		JOIN [Role] R ON R.Name = temp.[Role]
	)
	AS S	
	ON 
	(
        (T.RoleId = S.RoleId AND T.UserId = S.UserId)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.RoleId = S.RoleId,
		T.UserId = S.UserId

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (RoleId, UserId)
		VALUES (RoleId, UserId)

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE RoleUser WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempRoleUser
