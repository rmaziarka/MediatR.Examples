
CREATE TABLE #TempEnumType (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[Code] NVARCHAR (25) NULL ,
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
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Code] = S.[Code]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [Code])
		VALUES ([Id], [Code])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE EnumType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempEnumType
