CREATE TABLE #TempRequirementTypeActivityTypeDefinition (
	RequirementTypeEnumCode NVARCHAR (250) NOT NULL,
	ActivityTypeEnumCode NVARCHAR (250) NOT NULL,
	CountryIsoCode NVARCHAR (50) NOT NULL,
	PropertyTypeEnumCode NVARCHAR (250) NOT NULL
);

CREATE TABLE #Temp (
	Atd_Id uniqueidentifier NULL,
	Req_Id uniqueidentifier NULL
);

BULK INSERT #TempRequirementTypeActivityTypeDefinition
    FROM 'C:\_Projects\KnightFrank.Antares\src\KnightFrank.Antares.Database\Scripts\Data\Configuration\requirementtypeactivitytypedefinition.csv'
               WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    );


INSERT INTO #Temp (Atd_Id,Req_Id)
(
	SELECT atd.Id,rt.Id FROM ActivityTypeDefinition atd
		JOIN Country c ON c.Id = atd.CountryId
		JOIN ActivityType [at] ON at.Id = atd.ActivityTypeId
		JOIN PropertyType pt ON pt.Id = atd.PropertyTypeId
		JOIN #TempRequirementTypeActivityTypeDefinition tmp ON tmp.ActivityTypeEnumCode = at.EnumCode and tmp.CountryIsoCode = c.IsoCode and tmp.PropertyTypeEnumCode = pt.EnumCode
		JOIN RequirementType rt ON rt.EnumCode = tmp.RequirementTypeEnumCode
	)

ALTER TABLE RequirementTypeActivityTypeDefinition NOCHECK CONSTRAINT ALL

MERGE [RequirementTypeActivityTypeDefinition] AS T
	USING #Temp
	AS S
	ON
	(
        (T.RequirementTypeId = S.Req_Id and T.ActivityTypeDefinitionId = S.Atd_Id)
	) 
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (RequirementTypeId, ActivityTypeDefinitionId)
		VALUES (Req_Id,Atd_Id)

    WHEN NOT MATCHED BY SOURCE THEN
		DELETE;

ALTER TABLE [RequirementTypeActivityTypeDefinition] WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempRequirementTypeActivityTypeDefinition
DROP TABLE #Temp