
CREATE TABLE #TempAddressForm (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[CountryId] UNIQUEIDENTIFIER  NOT NULL ,
);

ALTER TABLE AddressForm NOCHECK CONSTRAINT ALL

BULK INSERT #TempAddressForm
    FROM '$(OutputPath)\Scripts\Data\Configuration\AddressForm.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.AddressForm AS T
	USING #TempAddressForm AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[CountryId] = S.[CountryId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [CountryId])
		VALUES ([Id], [CountryId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE AddressForm WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAddressForm
