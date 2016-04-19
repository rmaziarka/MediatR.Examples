
CREATE TABLE #TempPropertyTypeDefinition (	
	[PropertyTypeCode] NVARCHAR (50) NOT NULL,
	[CountryCode] NVARCHAR (2) NOT NULL,
	[Division] NVARCHAR (40) NOT NULL,
	[Order] SMALLINT NOT NULL
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
	USING 
	(
		SELECT DISTINCT
		P.Id AS PropertyTypeId,	
		C.Id AS CountryId,
		I.Id AS DivisionId,
		temp.[Order]
		FROM #TempPropertyTypeDefinition temp
		JOIN Country C ON C.IsoCode = temp.CountryCode
		JOIN EnumTypeItem I ON I.Code = temp.Division
		JOIN EnumType E ON E.Id = I.EnumTypeId
		JOIN PropertyType P ON P.Code = temp.PropertyTypeCode
		WHERE E.Code = 'Division'
	) AS S	
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
		INSERT ([PropertyTypeId], [CountryId], [DivisionId], [Order])
		VALUES ([PropertyTypeId], [CountryId], [DivisionId], [Order])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyTypeDefinition WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyTypeDefinition
