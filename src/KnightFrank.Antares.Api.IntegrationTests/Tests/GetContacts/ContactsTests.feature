	Feature: Contacts

	Scenario: Retrieve all contacts details
		Given User has defined multiple contact details
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		| David     | Dummy   | Mister |
		When User retrieves all contacts details
		Then User should get OK http status code
			And contacts should have following details
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| David     | Dummy   | Mister |
	
	
	Scenario: Retrieve single contact details
		Given User has defined a contact details
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		When User retrieves contacts details for proper id
		Then User should get OK http status code
			And contact should have same details as inserted


	Scenario Outline: Retrieve error messages for improper contact id
		Given User has defined a contact details
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		When User retrieves contacts details for <id> id
		Then User should get <statusCode> http status code

		Examples: 
		| id  | statusCode |
		| -2  | NotFound   |
		| "A" | BadRequest |



