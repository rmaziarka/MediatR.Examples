CREATE TABLE #TempRequirementType (
	[Code] NVARCHAR (50) NOT NULL,
	[EnumCode] NVARCHAR (250) NOT NULL
);

BULK INSERT #TempRequirementType
    FROM '$(OutputPath)\Scripts\Data\Configuration\requirementtype.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

ALTER TABLE RequirementType NOCHECK CONSTRAINT ALL

MERGE RequirementType AS T
	USING #TempRequirementType
	AS S
	ON
	(
        (T.Code = S.Code)
	)
	WHEN MATCHED THEN
		UPDATE SET
		T.[Code] = S.[Code],
		T.[EnumCode] = S.[EnumCode]

	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Code],[EnumCode])
		VALUES ([Code],[EnumCode])

    WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

ALTER TABLE RequirementType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempRequirementType
