
CREATE TABLE #TempAddressFieldLabel (
	[LabelKey] NVARCHAR (100) NULL,
	[AddressFieldName] NVARCHAR (250) NOT NULL
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
	USING (
		SELECT af.Id as AddressFieldId,tmp.LabelKey FROM #TempAddressFieldLabel tmp
		JOIN AddressField af on af.Name = tmp.AddressFieldName
	)
	AS S
	ON
	(
        (T.AddressFieldId = S.AddressFieldId and T.LabelKey = S.LabelKey)
	)
	WHEN MATCHED THEN
		UPDATE SET
		T.[AddressFieldId] = S.[AddressFieldId],
		T.[LabelKey] = S.[LabelKey]

	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([AddressFieldId], [LabelKey])
		VALUES ([AddressFieldId], [LabelKey])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

ALTER TABLE AddressFieldLabel WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAddressFieldLabel
