
CREATE TABLE #TempCountryLocalised (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[CountryId] UNIQUEIDENTIFIER  NOT NULL ,
	[LocaleId] UNIQUEIDENTIFIER  NOT NULL ,
	[Value] NVARCHAR (100) NULL ,
);

ALTER TABLE CountryLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempCountryLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\CountryLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.CountryLocalised AS T
	USING #TempCountryLocalised AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[CountryId] = S.[CountryId],
		T.[LocaleId] = S.[LocaleId],
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [CountryId], [LocaleId], [Value])
		VALUES ([Id], [CountryId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE CountryLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempCountryLocalised
