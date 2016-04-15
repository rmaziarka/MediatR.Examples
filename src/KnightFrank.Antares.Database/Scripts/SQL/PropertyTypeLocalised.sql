
CREATE TABLE #TempPropertyTypeLocalised (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[PropertyTypeId] UNIQUEIDENTIFIER  NOT NULL ,
	[LocaleId] UNIQUEIDENTIFIER  NOT NULL ,
	[Value] NVARCHAR (100) NOT NULL ,
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
	USING #TempPropertyTypeLocalised AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[PropertyTypeId] = S.[PropertyTypeId],
		T.[LocaleId] = S.[LocaleId],
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [PropertyTypeId], [LocaleId], [Value])
		VALUES ([Id], [PropertyTypeId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyTypeLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyTypeLocalised
