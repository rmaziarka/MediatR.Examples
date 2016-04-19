CREATE TABLE #TempPropertyAttributeFormDefinition (
	[CountryCode] NVARCHAR (2) NOT NULL,
	[PropertyTypeCode] NVARCHAR (50) NOT NULL,
	[AttributeNameKey] NVARCHAR (MAX) NOT NULL,
	[Order] INT NOT NULL
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
	USING 
	(
		SELECT A.Id AS AttributeId, F.Id AS PropertyAttributeFormId, Temp.[Order]
		FROM #TempPropertyAttributeFormDefinition Temp
		JOIN Attribute A ON A.NameKey = Temp.AttributeNameKey
		JOIN Country C ON C.IsoCode = Temp.CountryCode
		JOIN PropertyType P ON P.Code = Temp.PropertyTypeCode
		JOIN PropertyAttributeForm F ON F.CountryId = C.Id AND F.PropertyTypeId = P.Id
	) 
	AS S	
	ON 
	(
        (T.AttributeId = S.AttributeId AND T.PropertyAttributeFormId = S.PropertyAttributeFormId)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[AttributeId] = S.[AttributeId],
		T.[PropertyAttributeFormId] = S.[PropertyAttributeFormId],
		T.[Order] = S.[Order]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([AttributeId], [PropertyAttributeFormId], [Order])
		VALUES ([AttributeId], [PropertyAttributeFormId], [Order])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyAttributeFormDefinition WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyAttributeFormDefinition
