IF EXISTS (SELECT TABLE_NAME 
			FROM INFORMATION_SCHEMA.VIEWS
			WHERE TABLE_NAME = 'ElasticSearchProperty')
	DROP VIEW [dbo].[ElasticSearchProperty]
GO

CREATE VIEW [dbo].[ElasticSearchProperty]
AS
	SELECT
		-- ElasticSearch configuration
		p.[Id] AS '_id',
		'property' AS '_type',

		-- Property
		p.[Id] AS 'Id',
		p.[AddressId] AS 'AddressId',
		p.[PropertyTypeId] AS 'PropertyTypeId',
		p.[DivisionId] AS 'DivisionId',
		p.[AttributeValuesId] AS 'AttributeValuesId',
		p.[TotalAreaBreakdown] AS 'TotalAreaBreakdown',
		
		-- Address
		a.[Id] AS 'Address.Id',
		a.[CountryId] AS 'Address.CountryId',
		a.[AddressFormId] AS 'Address.AddressFormId',
		a.[PropertyName] AS 'Address.PropertyName',
		a.[PropertyNumber] AS 'Address.PropertyNumber',
		a.[Line1] AS 'Address.Line1',
		a.[Line2] AS 'Address.Line2',
		a.[Line3] AS 'Address.Line3',
		a.[Postcode] AS 'Address.Postcode',
		a.[City] AS 'Address.City',
		a.[County] AS 'Address.County',
		
		-- Address.Country
		c.[Id] AS 'Address.Country.Id',
		c.[IsoCode] AS 'Address.Country.IsoCode',

		-- PropertyType
		pt.[Id] AS 'PropertyType.Id',
		pt.[ParentId] AS 'PropertyType.ParentId',
		pt.[Code] AS 'PropertyType.Code',

		-- Division
		d.[Id] AS 'Division.Id',
		d.[Code] AS 'Division.Code',
		d.[EnumTypeId] AS 'Division.EnumTypeId',

		-- AttributeValues
		av.[Id] AS 'AttributeValues.Id',
		av.[MinBedrooms] AS 'AttributeValues.MinBedrooms',
		av.[MaxBedrooms] AS 'AttributeValues.MaxBedrooms',
		av.[MinReceptions] AS 'AttributeValues.MinReceptions',
		av.[MaxReceptions] AS 'AttributeValues.MaxReceptions',
		av.[MinBathrooms] AS 'AttributeValues.MinBathrooms',
		av.[MaxBathrooms] AS 'AttributeValues.MaxBathrooms',
		av.[MinArea] AS 'AttributeValues.MinArea',
		av.[MaxArea] AS 'AttributeValues.MaxArea',
		av.[MinLandArea] AS 'AttributeValues.MinLandArea',
		av.[MaxLandArea] AS 'AttributeValues.MaxLandArea',
		av.[MinGuestRooms] AS 'AttributeValues.MinGuestRooms',
		av.[MaxGuestRooms] AS 'AttributeValues.MaxGuestRooms',
		av.[MinFunctionRooms] AS 'AttributeValues.MinFunctionRooms',
		av.[MaxFunctionRooms] AS 'AttributeValues.MaxFunctionRooms',
		av.[MinCarParkingSpaces] AS 'AttributeValues.MinCarParkingSpaces',
		av.[MaxCarParkingSpaces] AS 'AttributeValues.MaxCarParkingSpaces',
				
		-- Ownerships
		o.[Id] AS 'Ownerships[Id]',		
		o.[PurchaseDate] AS 'Ownerships[PurchaseDate]',
		o.[SellDate] AS 'Ownerships[SellDate]',
		o.[PropertyId] AS 'Ownerships[PropertyId]',
		o.[BuyPrice] AS 'Ownerships[BuyPrice]',
		o.[SellPrice] AS 'Ownerships[SellPrice]',
		o.[OwnershipTypeId] AS 'Ownerships[OwnershipTypeId]',

		-- Ownerships.OwnershipType
		ot.[Id] AS 'Ownerships[OwnershipType.Id]',
		ot.[Code] AS 'Ownerships[OwnershipType.Code]',
		ot.[EnumTypeId] AS 'Ownerships[OwnershipType.EnumTypeId]',

		dbo.FlattenedJSON((
					Select 
						oc.[OwnershipId] AS 'OwnershipId',
						oc.[ContactId] AS 'ContactId'
					FROM [dbo].[OwnershipContact] oc
					WHERE oc.[OwnershipId] = o.[Id] FOR XML PATH, ROOT	
				 )) AS 'Ownerships[OwnershipContacts]'

	FROM [dbo].[Property] p
	JOIN [dbo].[Address] a ON p.[AddressId] = a.[Id]
	JOIN [dbo].[Country] c ON a.[CountryId] = c.[Id]
	JOIN [dbo].[PropertyType] pt ON p.[PropertyTypeId] = pt.[Id]
	JOIN [dbo].[EnumTypeItem] d ON p.[DivisionId] = d.[Id]
	JOIN [dbo].[AttributeValues] av ON p.[AttributeValuesId] = av.[Id]
	LEFT JOIN [dbo].[Ownership] o on o.[PropertyId] = p.[Id]
	LEFT JOIN [dbo].[EnumTypeItem] ot ON ot.[Id] = o.[OwnershipTypeId]
GO
