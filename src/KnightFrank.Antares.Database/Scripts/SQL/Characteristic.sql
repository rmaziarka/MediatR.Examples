IF OBJECT_ID('tempDB..#ChracteristicsCsv') IS NOT NULL
    DROP TABLE #ChracteristicsCsv

CREATE TABLE #ChracteristicsCsv (
	Code NVARCHAR(250)  NOT NULL,
	IsDisplayText BIT NOT NULL,
	IsEnabled BIT NULL
);

BULK INSERT #ChracteristicsCsv
    FROM '$(OutputPath)\Scripts\Data\Configuration\Characteristic.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    );

--select * from #ChracteristicsCsv

MERGE Characteristic AS T
    USING #ChracteristicsCsv AS S
    ON (
	    S.Code = T.Code
    )
    WHEN MATCHED THEN
		    UPDATE SET
		    T.IsEnabled = S.IsEnabled,
		    T.IsDisplayText = S.IsDisplayText --ifnull

    WHEN NOT MATCHED BY TARGET THEN
	   Insert (Code, IsEnabled, IsDisplayText) Values (S.Code, S.IsEnabled, S.IsDisplayText)

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

DROP TABLE #ChracteristicsCsv