Feature: Contacts

@Contacts
Scenario: Create contact
	Given All contacts have been deleted
	When User creates contact using api with max length fields
	Then User should get OK http status code
		And Contact details should be the same as already added

@Contacts
Scenario Outline: Create contact using invalid data
	When User creates contact using api with following data
		| FirstName   | Surname   | Title   |
		| <firstName> | <surname> | <title> |
	Then User should get BadRequest http status code

	Examples: 
	| firstName | surname | title |
	|           | Angel   | cheef |
	| Michael   |         | cheef |
	| Michael   | Angel   |       |

@Contacts
Scenario: Get all contacts
	Given All contacts have been deleted
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| David     | Dummy   | Mister |
	When User retrieves all contact details
	Then User should get OK http status code
		And Contacts details should have expected values

@Contacts
Scenario: Get contact
	Given All contacts have been deleted
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User retrieves contact details for latest id
	Then User should get OK http status code
		And Get contact details should be the same as already added

@Contacts
Scenario Outline: Get contact using invalid data
	When User retrieves contact details for <id> id
	Then User should get <statusCode> http status code

	Examples: 
	| id                                   | statusCode |
	| 00000000-0000-0000-0000-000000000000 | BadRequest |
	| A                                    | BadRequest |
	| invalid                              | NotFound   |
