Feature: Offers

@Offers
Scenario: Create residential sales offer
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| OfferStatus            | New              |
		| ActivityStatus         | PreAppraisal     |
		| Division               | Residential      |
		| ActivityUserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		| ActivityDepartmentType | Standard         |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
	When User creates New offer using api
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Create residential sales offer with mandatory fields
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| OfferStatus            | Accepted         |
		| ActivityStatus         | PreAppraisal     |
		| Division               | Residential      |
		| ActivityUserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		| ActivityDepartmentType | Standard         |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
	When User creates Accepted offer with mandatory fields using api
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario Outline: Create residential sales offer with invalid data
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| OfferStatus            | New              |
		| ActivityStatus         | PreAppraisal     |
		| Division               | Residential      |
		| ActivityUserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		| ActivityDepartmentType | Standard         |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
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
		| enumTypeCode           | enumTypeItemCode |
		| OfferStatus            | New              |
		| ActivityStatus         | PreAppraisal     |
		| Division               | Residential      |
		| ActivityUserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		| ActivityDepartmentType | Standard         |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And User creates New offer in database
	When User gets offer for latest id
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Get residential sales offer with invalid data
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| OfferStatus            | New              |
		| ActivityStatus         | PreAppraisal     |
		| Division               | Residential      |
		| ActivityUserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		| ActivityDepartmentType | Standard         |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And User creates New offer in database
	When User gets offer for invalid id
	Then User should get NotFound http status code

@Offers
Scenario: Update residential sales offer
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| OfferStatus            | New              |
		| ActivityStatus         | PreAppraisal     |
		| Division               | Residential      |
		| ActivityUserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		| ActivityDepartmentType | Standard         |
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And User creates New offer in database
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| OfferStatus  | Accepted         |
	When User updates offer
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario Outline: Update residential sales offer with invalid data
	Given Activity exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And User creates New offer in database
	When User updates offer with invalid <data> data
	Then User should get BadRequest http status code

	Examples: 
	| data   |
	| status |
	| offer  |
