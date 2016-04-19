
CREATE TABLE #TempPropertyTypeLocalised (
	[PropertyTypeCode] NVARCHAR (50) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NOT NULL
);

ALTER TABLE PropertyTypeLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempPropertyTypeLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\PropertyTypeLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.PropertyTypeLocalised AS T
	USING 
	(
		SELECT 
		P.Id AS PropertyTypeId,
		L.Id AS LocaleId,
		[Value]
		FROM TempPropertyTypeLocalised Temp
		JOIN Locale L ON L.IsoCode = Temp.LocaleCode
		JOIN PropertyType P ON P.Code = Temp.PropertyTypeCode
	)
	AS S	
	ON 
	(
        (T.[PropertyTypeId] = S.[PropertyTypeId] AND T.[LocaleId] = S.[LocaleId])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[PropertyTypeId] = S.[PropertyTypeId],
		T.[LocaleId] = S.[LocaleId],
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([PropertyTypeId], [LocaleId], [Value])
		VALUES ([PropertyTypeId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyTypeLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyTypeLocalised
