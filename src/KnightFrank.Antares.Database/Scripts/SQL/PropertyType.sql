CREATE TABLE #TempPropertyType (
	[ParentCode] NVARCHAR (50) NULL,
	[Code] NVARCHAR (50) NOT NULL
);

BULK INSERT #TempPropertyType
    FROM '$(OutputPath)\Scripts\Data\Configuration\PropertyType.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

ALTER TABLE PropertyType NOCHECK CONSTRAINT ALL
	    	
MERGE PropertyType AS T
	USING 
	(
		SELECT Temp.Code
		FROM #TempPropertyType Temp
		WHERE Temp.ParentCode IS NULL
	)
	AS S	
	ON 
	(
        (T.Code = S.Code AND T.ParentId IS NULL)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[ParentId] = NULL,
		T.[Code] = S.[Code]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([ParentId], [Code])
		VALUES (NULL, [Code])

    WHEN NOT MATCHED BY SOURCE AND T.[ParentId] IS NULL THEN
		DELETE;

MERGE dbo.PropertyType AS T
	USING 
	(
		SELECT P.Id as ParentId, Temp.Code as Code
		FROM #TempPropertyType Temp
		JOIN PropertyType P ON P.Code = Temp.ParentCode		
		WHERE Temp.ParentCode IS NOT NULL
	)
	AS S	
	ON 
	(
        (T.Code = S.Code AND T.ParentId = S.ParentId)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[ParentId] = S.[ParentId],
		T.[Code] = S.[Code]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([ParentId], [Code])
		VALUES ([ParentId], [Code])

	WHEN NOT MATCHED BY SOURCE AND T.[ParentId] IS NOT NULL THEN
		DELETE;
    
ALTER TABLE PropertyType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyType
