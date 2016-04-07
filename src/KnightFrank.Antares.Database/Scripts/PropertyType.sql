BULK INSERT dbo.PropertyType
    FROM '$(OutputPath)\Scripts\Data\Configuration\propertytypes.csv'
    WITH
    (
    FIRSTROW = 2,
    FIELDTERMINATOR = ';',  --CSV field delimiter
    ROWTERMINATOR = '\n',   --Use to shift the control to next row
    TABLOCK
    )