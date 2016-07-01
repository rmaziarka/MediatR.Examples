Feature: Requirement notes

@Requirements
Scenario: Save note to existing requirement
	Given Contacts exists in database
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		And Requirement of type ResidentialSale exists in database
	When User creates note for requirement using api
	Then User should get OK http status code
		And Note is saved in database

@Requirements
Scenario: Save note to non existing requirement
	When User creates note for requirement using api
	Then User should get BadRequest http status code
