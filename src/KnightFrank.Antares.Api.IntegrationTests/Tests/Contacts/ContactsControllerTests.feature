Feature: Contacts Controller tests
	
@Contacts
Scenario: Retrieve all contacts details
	Given All contacts have been deleted
	When User creates a contact with following data 
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		| David     | Dummy   | Mister |
	When User retrieves all contact details
	Then User should get OK http status code
		And contact details should be the same as already added

@Contacts
Scenario Outline: Retrieve error messages for improper contact id
		When User creates a contact with following data
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		When User retrieves contacts details for <id> id
		Then User should get <statusCode> http status code

		Examples: 
		| id                                   | statusCode |
		| 00000000-0000-0000-0000-000000000000 | NotFound   |
		| A                                    | BadRequest |

@Contacts
Scenario: Create new contact
		When User creates a contact with following data
			| FirstName | Surname | Title |
			| Michael   | Angel   | cheef |
		When User retrieves contacts details for latest id
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



