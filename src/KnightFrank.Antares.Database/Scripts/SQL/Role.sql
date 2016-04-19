
CREATE TABLE #TempRole (
	[Name] NVARCHAR (100) NOT NULL
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
        (T.[Name] = S.[Name])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Name] = S.[Name]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Name])
		VALUES ([Name])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Role WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempRole
