
CREATE TABLE #TempAddressFieldLabel (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[AddressFieldId] UNIQUEIDENTIFIER  NOT NULL ,
	[LabelKey] NVARCHAR (100) NULL ,
);

ALTER TABLE AddressFieldLabel NOCHECK CONSTRAINT ALL

BULK INSERT #TempAddressFieldLabel
    FROM '$(OutputPath)\Scripts\Data\Configuration\AddressFieldLabel.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.AddressFieldLabel AS T
	USING #TempAddressFieldLabel AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[AddressFieldId] = S.[AddressFieldId],
		T.[LabelKey] = S.[LabelKey]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [AddressFieldId], [LabelKey])
		VALUES ([Id], [AddressFieldId], [LabelKey])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE AddressFieldLabel WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAddressFieldLabel
