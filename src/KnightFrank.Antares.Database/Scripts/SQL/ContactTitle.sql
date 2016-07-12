
CREATE TABLE #TempContactTitle (
	[Title] NVARCHAR (100) NULL,
	[Locale] NVARCHAR (100) NOT NULL,
	[Priority] INT NULL
);

ALTER TABLE ContactTitle NOCHECK CONSTRAINT ALL

BULK INSERT #TempContactTitle
    FROM '$(OutputPath)\Scripts\Data\Configuration\contactTitle.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		CODEPAGE = 1252,
		ROWTERMINATOR = '\n',
		TABLOCK
    )

MERGE dbo.ContactTitle AS T
	USING 
	(
		SELECT CT.Title, L.Id AS LocaleId, CT.[Priority] 
		FROM #TempContactTitle CT
		JOIN Locale L ON CT.Locale = L.IsoCode
	)	
	AS S	
	ON 
	(
        (T.Title = S.Title AND T.LocaleId = S.LocaleId)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Title] = S.[Title],
		T.[LocaleId] = S.[LocaleId],
		T.[Priority] = S.[Priority]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Title], [LocaleId], [Priority])
		VALUES ([Title], [LocaleId], [Priority])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE ContactTitle WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempContactTitle
