
CREATE TABLE #TempEnumLocalised (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[EnumTypeItemId] UNIQUEIDENTIFIER  NOT NULL ,
	[LocaleId] UNIQUEIDENTIFIER  NOT NULL ,
	[Value] NVARCHAR (100) NULL ,
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
	USING #TempEnumLocalised AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[EnumTypeItemId] = S.[EnumTypeItemId],
		T.[LocaleId] = S.[LocaleId],
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [EnumTypeItemId], [LocaleId], [Value])
		VALUES ([Id], [EnumTypeItemId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE EnumLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempEnumLocalised
