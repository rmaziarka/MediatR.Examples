
CREATE TABLE #TempPropertyAttributeForm (
	[CountryCode] NVARCHAR (2) NOT NULL,
	[PropertyTypeCode] NVARCHAR (50) NOT NULL
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
	USING
	(
		SELECT C.Id AS CountryId, P.Id AS PropertyTypeId
		FROM #TempPropertyAttributeForm Temp
		JOIN Country C ON C.IsoCode = Temp.CountryCode
		JOIN PropertyType P ON P.Code = Temp.PropertyTypeCode
	)
	AS S	
	ON 
	(
        (T.CountryId = S.CountryId AND T.PropertyTypeId = S.PropertyTypeId)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[CountryId] = S.[CountryId],
		T.[PropertyTypeId] = S.[PropertyTypeId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([CountryId], [PropertyTypeId])
		VALUES ([CountryId], [PropertyTypeId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyAttributeForm WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyAttributeForm
