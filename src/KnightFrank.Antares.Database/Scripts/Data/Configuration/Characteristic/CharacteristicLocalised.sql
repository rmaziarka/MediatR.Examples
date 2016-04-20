IF OBJECT_ID('tempDB..#ChracteristicLocalisedsCsv') IS NOT NULL
    DROP TABLE #ChracteristicLocalisedsCsv

CREATE TABLE #ChracteristicLocalisedsCsv (
	Code NVARCHAR(250)  NOT NULL,
	LocaleCode NVARCHAR (250) NULL,
	Value NVARCHAR (100) NULL
);

BULK INSERT #ChracteristicLocalisedsCsv
    FROM '$(OutputPath)\Scripts\Data\Configuration\Characteristic\CharacteristicLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    );

--select * from #ChracteristicLocalisedsCsv

MERGE CharacteristicLocalised AS T
    USING
    (
		SELECT ch.Id AS CharacteristicId, l.Id AS LocaleId, csv.Value
		FROM #ChracteristicLocalisedsCsv csv
		JOIN Characteristic ch ON csv.Code = ch.Code
		JOIN Locale l ON csv.LocaleCode = l.IsoCode
    )
    AS S
	ON
	(
        (T.CharacteristicId = S.CharacteristicId AND T.LocaleId = S.LocaleId)
	)
    WHEN MATCHED THEN
		    UPDATE SET
		    T.Value = S.Value

    WHEN NOT MATCHED BY TARGET THEN
	   Insert (CharacteristicId, LocaleId, Value) Values (S.CharacteristicId, S.LocaleId, S.Value)

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

DROP TABLE #ChracteristicLocalisedsCsv