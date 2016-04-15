
CREATE TABLE #TempAttribute (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[NameKey] NVARCHAR (MAX) NULL ,
	[LabelKey] NVARCHAR (MAX) NULL ,
);

ALTER TABLE Attribute NOCHECK CONSTRAINT ALL

BULK INSERT #TempAttribute
    FROM '$(OutputPath)\Scripts\Data\Configuration\Attribute.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.Attribute AS T
	USING #TempAttribute AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[NameKey] = S.[NameKey],
		T.[LabelKey] = S.[LabelKey]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [NameKey], [LabelKey])
		VALUES ([Id], [NameKey], [LabelKey])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Attribute WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempAttribute
