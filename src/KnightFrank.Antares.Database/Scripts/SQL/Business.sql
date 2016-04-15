
CREATE TABLE #TempBusiness (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[Name] NVARCHAR (100) NULL ,
	[CountryId] UNIQUEIDENTIFIER  NOT NULL ,
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
	USING #TempBusiness AS S	
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
    
ALTER TABLE Business WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempBusiness
