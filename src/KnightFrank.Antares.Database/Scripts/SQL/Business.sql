
CREATE TABLE #TempBusiness (
	[Name] NVARCHAR (100) NULL,
	[CountryCode] NVARCHAR (2) NOT NULL
);

ALTER TABLE Business NOCHECK CONSTRAINT ALL

BULK INSERT #TempBusiness
    FROM '$(OutputPath)\Scripts\Data\Configuration\Business.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.Business AS T
	USING (
		SELECT C.Id AS CountryId, Temp.Name
		FROM #TempBusiness Temp
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
    
ALTER TABLE Business WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempBusiness
