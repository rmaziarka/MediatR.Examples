Feature: Companies

@Company
Scenario: Create new company
	Given Contacts exists in database
		| FirstName | Surname | Title |
		| Michael   | Angel   | cheef | 
	When User creates company with mandatory fields using api
	Then User should get OK http status code
		And Company should be added to database

@Company
Scenario: Create company with invalid data
	Given Contacts exists in database
		| FirstName | Surname | Title |
		| Michael   | Angel   | cheef | 
	When User creates company using api
		| Name |
		|      |
	Then User should get BadRequest http status code
