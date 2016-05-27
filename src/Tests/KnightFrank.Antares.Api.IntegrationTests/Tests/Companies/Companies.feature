Feature: Companies

@Company
Scenario: Create new company with required fields
	Given User creates contacts in database with following data
		| FirstName | Surname | Title |
		| Michael   | Angel   | cheef | 
	When User creates company by API for contact for maximum name length
	Then User should get OK http status code
		And Company should be added to database


@Company
Scenario Outline: Create new company with all fields
	Given User creates contacts in database with following data
		| FirstName | Surname | Title |
		| Michael   | Angel   | Mr |
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
	Given User creates contacts in database with following data
		| FirstName | Surname | Title |
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
Scenario: Update company with required fields
	Given User creates contacts in database with following data
		| FirstName | Surname | Title |
		| Michael   | Angel   | cheef | 
	And User creates company in database with following data
	 	| Name         | WebsiteUrl  | ClientCarePageUrl  | ClientCareStatus    |
	 	| Test Company | www.api.com | www.clientcare.com | MassiveActionClient |
	When User updates company by API with maximum length fields
	Then User should get OK http status code
		And Company should be updated
