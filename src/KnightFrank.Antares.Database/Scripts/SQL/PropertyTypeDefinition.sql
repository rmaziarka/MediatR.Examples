
CREATE TABLE #TempPropertyTypeDefinition (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[PropertyTypeId] UNIQUEIDENTIFIER  NOT NULL ,
	[CountryId] UNIQUEIDENTIFIER  NOT NULL ,
	[DivisionId] UNIQUEIDENTIFIER  NOT NULL ,
	[Order] SMALLINT  NOT NULL ,
);

ALTER TABLE PropertyTypeDefinition NOCHECK CONSTRAINT ALL

BULK INSERT #TempPropertyTypeDefinition
    FROM '$(OutputPath)\Scripts\Data\Configuration\PropertyTypeDefinition.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.PropertyTypeDefinition AS T
	USING #TempPropertyTypeDefinition AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[PropertyTypeId] = S.[PropertyTypeId],
		T.[CountryId] = S.[CountryId],
		T.[DivisionId] = S.[DivisionId],
		T.[Order] = S.[Order]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [PropertyTypeId], [CountryId], [DivisionId], [Order])
		VALUES ([Id], [PropertyTypeId], [CountryId], [DivisionId], [Order])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyTypeDefinition WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyTypeDefinition
