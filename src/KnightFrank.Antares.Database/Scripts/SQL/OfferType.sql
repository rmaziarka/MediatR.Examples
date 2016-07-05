CREATE TABLE #TempOfferType (
	[Code] NVARCHAR (50) NOT NULL,
	[EnumCode] NVARCHAR (250) NOT NULL
);

BULK INSERT #TempOfferType
    FROM '$(OutputPath)\Scripts\Data\Configuration\offertype.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

ALTER TABLE OfferType NOCHECK CONSTRAINT ALL

MERGE OfferType AS T
	USING #TempOfferType
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

ALTER TABLE OfferType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempOfferType
