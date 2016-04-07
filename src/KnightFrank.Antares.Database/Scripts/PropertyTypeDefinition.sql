BULK INSERT dbo.PropertyTypeDefinition
    FROM '$(OutputPath)\Scripts\Data\Configuration\propertytypedefinitions.csv'
    WITH
    (
    FIRSTROW = 2,
    FIELDTERMINATOR = ';',  --CSV field delimiter
    ROWTERMINATOR = '\n',   --Use to shift the control to next row
    TABLOCK
    )