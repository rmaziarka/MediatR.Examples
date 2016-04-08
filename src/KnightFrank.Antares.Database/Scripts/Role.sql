CREATE TABLE #TempRoles (      
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [nvarchar](100) NULL,
);

BULK INSERT #TempRoles
    FROM '$(OutputPath)\Scripts\Data\Configuration\roles.csv'
    WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',  --CSV field delimiter
		ROWTERMINATOR = '\n',   --Use to shift the control to next row
		TABLOCK
    )

MERGE dbo.Role AS T
USING #TempRoles AS S
ON (T.Id = S.Id)
       WHEN NOT MATCHED BY TARGET
              THEN   INSERT(Id, Name) 
                           VALUES(Id, Name)
       WHEN MATCHED 
              THEN   UPDATE SET
                                  T.Name = S.Name
       WHEN NOT MATCHED BY SOURCE
              THEN DELETE;

DROP TABLE #TempRoles