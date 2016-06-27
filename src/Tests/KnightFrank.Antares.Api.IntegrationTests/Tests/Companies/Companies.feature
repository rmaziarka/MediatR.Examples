Feature: Companies

@Company
Scenario: Create new company with required fields
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | cheef | 
	When User creates company by API for contact for maximum name length
	Then User should get OK http status code
		And Company should be added to database

@Company
Scenario Outline: Create new company with all fields
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | Mr    |
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode     | enumTypeItemCode    |
			| ClientCareStatus | MassiveActionClient |
	 When User creates company by API with all fields
	 	| Name   | WebsiteUrl   | ClientCarePageUrl   | ClientCareStatus   |
	 	| <name> | <websiteUrl> | <clientCarePageUrl> | <clientCareStatus> |
	 Then User should get OK http status code
		 And Company should be added to database

	Examples:
	| name         | websiteUrl  | clientCarePageUrl  | clientCareStatus    |
	| Test Company | www.api.com | www.clientcare.com | MassiveActionClient |

@Company
Scenario Outline: Create company with invalid data
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | ceo | 
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode     |
			| <enumTypeCode> | <enumTypeItemCode> |
	When User creates company by API for contact
		| Name   | EnumTypeCode   | EnumTypeItemCode   |
		| <name> | <enumTypeCode> | <enumTypeItemCode> |
	Then User should get <statusCode> http status code

	Examples: 
	| name         | enumTypeCode     | enumTypeItemCode | statusCode |
	| Company Test | OfferStatus      | Accepted         | BadRequest |
	|              | ClientCareStatus | KeyClient        | BadRequest |

@Company
Scenario: Get non existant company
	Given Company does not exist
	When User gets company details
	Then User should get NotFound http status code

@Company
Scenario: Get company with invalid query
	When User gets company details with invalid query
	Then User should get BadRequest http status code

@Company
Scenario: Get company details
	Given Contacts exists in database
			| FirstName | LastName | Title |
			| Michael   | Angel   | ceo | 
		And Company exists in database
	When User gets company details
	Then User should get OK http status code
		And Company details should match those in database

@Company
Scenario: Update company with all fields
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | cheef | 
		And User creates company in database with following data
	 		| Name         | WebsiteUrl  | ClientCarePageUrl  | ClientCareStatus    |
	 		| Test Company | www.api.com | www.clientcare.com | MassiveActionClient |
	When User updates company by API
	Then User should get OK http status code
		And Company should be updated

@Company
Scenario: Update company with invalid data
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | cheef | 
		And User creates company in database with following data
	 		| Name         | WebsiteUrl  | ClientCarePageUrl  | ClientCareStatus    |
	 		| Test Company | www.api.com | www.clientcare.com | MassiveActionClient |
	When User updates company by API with invalid data
	Then User should get BadRequest http status code
