Feature: Upward chains

Scenario: Create upward chain for residential sale activity
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Company exists in database
		And Requirement of type ResidentialSale exists in database
		And Offer with New status exists in database
	When User updates activity with upward chain
	Then User should get OK http status code
	When User gets offer for latest id
	Then User should get OK http status code
		And Upward chain transaction from offer activity should match transaction already added

Scenario: Create upward chain for residential sale activity with mandatory fields
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Company exists in database
		And Requirement of type ResidentialSale exists in database
		And Offer with New status exists in database
	When User updates activity with upward chain with mandatory fields
	Then User should get OK http status code
	When User gets offer for latest id
	Then User should get OK http status code
		And Upward chain transaction from offer activity should match transaction already added

@ignore
Scenario Outline: Create upward chain with invalid data
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Company exists in database
		And Requirement of type ResidentialSale exists in database
		And Offer with New status exists in database
	When User updates activity with upward chain with invalid <data> data
	Then User should get BadRequest http status code

	Examples: 
	| data               |
	| ActivityId         |
	| RequirementId      |
	| PropertyId         |
	| AgentCompanyId     |
	| AgentContactId     |
	| AgentUserId        |
	| SolicitorCompanyId |
	| SolicitorContactId |
	| MortgageId         |
	| SurveyId           |
	| SearchesId         |
	| EnquiriesId        |
	| ContractAgreedId   |
	| ParentId           |

@ignore
Scenario: Create upward chain for residential letting activity
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType        |
			| PreAppraisal   | Open Market Letting |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Company exists in database
		And Requirement of type ResidentialLetting exists in database
		And Offer with New status exists in database
	When User updates activity with upward chain
	Then User should get BadRequest http status code
