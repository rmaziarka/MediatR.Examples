/******* Load data form CSV *******/
IF OBJECT_ID('tempDB..#ChracteristicsMapCsv', 'U') IS NOT NULL
    DROP TABLE #ChracteristicsMapCsv

CREATE TABLE #ChracteristicsMapCsv (
    CountryIsoCode NVARCHAR(250)  NOT NULL,
    PropertyTypeCode NVARCHAR(250)  NOT NULL,
    CharacteristicGroupCode NVARCHAR(250)  NOT NULL,
    GroupOrder int NOT NULL,
    DisplayGroupLabel bit NOT NULL,
    CharacteristicCode  NVARCHAR(250)  NOT NULL
);
BULK INSERT #ChracteristicsMapCsv
    FROM '$(OutputPath)\Scripts\Data\Configuration\CharacteristicGroupUsageItems.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    );
select * from #ChracteristicsMapCsv
/******* End-Of: Load data form CSV *******/


/******* Prepare data for CharacteristicGroupUsage *******/
IF OBJECT_ID('tempDB..#CharacteristicGroupUsage_Expected', 'U') IS NOT NULL
    DROP TABLE #CharacteristicGroupUsage_Expected

CREATE TABLE #CharacteristicGroupUsage_Expected (
    CountryId UNIQUEIDENTIFIER,
    CountryIsoCode NVARCHAR(250)  NOT NULL,
    PropertyTypeId UNIQUEIDENTIFIER,
    PropertyTypeCode NVARCHAR(250)  NOT NULL,
    CharacteristicGroupId UNIQUEIDENTIFIER,
    CharacteristicGroupCode NVARCHAR(250)  NOT NULL,

    GroupOrder int NOT NULL,
    DisplayGroupLabel bit NOT NULL
);
INSERT INTO #CharacteristicGroupUsage_Expected
select distinct
    c.Id as CountryId,
    c.IsoCode as CountryIsoCode,
    pt.Id as PropertyTypeId,
    pt.Code as PropertyTypeCode,
    chg.Id as CharacteristicGroupId,
    chg.Code as CharacteristicGroupCode,
    csv.GroupOrder,
    csv.DisplayGroupLabel
from #ChracteristicsMapCsv csv
    inner join Country c on csv.CountryIsoCode = c.IsoCode
    inner join PropertyType pt on csv.PropertyTypeCode = pt.Code
    inner join CharacteristicGroup chg on csv.CharacteristicGroupCode = chg.Code

select * from #CharacteristicGroupUsage_Expected
/******* End-Of: Prepare data for CharacteristicGroupUsage *******/

/******* Remove items form table CharacteristicGroupItem that are realted to CharacteristicGroupUsage to be deleted  *******/
delete from CharacteristicGroupItem
where CharacteristicGroupUsageId in (
	   select id
	   from CharacteristicGroupUsage chgu
	   where not exists (
		  select 1 from #CharacteristicGroupUsage_Expected chgu_e
		  where ( (chgu.[CharacteristicGroupId] = chgu_e.[CharacteristicGroupId]) AND
				(chgu.[PropertyTypeId] = chgu_e.[PropertyTypeId]) AND
				(chgu.[CountryId] = chgu_e.[CountryId]) )
	   )
)

/******* End-Of: Remove items form table CharacteristicGroupItem that are realted to CharacteristicGroupUsage to be deleted *******/

/******* Update table CharacteristicGroupUsage *******/
MERGE CharacteristicGroupUsage AS T
	USING #CharacteristicGroupUsage_Expected AS S
	ON
	(
        (T.[CharacteristicGroupId] = S.[CharacteristicGroupId]) AND
	   (T.[PropertyTypeId] = S.[PropertyTypeId]) AND
	   (T.[CountryId] = S.[CountryId])
	)
	WHEN MATCHED THEN
		UPDATE SET
		T.[Order] = S.[GroupOrder],
		T.[DisplayLabel] = S.[DisplayGroupLabel]

	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([CharacteristicGroupId], [PropertyTypeId], [CountryId], [Order], [DisplayLabel])
		VALUES ([CharacteristicGroupId], [PropertyTypeId], [CountryId], [GroupOrder], [DisplayGroupLabel])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

