CREATE TABLE #TempActivityType (
	[Code] NVARCHAR (50) NOT NULL
);

BULK INSERT #TempActivityType
    FROM '$(OutputPath)\Scripts\Data\Configuration\activitytype.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )

ALTER TABLE ActivityType NOCHECK CONSTRAINT ALL
	    	
MERGE ActivityType AS T
	USING #TempActivityType
	AS S	
	ON 
	(
        (T.Code = S.Code)
	)
	WHEN MATCHED THEN
		UPDATE SET
		T.[Code] = S.[Code]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Code])
		VALUES ([Code])

    WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

ALTER TABLE ActivityType WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempActivityType
