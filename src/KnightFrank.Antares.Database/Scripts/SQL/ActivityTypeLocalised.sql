
CREATE TABLE #TempActivityTypeLocalised (
	[ActivityTypeCode] NVARCHAR (50) NOT NULL,
	[LocaleCode] NVARCHAR (2) NOT NULL,
	[Value] NVARCHAR (100) NOT NULL
);

ALTER TABLE ActivityTypeLocalised NOCHECK CONSTRAINT ALL

BULK INSERT #TempActivityTypeLocalised
    FROM '$(OutputPath)\Scripts\Data\Configuration\ActivityTypeLocalised.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.ActivityTypeLocalised AS T
	USING 
	(
		SELECT 
		P.Id AS ActivityTypeId,
		L.Id AS LocaleId,
		[Value]
		FROM #TempActivityTypeLocalised Temp
		JOIN Locale L ON L.IsoCode = Temp.LocaleCode
		JOIN ActivityType P ON P.Code = Temp.ActivityTypeCode
	)
	AS S	
	ON 
	(
        (T.[ActivityTypeId] = S.[ActivityTypeId] AND T.[LocaleId] = S.[LocaleId])
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Value] = S.[Value]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([ActivityTypeId], [LocaleId], [Value])
		VALUES ([ActivityTypeId], [LocaleId], [Value])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE ActivityTypeLocalised WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempActivityTypeLocalised
