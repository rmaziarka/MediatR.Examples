Feature: Contact UI tests

@Contact
Scenario: Create contact
	Given User navigates to create contact page
	When User fills in contact details on create contact page
		| FirstName | LastName  | Title |
		| Alan      | Macarthur | Sir   |
		And User clicks save contact button on create contact page
	Then New contact should be created
