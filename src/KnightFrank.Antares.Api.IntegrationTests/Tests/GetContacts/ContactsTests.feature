	Feature: GetContacts

	Scenario: Retrieve all contacts details
	Given All contacts have been deleted
		When User creates a contact with following data
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		| David     | Dummy   | Mister |
		When User retrieves all contacts details
		Then User should get OK http status code
			And contact details should be the same as already added

	


	Scenario Outline: Retrieve error messages for improper contact id
		When User creates a contact with following data
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		When User retrieves contacts details for <id> id
		Then User should get <statusCode> http status code

		Examples: 
		| id                                   | statusCode |
		| 00000000-0000-0000-0000-000000000000 | NotFound   |
		| "A"                                  | BadRequest |



