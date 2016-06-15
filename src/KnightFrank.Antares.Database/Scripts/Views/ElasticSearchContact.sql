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
		c.Title AS 'Title'
	FROM [dbo].[Contact] c
	
GO
