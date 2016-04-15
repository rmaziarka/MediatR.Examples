
CREATE TABLE #TempPropertyAttributeFormDefinition (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[AttributeId] UNIQUEIDENTIFIER  NOT NULL ,
	[PropertyAttributeFormId] UNIQUEIDENTIFIER  NOT NULL ,
	[Order] INT  NOT NULL ,
);

ALTER TABLE PropertyAttributeFormDefinition NOCHECK CONSTRAINT ALL

BULK INSERT #TempPropertyAttributeFormDefinition
    FROM '$(OutputPath)\Scripts\Data\Configuration\PropertyAttributeFormDefinition.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.PropertyAttributeFormDefinition AS T
	USING #TempPropertyAttributeFormDefinition AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[AttributeId] = S.[AttributeId],
		T.[PropertyAttributeFormId] = S.[PropertyAttributeFormId],
		T.[Order] = S.[Order]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [AttributeId], [PropertyAttributeFormId], [Order])
		VALUES ([Id], [AttributeId], [PropertyAttributeFormId], [Order])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyAttributeFormDefinition WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyAttributeFormDefinition
