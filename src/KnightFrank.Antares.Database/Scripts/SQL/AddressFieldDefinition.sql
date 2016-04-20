--Drop table #TempAddressFieldDefinitionCsv
--Drop table #MergeAddressFieldDefinition

/*
 Prepare temporary table to store data from .csv file
*/

CREATE TABLE #TempAddressFieldDefinitionCsv
                                           ( AddressFieldName     NVARCHAR(250) NOT NULL,
                                             AddressFieldLabelKey NVARCHAR(250) NOT NULL,
                                             CountryIsoCode       NVARCHAR(250) NOT NULL,
                                             EnumTypeItemCode     NVARCHAR(250),
                                             [Required]           BIT NOT NULL,
                                             RegEx                NVARCHAR(50) NULL,
                                             RowOrder             SMALLINT NOT NULL,
                                             ColumnOrder          SMALLINT NOT NULL,
                                             ColumnSize           SMALLINT NOT NULL,
                                           );
BULK INSERT #TempAddressFieldDefinitionCsv FROM '$(OutputPath)\Scripts\Data\Configuration\AddressFieldDefinition.csv' WITH(FIRSTROW = 2, FIELDTERMINATOR = ';', ROWTERMINATOR = '\n', TABLOCK);

/*
	Merge table with data from .csv file and database,
	will have table with all necessary data.
*/

CREATE TABLE #MergeAddressFieldDefinition
                                         ( AddressFormId          UNIQUEIDENTIFIER,
                                           AddressFormEntityTyeId UNIQUEIDENTIFIER,
                                           AddressFieldId         UNIQUEIDENTIFIER NOT NULL,
                                           AddressFieldName       NVARCHAR(250) NOT NULL,
                                           AddressFieldLabelId    UNIQUEIDENTIFIER NOT NULL,
                                           AddressFieldLabelKey   NVARCHAR(250) NOT NULL,
                                           CountryIsoCode         NVARCHAR(250) NOT NULL,
                                           CountryId              UNIQUEIDENTIFIER NOT NULL,
                                           EnumTypeItemId         UNIQUEIDENTIFIER NOT NULL,
                                           EnumTypeItemCode       NVARCHAR(250),
                                           [Required]             BIT,
                                           RegEx                  NVARCHAR(50),
                                           RowOrder               INT,
                                           ColumnOrder            INT,
                                           ColumnSize             INT
                                         );
INSERT INTO #MergeAddressFieldDefinition
                                        ( AddressFieldId,
                                          AddressFieldName,
                                          AddressFieldLabelId,
                                          AddressFieldLabelKey,
                                          CountryId,
                                          CountryIsoCode,
                                          EnumTypeItemId,
                                          EnumTypeItemCode,
                                          [Required],
                                          RegEx,
                                          RowOrder,
                                          ColumnOrder,
                                          ColumnSize
                                        )
       SELECT af.Id,
              af.Name,
              afl.Id,
              afl.LabelKey,
              c.Id,
              c.IsoCode,
              eti.Id,
              eti.Code,
              tmpCsv.[Required],
              tmpCsv.RegEx,
              tmpCsv.RowOrder,
              tmpCsv.ColumnOrder,
              tmpCsv.ColumnSize
       FROM #TempAddressFieldDefinitionCsv AS tmpCsv
            JOIN dbo.Country AS c ON c.IsoCode = tmpCsv.CountryIsoCode
       JOIN dbo.AddressField AS af ON af.Name = tmpCsv.AddressFieldName
       JOIN dbo.AddressFieldLabel AS afl ON afl.LabelKey = tmpCsv.AddressFieldLabelKey
       JOIN dbo.EnumTypeItem AS eti ON eti.Code = tmpCsv.EnumTypeItemCode;

/*
	Fill Merge temporary table with data
*/

CREATE TABLE #ExistingAddressForms
                                  ( AddressFormId          UNIQUEIDENTIFIER,
                                    AddressFormEntityTyeId UNIQUEIDENTIFIER,
                                    CountryIsoCode         NVARCHAR(250) NOT NULL,
                                    CountryId              UNIQUEIDENTIFIER NOT NULL,
                                    EnumTypeItemId         UNIQUEIDENTIFIER NOT NULL,
                                    EnumTypeItemCode       NVARCHAR(250)
                                  );
INSERT INTO #ExistingAddressForms
       SELECT af.Id,
              afet.Id,
              c.IsoCode,
              af.CountryId,
              afet.EnumTypeItemId,
              eti.Code
       FROM dbo.AddressForm AS af
            JOIN dbo.Country AS c ON c.Id = af.CountryId
       JOIN dbo.AddressFormEntityType AS afet ON afet.AddressFormId = af.Id
       JOIN dbo.EnumTypeItem AS eti ON eti.Id = afet.EnumTypeItemId;
--SELECT #MergeAddressFieldDefinition.CountryId,
--       #MergeAddressFieldDefinition.CountryIsoCode,
--       #MergeAddressFieldDefinition.EnumTypeItemId,
--       #MergeAddressFieldDefinition.EnumTypeItemCode
--FROM #MergeAddressFieldDefinition
--GROUP BY #MergeAddressFieldDefinition.CountryId,
--         #MergeAddressFieldDefinition.CountryIsoCode,
--         #MergeAddressFieldDefinition.EnumTypeItemId,
--         #MergeAddressFieldDefinition.EnumTypeItemCode;
CREATE TABLE #AddressFormToDelete
                                 ( AddressFormId UNIQUEIDENTIFIER
                                 );
CREATE TABLE #AddressFormToInsert
                                 ( CountryId      UNIQUEIDENTIFIER NOT NULL,
                                   EnumTypeItemId UNIQUEIDENTIFIER NULL
                                 );
INSERT INTO #AddressFormToDelete
       SELECT ef.AddressFormId
       FROM #ExistingAddressForms AS ef
       WHERE NOT EXISTS( SELECT 1
                         FROM #MergeAddressFieldDefinition AS mergeA
                         WHERE mergeA.CountryId = ef.CountryId
                               AND ( mergeA.EnumTypeItemId = ef.EnumTypeItemId
                                     OR mergeA.EnumTypeItemId IS NULL
                                     AND ef.EnumTypeItemId IS NULL
                                   ) );
INSERT INTO #AddressFormToInsert
       SELECT mergeA.CountryId,
              mergeA.EnumTypeItemId
       FROM #MergeAddressFieldDefinition AS mergeA
       WHERE NOT EXISTS( SELECT 1
                         FROM #ExistingAddressForms AS ef
                         WHERE mergeA.CountryId = ef.CountryId
                               AND ( mergeA.EnumTypeItemId = ef.EnumTypeItemId
                                     OR mergeA.EnumTypeItemId IS NULL
                                     AND ef.EnumTypeItemId IS NULL
                                   ) );
--DELETE FROM AddressFormEntityType
--WHERE AddressFormId IN
--                      (
--                        SELECT AddressFormId
--                        FROM #AddressFormToDelete
--                      );
--DELETE FROM AddressForm
--WHERE Id IN
--           (
--             SELECT AddressFormId
--             FROM #AddressFormToDelete
--           );
--DECLARE
--   @CountryId      UNIQUEIDENTIFIER,
--   @EnumTypeItemId UNIQUEIDENTIFIER;
--DECLARE address_cursor CURSOR
--FOR SELECT CountryId,
--           EnumTypeItemId
--    FROM #AddressFormToInsert;
--OPEN address_cursor;
--FETCH NEXT FROM address_cursor INTO
--   @CountryId,
--   @EnumTypeItemId;
--WHILE @@FETCH_STATUS = 0
--    BEGIN
--        INSERT INTO AddressForm
--                               ( CountryId
--                               )
--        VALUES
--               ( @CountryId
--               );
--        DECLARE
--           @newAfId UNIQUEIDENTIFIER = IDENT_CURRENT( 'AddressForm' );
--        IF @EnumTypeItemId IS NOT NULL
--            BEGIN
--                INSERT INTO AddressFormEntityType
--                                                 ( AddressFormId,
--                                                   EnumTypeItemId
--                                                 )
--                VALUES
--                       ( @newAfId, @EnumTypeItemId
--                       );
--            END;
--        FETCH NEXT FROM address_cursor INTO
--           @CountryId,
--           @EnumTypeItemId;
--    END;

IF( SELECT CURSOR_STATUS('global', 'insert_address_csv_cursor') ) >= -1
    BEGIN
        IF( SELECT CURSOR_STATUS('global', 'insert_address_csv_cursor') ) > -1
            BEGIN
                CLOSE insert_address_csv_cursor;
            END;
        DEALLOCATE insert_address_csv_cursor;
    END;
DECLARE
   @CountryIsoCode   NVARCHAR(250),
   @EnumTypeItemCode NVARCHAR(250);
DECLARE insert_address_csv_cursor CURSOR
FOR SELECT #TempAddressFieldDefinitionCsv.CountryIsoCode,
           #TempAddressFieldDefinitionCsv.EnumTypeItemCode
    FROM #TempAddressFieldDefinitionCsv
    GROUP BY CountryIsoCode,
             EnumTypeItemCode
    HAVING EnumTypeItemCode IS NOT NULL;
OPEN insert_address_csv_cursor;
FETCH NEXT FROM insert_address_csv_cursor INTO
   @CountryIsoCode,
   @EnumTypeItemCode;
WHILE @@FETCH_STATUS = 0
    BEGIN
        --split code
        DECLARE
           @str        NVARCHAR(250),
           @singleCode NVARCHAR(250),
           @commaIdx   INT;
        DECLARE
           @csvEntityCodes TABLE
                                ( Code NVARCHAR(250)
                                );
        SET @str = @EnumTypeItemCode;
        --BEGIN WHILE
        WHILE @str <> ''
            BEGIN
                DELETE FROM @csvEntityCodes;
                SET @commaIdx = CHARINDEX(',', @str, 1);
                IF @commaIdx = 0
                    BEGIN
                        INSERT INTO @csvEntityCodes
                        VALUES
                               ( @str
                               );
                        SET @str = '';
                    END;
                ELSE
                    BEGIN
                        INSERT INTO @csvEntityCodes
                        VALUES
                               ( SUBSTRING(@str, 1, @commaIdx-1)
                               );
                        SET @str = SUBSTRING(@str, @commaIdx+1, 4000);
                    END;
            END;--END WHILE
        --@CountryIsoCode
        --@singleCode
        --Sprawdź czy jest jakikolwiek addressForm zdefiniowany dla któregokolwiek z entity type item codeów z pliku CSV
        DECLARE
           @existingAddressFormId UNIQUEIDENTIFIER = ( SELECT TOP 1 af.Id
                                                       FROM dbo.AddressForm AS af
                                                            JOIN dbo.Country AS c ON c.Id = af.CountryId
                                                       JOIN dbo.AddressFormEntityType AS afet ON afet.AddressFormId = af.Id
                                                       JOIN dbo.EnumTypeItem AS eti ON eti.Id = afet.EnumTypeItemId
                                                       WHERE c.IsoCode = @CountryIsoCode
                                                             AND eti.Code IN(
                                                                              SELECT Code
                                                                              FROM @csvEntityCodes
                                                                            ) );
        IF @existingAddressFormId IS NULL --adressForm not exist for a given Country code and Enum type item
            BEGIN
                DECLARE
                   @AfId TABLE
                              ( FormId UNIQUEIDENTIFIER
                              );
                INSERT dbo.AddressForm
                                      ( CountryId
                                      )
                OUTPUT INSERTED.Id  -- way to get inserted value right after insert used beceouse od guid
                       INTO @AfId
                       SELECT TOP 1 Id
                       FROM Country
                       WHERE IsoCode = @CountryIsoCode;
                SET @existingAddressFormId = ( SELECT TOP 1 FormId
                                               FROM @AfId );
                DELETE FROM @AfId;
                IF( SELECT COUNT(*)
                    FROM @csvEntityCodes ) > 0
                    BEGIN
                        INSERT INTO dbo.AddressFormEntityType
                                                             ( AddressFormId,
                                                               EnumTypeItemId
                                                             )
                               SELECT @existingAddressFormId,
                                     eti.Id
                               FROM @csvEntityCodes AS csvEC
                                    JOIN EnumTypeItem AS eti ON csvEC.Code = eti.Code;
                    END;
            END;
        ELSE --adressForm exist for a given Country code and at least one of Enum type item we need to add missing entity type items in to AddressFormEntityTypeItem table
            BEGIN
                INSERT INTO dbo.AddressFormEntityType
                                                     ( EnumTypeItemId,
                                                       AddressFormId
                                                     )
                       SELECT eti.Id,
                              @existingAddressFormId
                       FROM @csvEntityCodes AS cec
                            JOIN dbo.EnumTypeItem AS eti ON eti.Code = cec.Code -- and type.code = 'EntityType'
                       WHERE NOT EXISTS( SELECT *
                                         FROM dbo.AddressFormEntityType AS afet
                                         WHERE afet.EnumTypeItemId = eti.Id
                                               AND afet.AddressFormId = @existingAddressFormId );
            END;

        -- insert address field definition
        MERGE dbo.AddressFieldDefinition AS T
        USING( SELECT mafd.AddressFieldId,
                      mafd.AddressFieldLabelId,
                      @existingAddressFormId AS AddressFormId,
                      mafd.[Required],
                      mafd.RegEx,
                      mafd.RowOrder,
                      mafd.ColumnOrder,
                      mafd.ColumnSize
               FROM #MergeAddressFieldDefinition mafd
               WHERE mafd.CountryIsoCode = @CountryIsoCode
                     AND mafd.EnumTypeItemCode = @EnumTypeItemCode ) AS S
        ON( ( T.AddressFieldId = S.AddressFieldId
              AND T.AddressFieldLabelId = S.AddressFieldLabelId
              AND T.AddressFormId = S.AddressFormId
            )
          )
            WHEN MATCHED
            THEN UPDATE SET T.[AddressFieldId] = S.[AddressFieldId], T.[AddressFieldLabelId] = S.[AddressFieldLabelId], T.[AddressFormId] = S.[AddressFormId], T.[Required] = S.[Required], T.[RegEx] = S.[RegEx], T.[RowOrder] = S.[RowOrder], T.[ColumnOrder] = S.[ColumnOrder], T.[ColumnSize] = S.[ColumnSize]
            WHEN NOT MATCHED BY TARGET
            THEN INSERT([AddressFieldId],
                        [AddressFieldLabelId],
                        [AddressFormId],
                        [Required],
                        [RegEx],
                        [RowOrder],
                        [ColumnOrder],
                        [ColumnSize]) VALUES
                                             ( [AddressFieldId], [AddressFieldLabelId], [AddressFormId], [Required], [RegEx], [RowOrder], [ColumnOrder], [ColumnSize]
                                             );
        FETCH NEXT FROM insert_address_csv_cursor INTO
           @CountryIsoCode,
           @EnumTypeItemCode;
    END;
CLOSE insert_address_csv_cursor;
DEALLOCATE insert_address_csv_cursor;

--Handle empty enum type item
--DECLARE insert_empty_address_csv_cursor CURSOR
--FOR SELECT #TempAddressFieldDefinitionCsv.CountryIsoCode,
--           #TempAddressFieldDefinitionCsv.EnumTypeItemCode
--    FROM #TempAddressFieldDefinitionCsv
--    GROUP BY CountryIsoCode,
--             EnumTypeItemCode
--    HAVING EnumTypeItemCode IS NULL;
--OPEN insert_address_csv_cursor;
--FETCH NEXT FROM insert_address_csv_cursor INTO
--   @CountryIsoCode,
--   @EnumTypeItemCode;
--WHILE @@FETCH_STATUS = 0
--    BEGIN
--        --split code
--        DECLARE
--           @str        NVARCHAR(250),
--           @singleCode NVARCHAR(250),
--           @commaIdx   INT;
--        DECLARE
--           @csvEntityCodes TABLE
--                                ( Code NVARCHAR(250)
--                                );
--        SET @str = @EnumTypeItemCode;
--        --BEGIN WHILE
--        WHILE @str <> ''
--            BEGIN
--                DELETE FROM @csvEntityCodes;
--                SET @commaIdx = CHARINDEX(',', @str, 1);
--                IF @commaIdx = 0
--                    BEGIN
--                        INSERT INTO @csvEntityCodes
--                        VALUES
--                               ( @str
--                               );
--                        SET @str = '';
--                    END;
--                ELSE
--                    BEGIN
--                        INSERT INTO @csvEntityCodes
--                        VALUES
--                               ( SUBSTRING(@str, 1, @commaIdx-1)
--                               );
--                        SET @str = SUBSTRING(@str, @commaIdx+1, 4000);
--                    END;
--            END;--END WHILE
--        --@CountryIsoCode
--        --@singleCode
--        --Sprawdź czy jest jakikolwiek addressForm zdefiniowany dla któregokolwiek z entity type item codeów z pliku CSV
--        DECLARE
--           @existingAddressFormId UNIQUEIDENTIFIER = ( SELECT TOP 1 af.Id
--                                                       FROM dbo.AddressForm AS af
--                                                            JOIN dbo.Country AS c ON c.Id = af.CountryId
--                                                       JOIN dbo.AddressFormEntityType AS afet ON afet.AddressFormId = af.Id
--                                                       JOIN dbo.EnumTypeItem AS eti ON eti.Id = afet.EnumTypeItemId
--                                                       WHERE c.IsoCode = @CountryIsoCode
--                                                             AND eti.Code IN(
--                                                                              SELECT Code
--                                                                              FROM @csvEntityCodes
--                                                                            ) );
--        IF @existingAddressFormId IS NULL --adressForm not exist for a given Country code and Enum type item
--            BEGIN
--                DECLARE
--                   @AfId TABLE
--                              ( FormId UNIQUEIDENTIFIER
--                              );
--                INSERT dbo.AddressForm
--                                      ( CountryId
--                                      )
--                OUTPUT INSERTED.Id  -- way to get inserted value right after insert used beceouse od guid
--                       INTO @AfId
--                       SELECT TOP 1 Id
--                       FROM Country
--                       WHERE IsoCode = @CountryIsoCode;
--                IF( SELECT COUNT(*)
--                    FROM @csvEntityCodes ) > 0
--                    BEGIN
--                        INSERT INTO dbo.AddressFormEntityType
--                                                             ( AddressFormId,
--                                                               EnumTypeItemId
--                                                             )
--                               SELECT( SELECT TOP 1 FormId
--                                       FROM @AfId ),
--                                     eti.Id
--                               FROM @csvEntityCodes AS csvEC
--                                    JOIN EnumTypeItem AS eti ON csvEC.Code = eti.Code;
--                    END;
--            END;
--        ELSE --adressForm exist for a given Country code and at least one of Enum type item we need to add missing entity type items in to AddressFormEntityTypeItem table
--            BEGIN
--                INSERT INTO dbo.AddressFormEntityType
--                                                     ( EnumTypeItemId,
--                                                       AddressFormId
--                                                     )
--                       SELECT eti.Id,
--                              @existingAddressFormId
--                       FROM @csvEntityCodes AS cec
--                            JOIN dbo.EnumTypeItem AS eti ON eti.Code = cec.Code -- and type.code = 'EntityType'
--                       WHERE NOT EXISTS( SELECT *
--                                         FROM dbo.AddressFormEntityType AS afet
--                                         WHERE afet.EnumTypeItemId = eti.Id
--                                               AND afet.AddressFormId = @existingAddressFormId );
--            END;
--        FETCH NEXT FROM insert_address_csv_cursor INTO
--           @CountryIsoCode,
--           @EnumTypeItemCode;
--    END;
-- DELETE
-- cursor po existing address form entity types
-- delete address form if country code not exist in csv
--split code by ,
-- while delete from addressFormEntityType
IF( SELECT CURSOR_STATUS('global', 'delete_address_csv_cursor') ) >= -1
    BEGIN
        IF( SELECT CURSOR_STATUS('global', 'delete_address_csv_cursor') ) > -1
            BEGIN
                CLOSE delete_address_csv_cursor;
            END;
        DEALLOCATE delete_address_csv_cursor;
    END;
