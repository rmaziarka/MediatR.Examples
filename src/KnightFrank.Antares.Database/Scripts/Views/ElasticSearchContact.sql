IF EXISTS (SELECT TABLE_NAME 
			FROM INFORMATION_SCHEMA.VIEWS
			WHERE TABLE_NAME = 'ElasticSearchContact')
	DROP VIEW [dbo].[ElasticSearchContact]
GO

CREATE VIEW [dbo].[ElasticSearchContact]
AS
	SELECT
		-- ElasticSearch configuration
		c.[Id] AS '_id',
		'contact' AS '_type',
				
		-- Contact
		c.Id AS 'Id',
		c.FirstName AS 'FirstName',
		c.Surname AS 'Surname',
		c.Title AS 'Title',

		-- CompanyContacts
		cc.CompanyId AS 'CompanyContacts[CompanyId]',
		cc.ContactId AS 'CompanyContacts[ContactId]',
		comp.Id AS 'CompanyContacts[Company.Id]',
		comp.Name AS 'CompanyContacts[Company.Name]'

	FROM [dbo].[Contact] c
	LEFT JOIN [dbo].[CompanyContact] cc ON cc.[ContactId] = c.[Id]
	LEFT JOIN [dbo].[Company] comp ON comp.[Id] = cc.[CompanyId]
GO
