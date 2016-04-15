
CREATE TABLE #TempAddressFieldDefinition (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[AddressFieldId] UNIQUEIDENTIFIER  NOT NULL ,
	[AddressFieldLabelId] UNIQUEIDENTIFIER  NOT NULL ,
	[AddressFormId] UNIQUEIDENTIFIER  NOT NULL ,
	[Required] BIT  NOT NULL ,
	[RegEx] NVARCHAR (50) NULL ,
	[RowOrder] SMALLINT  NOT NULL ,
	[ColumnOrder] SMALLINT  NOT NULL ,
	[ColumnSize] SMALLINT  NOT NULL ,
);

ALTER TABLE AddressFieldDefinition NOCHECK CONSTRAINT ALL

BULK INSERT #TempAddressFieldDefinition
    FROM '$(OutputPath)\Scripts\Data\Configuration\AddressFieldDefinition.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.AddressFieldDefinition AS T
	USING #TempAddressFieldDefinition AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[AddressFieldId] = S.[AddressFieldId],
		T.[AddressFieldLabelId] = S.[AddressFieldLabelId],
		T.[AddressFormId] = S.[AddressFormId],
		T.[Required] = S.[Required],
		T.[RegEx] = S.[RegEx],
		T.[RowOrder] = S.[RowOrder],
		T.[ColumnOrder] = S.[ColumnOrder],
		T.[ColumnSize] = S.[ColumnSize]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [AddressFieldId], [AddressFieldLabelId], [AddressFormId], [Required], [RegEx], [RowOrder], [ColumnOrder], [ColumnSize])
		VALUES ([Id], [AddressFieldId], [AddressFieldLabelId], [AddressFormId], [Required], [RegEx], [RowOrder], [ColumnOrder], [ColumnSize])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE AddressFieldDefinition WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAddressFieldDefinition
