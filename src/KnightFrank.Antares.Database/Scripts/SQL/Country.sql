
CREATE TABLE #TempCountry (
	[IsoCode] NVARCHAR (2) NULL
);

ALTER TABLE Country NOCHECK CONSTRAINT ALL

BULK INSERT #TempCountry
    FROM '$(OutputPath)\Scripts\Data\Configuration\Country.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.Country AS T
	USING #TempCountry AS S	
	ON 
	(
        (T.IsoCode = S.IsoCode)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[IsoCode] = S.[IsoCode]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([IsoCode])
		VALUES ([IsoCode])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Country WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempCountry
