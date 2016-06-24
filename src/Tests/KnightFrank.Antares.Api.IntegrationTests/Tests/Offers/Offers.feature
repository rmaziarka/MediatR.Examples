Feature: Offers

@Offers
Scenario: Create residential sales offer
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| ActivityStatus         | PreAppraisal     |
		| UserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		And User gets Freehold Sale for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
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
		| ActivityStatus         | PreAppraisal     |
		| UserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		And User gets Freehold Sale for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
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
		| ActivityStatus         | PreAppraisal     |
		| UserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		And User gets Freehold Sale for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
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
		| ActivityStatus         | PreAppraisal     |
		| UserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		And User gets Freehold Sale for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Offer with New status exists in database
	When User gets offer for latest id
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Get residential sales offer with invalid data
	When User gets offer for invalid id
	Then User should get NotFound http status code

@Offers
Scenario: Update residential sales offer
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| ActivityStatus         | PreAppraisal     |
		| UserType       | LeadNegotiator   |
		| ActivityDepartmentType | Managing         |
		And User gets Freehold Sale for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Offer with New status exists in database
	When User updates offer with Accepted status
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario Outline: Update residential sales offer with invalid data
	Given Activity exists in database
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Offer with New status exists in database
	When User updates offer with invalid <data> data
	Then User should get BadRequest http status code

	Examples: 
	| data   |
	| status |
	| offer  |
