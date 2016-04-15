
CREATE TABLE #TempPropertyAttributeForm (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[CountryId] UNIQUEIDENTIFIER  NOT NULL ,
	[PropertyTypeId] UNIQUEIDENTIFIER  NOT NULL ,
);

ALTER TABLE PropertyAttributeForm NOCHECK CONSTRAINT ALL

BULK INSERT #TempPropertyAttributeForm
    FROM '$(OutputPath)\Scripts\Data\Configuration\PropertyAttributeForm.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.PropertyAttributeForm AS T
	USING #TempPropertyAttributeForm AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[CountryId] = S.[CountryId],
		T.[PropertyTypeId] = S.[PropertyTypeId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [CountryId], [PropertyTypeId])
		VALUES ([Id], [CountryId], [PropertyTypeId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyAttributeForm WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyAttributeForm
