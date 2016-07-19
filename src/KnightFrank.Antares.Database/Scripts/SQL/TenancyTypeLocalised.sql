
CREATE TABLE #TempTenancyTypeLocalised (
	[TenancyTypeCode] NVARCHAR (50) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NOT NULL
);

ALTER TABLE TenancyTypeLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempTenancyTypeLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\tenancytypelocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

MERGE TenancyTypeLocalised AS T
	USING
	(
		SELECT P.Id AS TenancyTypeId, L.Id AS LocaleId, Temp.Value
		FROM #TempTenancyTypeLocalised Temp
		JOIN dbo.Locale L ON L.IsoCode = Temp.LocaleCode
		JOIN dbo.TenancyType P ON P.Code = Temp.TenancyTypeCode
	)
	AS S
	ON
	(
        (T.[TenancyTypeId] = S.[TenancyTypeId] AND T.[LocaleId] = S.[LocaleId])
	)
	WHEN MATCHED THEN
		UPDATE SET
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([TenancyTypeId], [LocaleId], [Value])
		VALUES ([TenancyTypeId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

ALTER TABLE TenancyTypeLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempTenancyTypeLocalised
