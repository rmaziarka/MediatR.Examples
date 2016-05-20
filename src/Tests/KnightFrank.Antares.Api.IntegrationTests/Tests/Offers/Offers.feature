Feature: Offers

@Offers
Scenario: Create residential sales offer
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode   | enumTypeItemCode |
		| OfferStatus    | New              |
		| ActivityStatus | PreAppraisal     |
		| Division       | Residential      |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms |
 			| 1000000  | 4000000  | 1           | 5           |
	When User creates New offer using api
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Create residential sales offer with mandatory fields
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode   | enumTypeItemCode |
		| OfferStatus    | Accepted         |
		| ActivityStatus | PreAppraisal     |
		| Division       | Residential      |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms |
 			| 1000000  | 4000000  | 1           | 5           |
	When User creates Accepted offer with mandatory fields using api
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario Outline: Create residential sales offer with invalid data
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode   | enumTypeItemCode |
		| OfferStatus    | New              |
		| ActivityStatus | PreAppraisal     |
		| Division       | Residential      |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms |
 			| 1000000  | 4000000  | 1           | 5           |
	When User creates offer with invalid <data> using api
	Then User should get BadRequest http status code

	Examples: 
	| data        |
	| requirement |
	| activity    |
	| status      |

@Offers
Scenario: Get residential sales offer
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode   | enumTypeItemCode |
		| OfferStatus    | New              |
		| ActivityStatus | PreAppraisal     |
		| Division       | Residential      |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms |
 			| 1000000  | 4000000  | 1           | 5           |
		And User creates New offer in database
	When User gets offer for latest id
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Get residential sales offer with invalid data
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode   | enumTypeItemCode |
		| OfferStatus    | New              |
		| ActivityStatus | PreAppraisal     |
		| Division       | Residential      |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms |
 			| 1000000  | 4000000  | 1           | 5           |
		And User creates New offer in database
	When User gets offer for invalid id
	Then User should get NotFound http status code
