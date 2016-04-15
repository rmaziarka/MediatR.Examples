
CREATE TABLE #TempPropertyType (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[ParentId] UNIQUEIDENTIFIER  NULL ,
	[Code] NVARCHAR (50) NOT NULL ,
);

ALTER TABLE PropertyType NOCHECK CONSTRAINT ALL

BULK INSERT #TempPropertyType
    FROM '$(OutputPath)\Scripts\Data\Configuration\PropertyType.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.PropertyType AS T
	USING #TempPropertyType AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[ParentId] = S.[ParentId],
		T.[Code] = S.[Code]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [ParentId], [Code])
		VALUES ([Id], [ParentId], [Code])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE PropertyType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempPropertyType
