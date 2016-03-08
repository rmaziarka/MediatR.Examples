	Feature: GetContacts

	Scenario: Retrieve all contacts details
			Given All contacts have been deleted
			When User creates a contact with following data
				| FirstName | Surname | Title |
				| Michael   | Angel   | cheef |
				| Michael   | Angel   | slave |
				| Tomasz    | Bien    | cheef |
				| Tomasz    | Bien    | slave |
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
		| id  | statusCode |
		| -2  | NotFound   |
		| "A" | BadRequest |



