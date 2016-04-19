
CREATE TABLE #TempUser (
	ActiveDirectoryDomain NVARCHAR (40) NULL,
	ActiveDirectoryLogin NVARCHAR (100) NOT NULL,
	FirstName NVARCHAR (40) NULL,
	LastName NVARCHAR (40) NULL,
	Business NVARCHAR (255) NOT NULL,
	CountryCode NVARCHAR (2) NOT NULL,
	Department NVARCHAR (100) NOT NULL,
	LocaleCode NVARCHAR (2) NOT NULL
);

ALTER TABLE [User] NOCHECK CONSTRAINT ALL

BULK INSERT #TempUser
    FROM '$(OutputPath)\Scripts\Data\Configuration\User.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.[User] AS T
	USING 
	(
	SELECT temp.ActiveDirectoryDomain,
		temp.ActiveDirectoryLogin,
		temp.FirstName,
		temp.LastName,
		B.Id AS BusinessId,
		D.Id AS DepartmentId,
		L.Id AS LocaleId,
		C.Id AS CountryId
	FROM #TempUser temp
	JOIN Business B ON B.Name = temp.Business
	JOIN Department D ON D.Name = temp.Department
	JOIN Locale L ON L.IsoCode = temp.LocaleCode
	JOIN Country C ON C.IsoCOde = temp.CountryCode
	)
	AS S	
	ON 
	(
        (T.ActiveDirectoryLogin = S.ActiveDirectoryLogin)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.ActiveDirectoryDomain = S.ActiveDirectoryDomain,
		T.ActiveDirectoryLogin = S.ActiveDirectoryLogin,
		T.FirstName = S.FirstName,
		T.LastName = S.LastName,
		T.BusinessId = S.BusinessId,
		T.CountryId = S.CountryId,
		T.DepartmentId = S.DepartmentId,
		T.LocaleId = S.LocaleId

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (ActiveDirectoryDomain, ActiveDirectoryLogin, FirstName, LastName, BusinessId, CountryId, DepartmentId, LocaleId)
		VALUES (ActiveDirectoryDomain, ActiveDirectoryLogin, FirstName, LastName, BusinessId, CountryId, DepartmentId, LocaleId)

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE [User] WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempUser
