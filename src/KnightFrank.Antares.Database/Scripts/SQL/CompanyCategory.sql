CREATE TABLE #TempCompanyCategory (
	[Code] NVARCHAR (50) NOT NULL,
	[EnumCode] NVARCHAR (250) NOT NULL
);

BULK INSERT #TempCompanyCategory
    FROM '$(OutputPath)\Scripts\Data\Configuration\companyCategory.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

ALTER TABLE dbo.CompanyCategory NOCHECK CONSTRAINT ALL

MERGE dbo.CompanyCategory AS T
	USING #TempCompanyCategory
	AS S
	ON
	(
        (T.Code = S.Code)
	)
	WHEN MATCHED THEN
		UPDATE SET
		T.[EnumCode] = S.[EnumCode]

	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Code],[EnumCode])
		VALUES ([Code],[EnumCode])

    WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

ALTER TABLE CompanyCategory WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempCompanyCategory
