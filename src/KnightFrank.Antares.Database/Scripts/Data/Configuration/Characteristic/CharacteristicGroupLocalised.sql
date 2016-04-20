IF OBJECT_ID('tempDB..#ChracteristicGroupLocalisedsCsv') IS NOT NULL
    DROP TABLE #ChracteristicGroupLocalisedsCsv

CREATE TABLE #ChracteristicGroupLocalisedsCsv (
	Code NVARCHAR(250)  NOT NULL,
	LocaleCode NVARCHAR (250) NULL,
	Value NVARCHAR (100) NULL
);

BULK INSERT #ChracteristicGroupLocalisedsCsv
    FROM '$(OutputPath)\Scripts\Data\Configuration\Characteristic\CharacteristicGroupLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    );

--select * from #ChracteristicGroupLocalisedsCsv

MERGE CharacteristicGroupLocalised AS T
    USING
    (
		SELECT chg.Id AS CharacteristicGroupId, l.Id AS LocaleId, csv.Value
		FROM #ChracteristicGroupLocalisedsCsv csv
		JOIN CharacteristicGroup chg ON csv.Code = chg.Code
		JOIN Locale l ON csv.LocaleCode = l.IsoCode
    )
    AS S
	ON
	(
        (T.CharacteristicGroupId = S.CharacteristicGroupId AND T.LocaleId = S.LocaleId)
	)
    WHEN MATCHED THEN
		    UPDATE SET
		    T.Value = S.Value

    WHEN NOT MATCHED BY TARGET THEN
	   Insert (CharacteristicGroupId, LocaleId, Value) Values (S.CharacteristicGroupId, S.LocaleId, S.Value)

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

DROP TABLE #ChracteristicGroupLocalisedsCsv