
CREATE TABLE #TempRole (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[Name] NVARCHAR (100) NULL ,
);

ALTER TABLE Role NOCHECK CONSTRAINT ALL

BULK INSERT #TempRole
    FROM '$(OutputPath)\Scripts\Data\Configuration\Role.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.Role AS T
	USING #TempRole AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Name] = S.[Name]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [Name])
		VALUES ([Id], [Name])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Role WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempRole
