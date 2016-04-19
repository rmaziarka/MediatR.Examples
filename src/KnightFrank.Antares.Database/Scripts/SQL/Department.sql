
CREATE TABLE #TempDepartment (
	[Name] NVARCHAR (255) NULL,
	[CountryCode] NVARCHAR (2) NOT NULL
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
	USING (
		SELECT C.Id AS CountryId, Temp.Name
		FROM #TempDepartment Temp
		JOIN Country C ON Temp.CountryCode = C.IsoCode
	)
	AS S	
	ON 
	(
        (T.CountryId = S.CountryId AND T.Name = S.Name)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Name] = S.[Name],
		T.[CountryId] = S.[CountryId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Name], [CountryId])
		VALUES ([Name], [CountryId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Department WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempDepartment
