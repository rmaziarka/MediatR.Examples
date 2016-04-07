BULK INSERT dbo.EnumType
    FROM '$(OutputPath)\Scripts\Data\Configuration\enumtypes.csv'
    WITH
    (
    FIRSTROW = 2,
    FIELDTERMINATOR = ';',  --CSV field delimiter
    ROWTERMINATOR = '\n',   --Use to shift the control to next row
    TABLOCK
    )