/* ------ Test result: ------
SELECT c.IsoCode as Country
	 ,pt.Code as PropertyType
	 ,chg.Code as GroupCode
      ,gu.[Order]
      ,gu.DisplayLabel
FROM [KnightFrank.Antares].[dbo].[CharacteristicGroupUsage] gu
    inner join [KnightFrank.Antares].[dbo].Country c on gu.[CountryId] = c.Id
    inner join [KnightFrank.Antares].[dbo].PropertyType pt on gu.[PropertyTypeId] = pt.Id
    inner join [KnightFrank.Antares].[dbo].CharacteristicGroup chg on gu.[CharacteristicGroupId] = chg.Id
ORDER BY c.IsoCode,pt.Code,gu.[Order]
*/

/******* End-Of: Update table CharacteristicGroupUsage *******/


/******* Prepare data for CharacteristicGroupItem *******/
IF OBJECT_ID('tempDB..#CharacteristicGroupItem_Expected', 'U') IS NOT NULL
    DROP TABLE #CharacteristicGroupItem_Expected

CREATE TABLE #CharacteristicGroupItem_Expected (
     CharacteristicGroupUsageId UNIQUEIDENTIFIER,
	CharacteristicId UNIQUEIDENTIFIER,
	CharacteristicCode  NVARCHAR(250) NOT NULL
);
INSERT INTO #CharacteristicGroupItem_Expected
select
    chgu.Id as CharacteristicGroupUsageId,
    ch.Id as CharacteristicId,
    ch.Code as CharacteristicCode
from #ChracteristicsMapCsv csv
    inner join Characteristic ch on csv.CharacteristicCode = ch.Code
    inner join #CharacteristicGroupUsage_Expected chgu_e
	   on csv.CountryIsoCode = chgu_e.CountryIsoCode and csv.PropertyTypeCode = chgu_e.PropertyTypeCode and csv.CharacteristicGroupCode = chgu_e.CharacteristicGroupCode
    inner join CharacteristicGroupUsage chgu
	   on chgu_e.CountryId = chgu.CountryId and chgu_e.PropertyTypeId = chgu.PropertyTypeId and chgu_e.CharacteristicGroupId = chgu.CharacteristicGroupId

select * from #CharacteristicGroupItem_Expected
/******* End-Of: Prepare data for CharacteristicGroupItem *******/

/******* Update table CharacteristicGroupItem *******/
MERGE CharacteristicGroupItem AS T
	USING #CharacteristicGroupItem_Expected AS S
	ON
	(
        (T.[CharacteristicId] = S.[CharacteristicId]) AND
	   (T.[CharacteristicGroupUsageId] = S.[CharacteristicGroupUsageId])
	)

	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([CharacteristicId], [CharacteristicGroupUsageId])
		VALUES ([CharacteristicId], [CharacteristicGroupUsageId])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;

/* ------ Test result: ------
SELECT c.IsoCode as Country
	 ,pt.Code as PropertyType
	 ,chg.Code as GroupCode
	 ,ch.Code
FROM [KnightFrank.Antares].[dbo].[CharacteristicGroupItem] it
    inner join [KnightFrank.Antares].[dbo].[Characteristic] ch on ch.Id = it.[CharacteristicId]
    inner join [KnightFrank.Antares].[dbo].[CharacteristicGroupUsage] gu on gu.Id = it.[CharacteristicGroupUsageId]
    inner join [KnightFrank.Antares].[dbo].Country c on gu.[CountryId] = c.Id
    inner join [KnightFrank.Antares].[dbo].PropertyType pt on gu.[PropertyTypeId] = pt.Id
    inner join [KnightFrank.Antares].[dbo].CharacteristicGroup chg on gu.[CharacteristicGroupId] = chg.Id
ORDER BY c.IsoCode,pt.Code,gu.[Order],ch.Code
*/

/******* End-Of: Update table CharacteristicGroupItem *******/

DROP TABLE #ChracteristicsMapCsv
DROP TABLE #CharacteristicGroupUsage_Expected
DROP TABLE #CharacteristicGroupItem_Expected
