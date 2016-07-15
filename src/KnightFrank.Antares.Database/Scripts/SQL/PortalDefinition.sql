CREATE TABLE #TempPortalDefinition (
	[Name] NVARCHAR (250) NOT NULL,
	[CountryCode] NVARCHAR (250) NOT NULL
);

CREATE TABLE #TempPortal (
	[Name] NVARCHAR (250) NOT NULL
);

BULK INSERT #TempPortalDefinition
    FROM 'C:\_Projects\KnightFrank.Antares\src\KnightFrank.Antares.Database\Scripts\Data\Configuration\portaldefinition.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
	 
Insert into #TempPortal (Name) (Select distinct Name From #TempPortalDefinition)

-- Portal table update
ALTER TABLE Portal NOCHECK CONSTRAINT ALL

MERGE Portal AS T
	USING #TempPortal
	AS S
	ON
	(
        (T.Name = S.Name)
	)
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([Name])
		VALUES ([Name])

    WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

ALTER TABLE Portal WITH CHECK CHECK CONSTRAINT ALL

-- Portal definition table update
MERGE PortalDefinition AS T
	USING (select p.Id as PortalId, c.Id as CountryId from #TempPortalDefinition tmp join Country c on c.IsoCode = tmp.CountryCode join Portal p on p.Name = tmp.Name)
	AS S
	ON
	(
        (T.PortalId = S.PortalId and T.CountryId = S.CountryId)
	)
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (PortalId,CountryId)
		VALUES (PortalId,CountryId)

    WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

DROP TABLE #TempPortal
DROP TABLE #TempPortalDefinition
