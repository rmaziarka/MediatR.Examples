Feature: Requirement 

@Requirements
Scenario Outline: Create requirement
	Given User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User sets locations details for the requirement with max length fields
		And User creates <type> requirement using api
	Then User should get OK http status code
		And Requirement should be the same as added

	Examples: 
	| type               |
	| ResidentialLetting |
	| ResidentialSale    |

@Requirements
Scenario Outline: Create requirement with mandatory fields
	Given User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User creates <type> requirement with mandatory fields using api
	Then User should get OK http status code
		And Requirement should be the same as added

	Examples: 
	| type               |
	| ResidentialLetting |
	| ResidentialSale    |
		
@Requirements
Scenario Outline: Create requirement without data
	Given User gets GB address form for Property and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User sets locations details for the requirement
		| Postcode | City   | Line2   |
		| 1234     | London | Big Ben |
		And User creates following requirement without <data> using api
			| Description            | RentMin | RentMax |
			| RequirementDescription | 10      | 20      |
	Then User should get BadRequest http status code

	Examples: 
	| data         |
	| contact      |
	| country      |
	| address form |
	| type         |

@Requirements
Scenario: Create requirement with invalid contact
	Given User gets GB address form for Requirement and country details
	When User sets locations details for the requirement
		| Postcode | City   | Line2   |
		| 1234     | London | Big Ben |
		And User creates following requirement with invalid contact using api			
			| Description            | RentMin | RentMax |
			| RequirementDescription | 10      | 20      |
	Then User should get BadRequest http status code

@Requirements
Scenario Outline: Get requirement
	Given Contacts exists in database
		| Title  | FirstName | Surname |
		| Mister | Tomasz    | Bien    |
		And Requirement of type <type> exists in database
	When User retrieves requirement for latest id
	Then User should get OK http status code
		And Requirement should be the same as added

	Examples: 
	| type               |
	| ResidentialLetting |
	| ResidentialSale    |

@Requirements
Scenario Outline: Get requirement with notes
	Given Contacts exists in database
		| Title  | FirstName | Surname |
		| Mister | Tomasz    | Bien    |
		And Requirement of type <type> exists in database
		And Requirement notes exists in database
			| Description |
			| Note1       |
			| Note2       |
	When User retrieves requirement for latest id
	Then User should get OK http status code
		And Notes should be the same as added

	Examples: 
	| type               |
	| ResidentialLetting |
	| ResidentialSale    |

@Requirements
Scenario Outline: Get requirement with offer and viewing
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| Title  | FirstName | Surname |
			| Mister | Tomasz    | Bien    |
		And Requirement of type <type> exists in database
		And Offer with New status exists in database
		And Viewing exists in database
	When User retrieves requirement for latest id
	Then User should get OK http status code
		And Offer details in requirement should be the same as added
		And Viewing details in requirement should be the same as added

	Examples: 
	| type               |
	| ResidentialLetting |
	| ResidentialSale    |

@Requirements
Scenario Outline: Get requirement with invalid data		
	When User retrieves requirement for <id> id
	Then User should get <statusCode> http status code

	Examples: 
	| id                                   | statusCode |
	| 00000000-0000-0000-0000-000000000000 | NotFound   |
	| A                                    | BadRequest |
