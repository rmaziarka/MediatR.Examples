
CREATE TABLE #TempAddressField (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[Name] NVARCHAR (100) NULL ,
);

ALTER TABLE AddressField NOCHECK CONSTRAINT ALL

BULK INSERT #TempAddressField
    FROM '$(OutputPath)\Scripts\Data\Configuration\AddressField.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.AddressField AS T
	USING #TempAddressField AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[Name] = S.[Name]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [Name])
		VALUES ([Id], [Name])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE AddressField WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAddressField
