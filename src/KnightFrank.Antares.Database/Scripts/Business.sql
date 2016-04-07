BULK INSERT dbo.Business
    FROM '$(OutputPath)\Scripts\Data\Configuration\businesses.csv'
    WITH
    (
    FIRSTROW = 2,
    FIELDTERMINATOR = ';',  --CSV field delimiter
    ROWTERMINATOR = '\n',   --Use to shift the control to next row
    TABLOCK
    )