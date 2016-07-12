CREATE TABLE #TempCompanyCategoryLocalised (
	[CompanyCategoryEnumCode] NVARCHAR (50) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NOT NULL
);

ALTER TABLE CompanyCategoryLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempCompanyCategoryLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\companyCategoryLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.CompanyCategoryLocalised AS T
	USING 
	(
		SELECT 
		P.Id AS CompanyCategoryId,
		L.Id AS LocaleId,
		[Value]
		FROM #TempCompanyCategoryLocalised Temp
		JOIN Locale L ON L.IsoCode = Temp.LocaleCode
		JOIN CompanyCategory P ON P.EnumCode = Temp.CompanyCategoryEnumCode
	)
	AS S	
	ON 
	(
        (T.[CompanyCategoryId] = S.[CompanyCategoryId] AND T.[LocaleId] = S.[LocaleId])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([CompanyCategoryId], [LocaleId], [Value])
		VALUES ([CompanyCategoryId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE CompanyCategoryLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempCompanyCategoryLocalised
