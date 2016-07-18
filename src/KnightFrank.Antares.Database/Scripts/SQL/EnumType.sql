
CREATE TABLE #TempEnumType (
	[Code] NVARCHAR (50) NOT NULL
);

ALTER TABLE EnumType NOCHECK CONSTRAINT ALL

BULK INSERT #TempEnumType
    FROM '$(OutputPath)\Scripts\Data\Configuration\EnumType.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.EnumType AS T
	USING #TempEnumType AS S	
	ON 
	(
        (T.[Code] = S.[Code])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Code] = S.[Code]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Code])
		VALUES ([Code])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE EnumType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempEnumType
