CREATE TABLE #TempAddressField (
	[Name] NVARCHAR (100) NULL
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
        (T.Name = S.Name)
	)
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Name])
		VALUES ([Name])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

ALTER TABLE AddressField WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAddressField
