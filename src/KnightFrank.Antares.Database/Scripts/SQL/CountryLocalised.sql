
CREATE TABLE #TempCountryLocalised (
	[CountryCode] NVARCHAR (2) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NULL
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
	USING 
	(
		SELECT C.Id AS CountryId, L.Id AS LocaleId, CL.Value
		FROM #TempCountryLocalised CL
		JOIN Country C ON CL.CountryCode = C.IsoCode
		JOIN Locale L ON CL.LocaleCode = L.IsoCode
	)	
	AS S	
	ON 
	(
        (T.CountryId = S.CountryId AND T.LocaleId = S.LocaleId)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[CountryId] = S.[CountryId],
		T.[LocaleId] = S.[LocaleId],
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([CountryId], [LocaleId], [Value])
		VALUES ([CountryId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE CountryLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempCountryLocalised
