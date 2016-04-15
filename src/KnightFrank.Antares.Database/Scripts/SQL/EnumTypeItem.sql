
CREATE TABLE #TempEnumTypeItem (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[Code] NVARCHAR (40) NULL ,
	[EnumTypeId] UNIQUEIDENTIFIER  NOT NULL ,
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
	USING #TempEnumTypeItem AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Code] = S.[Code],
		T.[EnumTypeId] = S.[EnumTypeId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [Code], [EnumTypeId])
		VALUES ([Id], [Code], [EnumTypeId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE EnumTypeItem WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempEnumTypeItem
