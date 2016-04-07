BULK INSERT dbo.EnumTypeItems
    FROM '$(OutputPath)\Scripts\Data\Configuration\enumtypeitems.csv'
    WITH
    (
    FIRSTROW = 2,
    FIELDTERMINATOR = ';',  --CSV field delimiter
    ROWTERMINATOR = '\n',   --Use to shift the control to next row
    TABLOCK
    )