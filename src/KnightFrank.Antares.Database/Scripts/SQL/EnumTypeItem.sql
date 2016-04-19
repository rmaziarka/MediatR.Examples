
CREATE TABLE #TempEnumTypeItem (	
	[EnumTypeCode] NVARCHAR (25) NOT NULL,
	[Code] NVARCHAR (40) NOT NULL
);

ALTER TABLE EnumTypeItem NOCHECK CONSTRAINT ALL

BULK INSERT #TempEnumTypeItem
    FROM '$(OutputPath)\Scripts\Data\Configuration\EnumTypeItem.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.EnumTypeItem AS T
	USING
	(
		SELECT E.Id AS EnumTypeId, Temp.Code
		FROM #TempEnumTypeItem Temp
		JOIN EnumType E ON E.Code = Temp.EnumTypeCode
	)
	AS S	
	ON 
	(
        (T.EnumTypeId = S.EnumTypeId AND T.Code = S.Code)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Code] = S.[Code],
		T.[EnumTypeId] = S.[EnumTypeId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Code], [EnumTypeId])
		VALUES ([Code], [EnumTypeId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE EnumTypeItem WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempEnumTypeItem
