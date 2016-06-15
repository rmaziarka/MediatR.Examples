Feature: Companies

@Company
Scenario: Create new company
	Given Contacts exists in database
		| FirstName | Surname | Title |
		| Michael   | Angel   | cheef | 
	When User creates company by API for contact for maximum name length
	Then User should get OK http status code
		And Company should be added to database

@Company
Scenario Outline: Create company with invalid data
	Given Contacts exists in database
		| FirstName | Surname | Title |
		| Michael   | Angel   | cheef | 
	When User creates company by API for contact
		| Name   |
		| <name> |
	Then User should get <statusCode> http status code

	Examples: 
	| name | statusCode |
	|      | BadRequest |
