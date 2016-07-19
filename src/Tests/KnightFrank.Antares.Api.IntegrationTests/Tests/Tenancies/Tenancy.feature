Feature: Tenancy 

@Tenancies
Scenario: Create tenancy
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType        |
			| PreAppraisal   | Open Market Letting |
		And Contacts exists in database
		| Title  | FirstName | LastName |
		| Mister | Tomasz    | Bien     |
		And Requirement of type ResidentialLetting exists in database
	When User creates tenancy for latest requirement and activity
	Then User should get OK http status code
		And Tenancy should be the same as added

@Tenancies
Scenario Outline: Create tenancy with invalid data
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Open Market Letting |
		And Contacts exists in database
		| Title  | FirstName | LastName |
		| Mister | Tomasz    | Bien     |
		And Requirement of type ResidentialLetting exists in database
	When User creates tenancy with invalid <data> using api
	Then User should get BadRequest http status code

	Examples: 
	| data        |
	| requirement |
	| activity    |

@Tenancies
Scenario: Update tenancy
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
		| Title  | FirstName | LastName |
		| Mister | Tomasz    | Bien     |
		And Requirement of type ResidentialLetting exists in database
		And tenancy exists in database
	When User updates tenancy with terms
	Then User should get OK http status code
		And Tenancy should be the same as added