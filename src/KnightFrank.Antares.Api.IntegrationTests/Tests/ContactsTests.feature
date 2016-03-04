	Feature: Contacts

	Scenario: Retrieve single contact details
		Given User has defined a contact details
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		When User retrieves 'specific' contacts details
		Then contact should have following details
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |


	Scenario: Retrieve all contacts details
	Given User has defined a contact details
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		| David     | Dummy   | Mister |
		When User retrieves 'all' contacts details
		Then contacts should have following details
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		| David     | Dummy   | Mister |

