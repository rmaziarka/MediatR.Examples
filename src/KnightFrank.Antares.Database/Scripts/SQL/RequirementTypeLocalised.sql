CREATE TABLE #TempRequirementTypeLocalised (
	[RequirementTypeEnumCode] NVARCHAR (50) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NOT NULL
);

ALTER TABLE RequirementTypeLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempRequirementTypeLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\RequirementTypeLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.RequirementTypeLocalised AS T
	USING 
	(
		SELECT 
		P.Id AS RequirementTypeId,
		L.Id AS LocaleId,
		[Value]
		FROM #TempRequirementTypeLocalised Temp
		JOIN Locale L ON L.IsoCode = Temp.LocaleCode
		JOIN RequirementType P ON P.EnumCode = Temp.RequirementTypeEnumCode
	)
	AS S	
	ON 
	(
        (T.[RequirementTypeId] = S.[RequirementTypeId] AND T.[LocaleId] = S.[LocaleId])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[RequirementTypeId] = S.[RequirementTypeId],
		T.[LocaleId] = S.[LocaleId],
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([RequirementTypeId], [LocaleId], [Value])
		VALUES ([RequirementTypeId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE RequirementTypeLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempRequirementTypeLocalised
