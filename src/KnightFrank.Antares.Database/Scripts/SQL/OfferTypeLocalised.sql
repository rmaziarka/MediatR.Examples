
CREATE TABLE #TempOfferTypeLocalised (
	[OfferTypeCode] NVARCHAR (50) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NOT NULL
);

ALTER TABLE OfferTypeLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempOfferTypeLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\offerTypeLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.OfferTypeLocalised AS T
	USING 
	(
		SELECT 
		P.Id AS OfferTypeId,
		L.Id AS LocaleId,
		[Value]
		FROM #TempOfferTypeLocalised Temp
		JOIN Locale L ON L.IsoCode = Temp.LocaleCode
		JOIN OfferType P ON P.Code = Temp.OfferTypeCode
	)
	AS S	
	ON 
	(
        (T.[OfferTypeId] = S.[OfferTypeId] AND T.[LocaleId] = S.[LocaleId])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([OfferTypeId], [LocaleId], [Value])
		VALUES ([OfferTypeId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE OfferTypeLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempOfferTypeLocalised
