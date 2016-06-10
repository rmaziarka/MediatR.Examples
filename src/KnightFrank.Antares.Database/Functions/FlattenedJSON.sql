CREATE FUNCTION [dbo].FlattenedJSON (@XMLResult XML)
RETURNS NVARCHAR(MAX)
WITH EXECUTE AS CALLER
AS
BEGIN
    DECLARE @JSONVersion NVARCHAR(MAX), @Rowcount INT
    SELECT @JSONVersion = '', @rowcount=COUNT(*) FROM @XMLResult.nodes('/root/*') x(a)
    SELECT @JSONVersion = @JSONVersion +
        STUFF((
            SELECT TheLine 
            FROM (
                SELECT ', {' + STUFF((
                    SELECT ',"'
                            + COALESCE(b.c.value('local-name(.)', 'NVARCHAR(255)'),'') + '":"'
                            + REPLACE( --escape tab properly within a value
                                    REPLACE( --escape return properly
                                        REPLACE( --linefeed must be escaped
                                            REPLACE( --backslash too
                                                REPLACE(COALESCE(b.c.value('text()[1]','NVARCHAR(MAX)'),''),--forwardslash
                                                        '\', '\\'),
                                                '/', '\/'),
                                        CHAR(10),''),
                                    CHAR(13),''),
                                CHAR(09),'')
                            + '"'
                    FROM x.a.nodes('*') b(c)
                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') +'}'
                FROM @XMLResult.nodes('/root/*') x(a)
                ) JSON(theLine) FOR XML PATH(''),TYPE).value('.','NVARCHAR(MAX)' ),
                1,
                1,
                '')
    IF @Rowcount>1 
        RETURN '['+@JSONVersion+']'
    RETURN @JSONVersion
END
GO