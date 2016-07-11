Feature: Requirement notes

@Requirements
Scenario Outline: Save note to existing requirement
	Given Contacts exists in database
		| FirstName | LastName | Title  |
		| Tomasz    | Bien     | Mister |
		And Requirement of type <type> exists in database
	When User creates note for requirement using api
	Then User should get OK http status code
		And Note is saved in database

	Examples: 
	| type               |
	| ResidentialLetting |
	| ResidentialSale    |

@Requirements
Scenario: Save note to non existing requirement
	When User creates note for requirement using api
	Then User should get BadRequest http status code
