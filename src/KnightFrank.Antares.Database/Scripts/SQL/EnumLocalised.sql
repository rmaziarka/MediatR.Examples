
CREATE TABLE #TempEnumLocalised (
	[EnumTypeCode] NVARCHAR (25) NOT NULL,
	[EnumTypeItemCode] NVARCHAR (40) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NULL
);

ALTER TABLE EnumLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempEnumLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\EnumLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.EnumLocalised AS T
	USING 
	(
		SELECT 
			I.Id AS EnumTypeItemId,
			L.Id AS LocaleId,
			Temp.Value
		FROM #TempEnumLocalised Temp
		JOIN Locale L ON L.IsoCode = Temp.LocaleCode
		JOIN EnumTypeItem I ON I.Code = Temp.EnumTypeItemCode
		JOIN EnumType E ON E.Code = Temp.EnumTypeCode AND E.Id = I.EnumTypeId
	)
	AS S	
	ON 
	(
        (T.[EnumTypeItemId] = S.[EnumTypeItemId] AND T.[LocaleId] = S.[LocaleId])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[EnumTypeItemId] = S.[EnumTypeItemId],
		T.[LocaleId] = S.[LocaleId],
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([EnumTypeItemId], [LocaleId], [Value])
		VALUES ([EnumTypeItemId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE EnumLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempEnumLocalised
