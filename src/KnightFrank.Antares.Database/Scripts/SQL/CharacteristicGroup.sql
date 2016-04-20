IF OBJECT_ID('tempdb..#ChracteristicsGroupsCsv') IS NOT NULL
    DROP TABLE #ChracteristicsGroupsCsv

CREATE TABLE #ChracteristicsGroupsCsv (
	Code NVARCHAR(250)  NOT NULL
);

BULK INSERT #ChracteristicsGroupsCsv
    FROM '$(OutputPath)\Scripts\Data\Configuration\CharacteristicGroup.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    );

--select * from #ChracteristicsGroupsCsv

MERGE CharacteristicGroup AS T
    USING #ChracteristicsGroupsCsv AS S
    ON (
	    S.Code = T.Code
    )
    WHEN NOT MATCHED BY TARGET THEN
	   Insert (Code) Values (S.Code)

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

DROP TABLE #ChracteristicsGroupsCsv
