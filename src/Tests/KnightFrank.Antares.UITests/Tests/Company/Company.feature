Feature: Company UI tests

@Company
Scenario: Create company
	Given Contacts are created in database
		| FirstName | LastName | Title |
		| Indiana   | Jones    | Dr    |
		| Adam      | Sandler  | Sir   |
	When User navigates to create company page
		And User fills in company details on create company page
			| Name         |
			| Knight Frank |
		And User selects contacts on create company page
			| FirstName | LastName |
			| Indiana   | Jones    |
			| Adam      | Sandler  |
	Then List of company contacts should contain following contacts
		| FirstName | LastName |
		| Indiana   | Jones    |
		| Adam      | Sandler  |
	When User clicks save company button on create company page
	Then New company should be created
