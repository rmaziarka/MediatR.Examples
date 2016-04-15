
CREATE TABLE #TempContact (

	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[FirstName] NVARCHAR (MAX) NULL ,
	[Surname] NVARCHAR (MAX) NULL ,
	[Title] NVARCHAR (MAX) NULL ,
);

ALTER TABLE Contact NOCHECK CONSTRAINT ALL

BULK INSERT #TempContact
    FROM '$(OutputPath)\Scripts\Data\Test\Contact.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.Contact AS T
	USING #TempContact AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[FirstName] = S.[FirstName],
		T.[Surname] = S.[Surname],
		T.[Title] = S.[Title]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [FirstName], [Surname], [Title])
		VALUES ([Id], [FirstName], [Surname], [Title])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Contact WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempContact
