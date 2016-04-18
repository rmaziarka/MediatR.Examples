Feature: CompaniesController

@Company
Scenario: Create new company
	Given User creates contacts in database with following data
		    | FirstName | Surname | Title |
		    | Michael   | Angel   | cheef | 
	When User create company by API for contact for maximum name length
	Then User should get OK http status code
		And company should be added to data base

@Company
Scenario Outline: Check if validation is invoked 
	Given User creates contacts in database with following data
		    | FirstName | Surname | Title |
		    | Michael   | Angel   | cheef | 
	When User create company by API for contact
		| Name    |
		| <name> |
	Then User should get <statusCode> http status code

	Examples: 
	| name | statusCode |
	|      | BadRequest |

