
CREATE TABLE #TempAddressFormEntityType (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[AddressFormId] UNIQUEIDENTIFIER  NOT NULL ,
	[EnumTypeItemId] UNIQUEIDENTIFIER  NOT NULL ,
);

ALTER TABLE AddressFormEntityType NOCHECK CONSTRAINT ALL

BULK INSERT #TempAddressFormEntityType
    FROM '$(OutputPath)\Scripts\Data\Configuration\AddressFormEntityType.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.AddressFormEntityType AS T
	USING #TempAddressFormEntityType AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[AddressFormId] = S.[AddressFormId],
		T.[EnumTypeItemId] = S.[EnumTypeItemId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [AddressFormId], [EnumTypeItemId])
		VALUES ([Id], [AddressFormId], [EnumTypeItemId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE AddressFormEntityType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAddressFormEntityType
