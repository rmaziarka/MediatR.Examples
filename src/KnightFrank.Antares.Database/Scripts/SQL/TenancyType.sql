CREATE TABLE #TempTenancyType (
	[Code] NVARCHAR (50) NOT NULL,
	[EnumCode] NVARCHAR (250) NOT NULL
);

BULK INSERT #TempTenancyType
    FROM '$(OutputPath)\Scripts\Data\Configuration\tenancyType.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

ALTER TABLE TenancyType NOCHECK CONSTRAINT ALL

MERGE TenancyType AS T
	USING #TempTenancyType
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

ALTER TABLE TenancyType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempTenancyType
