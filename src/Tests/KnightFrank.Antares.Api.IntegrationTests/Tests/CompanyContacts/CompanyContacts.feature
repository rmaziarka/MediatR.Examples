Feature: Company Contacts

@CompanyContacts
Scenario: Get company contacts
	Given Contacts exists in database
		| FirstName | LastName | Title  |
		| Adam      | Angel   | mister |
		| Angel     | Adam    | mister |
		| John      | Cena    | mister |
		And Company exists in database
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Adam      | Angel   | mister |
		And Company exists in database
	When User retrieves company contacts
	Then User should get OK http status code
		And Company contacts details should have expected values
