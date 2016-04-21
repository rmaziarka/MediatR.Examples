CREATE TABLE #TempActivityTypeDefinition (
	[PropertyTypeCode] NVARCHAR (50) NOT NULL,
	[CountryCode] NVARCHAR (50) NOT NULL,
	[ActivityTypeCode] NVARCHAR (50) NOT NULL,
	[Order] NVARCHAR (50) NOT NULL
);

BULK INSERT #TempActivityTypeDefinition
    FROM '$(OutputPath)\Scripts\Data\Configuration\activitytypedefinition.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

ALTER TABLE ActivityTypeDefinition NOCHECK CONSTRAINT ALL
	    	
MERGE ActivityTypeDefinition AS TargetTable
	USING 
	(
		SELECT
		PT.Id AS PropertyTypeId,	
		C.Id AS CountryId,
        AT.Id AS ActivityTypeId,
		temp.[Order]
		FROM #TempActivityTypeDefinition temp
		JOIN Country C ON C.IsoCode = temp.CountryCode
		JOIN PropertyType PT ON PT.Code = temp.PropertyTypeCode
		JOIN ActivityType AT ON AT.Code = temp.ActivityTypeCode
	)	
	AS SourceTable
	ON 
	(
        (TargetTable.ActivityTypeId = SourceTable.ActivityTypeId 
            AND TargetTable.CountryId = SourceTable.CountryId 
            AND TargetTable.PropertyTypeId = SourceTable.PropertyTypeId )
	)
	WHEN MATCHED THEN
		UPDATE SET
		TargetTable.[Order] = SourceTable.[Order]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([ActivityTypeId], [PropertyTypeId], [CountryId], [Order])
		VALUES ([ActivityTypeId], [PropertyTypeId], [CountryId], [Order])

    WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

ALTER TABLE ActivityTypeDefinition WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempActivityTypeDefinition