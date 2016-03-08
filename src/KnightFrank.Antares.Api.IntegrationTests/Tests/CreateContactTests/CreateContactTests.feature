Feature: CreateContactTest

	Scenario: Create new contact
			When User creates a contact with following data
				| FirstName | Surname | Title |
				| Michael   | Angel   | cheef |
			When User retrieves contacts details for latest id
			Then User should get OK http status code
			And contact details should be the same as already added

