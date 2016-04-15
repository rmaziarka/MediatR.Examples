
CREATE TABLE #TempDepartment (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[Name] NVARCHAR (255) NULL ,
	[CountryId] UNIQUEIDENTIFIER  NOT NULL ,
);

ALTER TABLE Department NOCHECK CONSTRAINT ALL

BULK INSERT #TempDepartment
    FROM '$(OutputPath)\Scripts\Data\Configuration\Department.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.Department AS T
	USING #TempDepartment AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Name] = S.[Name],
		T.[CountryId] = S.[CountryId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [Name], [CountryId])
		VALUES ([Id], [Name], [CountryId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Department WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempDepartment
