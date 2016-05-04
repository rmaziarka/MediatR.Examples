Feature: Company UI tests

@Company
Scenario: Create company
	Given Contacts are created in database
		| FirstName | Surname | Title |
		| Indiana   | Jones   | Dr    |
		| Adam      | Sandler | Sir   |
	When User navigates to create company page
		And User fills in company details on create company page
			| Name         |
			| Knight Frank |
		And User selects contacts on create company page
			| FirstName | Surname |
			| Indiana   | Jones   |
			| Adam      | Sandler |
	Then list of company contacts should contain following contacts
		| FirstName | Surname |
		| Indiana   | Jones   |
		| Adam      | Sandler |
	When User clicks save button on create company page
	Then New company should be created