DECLARE delete_address_csv_cursor CURSOR
FOR SELECT #TempAddressFieldDefinitionCsv.CountryIsoCode,
           #TempAddressFieldDefinitionCsv.EnumTypeItemCode
    FROM #TempAddressFieldDefinitionCsv;
OPEN delete_address_csv_cursor;
FETCH NEXT FROM delete_address_csv_cursor INTO
   @CountryIsoCode,
   @EnumTypeItemCode;
WHILE @@FETCH_STATUS = 0
    BEGIN
        FETCH NEXT FROM delete_address_csv_cursor INTO
           @CountryIsoCode,
           @EnumTypeItemCode;
    END;
CLOSE delete_address_csv_cursor;
DEALLOCATE delete_address_csv_cursor;

/*
ALTER TABLE AddressFieldDefinition NOCHECK CONSTRAINT ALL

WITH S AS
(
   SELECT tmpAf.CountryIsoCode, tmpAf.EnumTypeItemCode
   FROM #TempAddressFieldDefinitionCsv AS tmpCsv
   --INNER JOIN #MergeAddressForm tmpAf ON tmpAf.CountryIsoCode = tmpCsv.CountryIsoCode and tmpAf.EnumTypeItemCode = tmpCsv.EnumTypeItemCode
)

MERGE #MergeAddressForm AS T
USING S
ON (
	S.CountryIsoCode = T.CountryIsoCode AND
	S.EnumTypeItemCode = T.EnumTypeItemCode
)
WHEN NOT MATCHED BY TARGET THEN
	Insert (CountryId, CountryIsoCode, EnumTypeItemId, EnumTypeItemCode) Values (S.CountryId, S.EnumTypeItemId)
WHEN NOT MATCHED BY SOURCE THEN DELETE;




MERGE dbo.AddressFieldDefinition AS T
	USING (
    	Select
			(Select Id from AddressField where Name = tmp.AddressFieldName) As AddressFieldId,
			(Select Id From AddressFieldLabel where LabelKey = tmp.AddressFieldLabelKey ) as AddressFieldLabelId,
			(Select Id From Country where IsoCode = tmp.CountryIsoCode) as CountryId,
			(select Id From EnumTypeItem where Code = tmp.EnumTypeItemCode) as EnumTypeItemId,
			(select af.Id From AddressForm af
				left join AddressFormEntityType as afet on afet.AddressFormId = AddressFormId
				left join Country as c on c.Id = af.CountryId
				left join EnumTypeItem as eti on eti.Code = tmp.EnumTypeItemCode
				where c.IsoCode = tmp.CountryIsoCode and eti.Code = tmp.EnumTypeItemCode) as AddressFormId,
			tmp.[Required],
			tmp.RegEx,
			tmp.RowOrder,
			tmp.ColumnOrder,
			tmp.ColumnSize
			From #TempAddressFieldDefinition AS tmp
	) AS S
	ON
	(
		(
			T.AddressFieldId = S.AddressFieldId and
			T.AddressFieldLabelId = S.AddressFieldLabelId and
			T.AddressFormId = S.AddressFormId
		)
	)
	WHEN MATCHED THEN
		UPDATE SET
		T.[AddressFieldId] = S.[AddressFieldId],
		T.[AddressFieldLabelId] = S.[AddressFieldLabelId],
		T.[AddressFormId] = S.[AddressFormId],
		T.[Required] = S.[Required],
		T.[RegEx] = S.[RegEx],
		T.[RowOrder] = S.[RowOrder],
		T.[ColumnOrder] = S.[ColumnOrder],
		T.[ColumnSize] = S.[ColumnSize]

	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([AddressFieldId], [AddressFieldLabelId], [AddressFormId], [Required], [RegEx], [RowOrder], [ColumnOrder], [ColumnSize])
		VALUES ([AddressFieldId], [AddressFieldLabelId], [AddressFormId], [Required], [RegEx], [RowOrder], [ColumnOrder], [ColumnSize])

    WHEN NOT MATCHED BY SOURCE THEN DELETE;


DROP TABLE #TempAddressFieldDefinition
*/

ALTER TABLE AddressFieldDefinition
WITH CHECK CHECK CONSTRAINT ALL;
DROP TABLE #TempAddressFieldDefinitionCsv;
DROP TABLE #MergeAddressFieldDefinition;
DROP TABLE #ExistingAddressForms;
DROP TABLE #AddressFormToDelete;
DROP TABLE #AddressFormToInsert;