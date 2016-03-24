Feature: Contacts
	
@Contacts
Scenario: Retrieve all contacts
	Given All contacts have been deleted
		And User creates contacts in database with following data
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| David     | Dummy   | Mister |
	When User retrieves all contact details
	Then User should get OK http status code
		And contact details should be the same as already added

@Contacts
Scenario Outline: Retrieve error messages for improper contact id
	When User retrieves contact details for <id> id
	Then User should get <statusCode> http status code

	Examples: 
	| id                                   | statusCode |
	| 00000000-0000-0000-0000-000000000000 | BadRequest |
	| A                                    | BadRequest |

@Contacts
Scenario: Create new contact
	Given All contacts have been deleted
		And User creates contact using api with following data
			| FirstName | Surname | Title |
			| Michael   | Angel   | cheef |
	When User retrieves contact details for latest id
	Then User should get OK http status code
		And contact details should be the same as already added

@Contacts
Scenario Outline: Check if validation is invoked 
	When Try to creates a contact with following data
		| FirstName   | Surname   | Title   |
		| <FirstName> | <Surname> | <Title> |
	Then User should get BadRequest http status code

	Examples: 
	| FirstName | Surname | Title |
	|           | Angel   | cheef |
	| Michael   |         | cheef |
	| Michael   | Angel   |       |
