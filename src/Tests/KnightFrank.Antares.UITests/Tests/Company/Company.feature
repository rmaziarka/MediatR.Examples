Feature: Company UI tests

@Company
Scenario: Create company
	Given Contacts are created in database
		| FirstName | Surname | Title |
		| Indiana   | Jones   | Dr    |
		| Adam      | Sandler | Sir   |
	When User navigates to create company page
		And User fills in company details on create company page 
			| Name         | WebsiteUrl          | ClientCarePageUrl |
			| Knight Frank | www.knightfrank.com | www.test.com      | 
		And User selects contacts on create company page
			| FirstName | Surname |
			| Indiana   | Jones   |
			| Adam      | Sandler |
	Then List of company contacts should contain following contacts
		| FirstName | Surname |
		| Indiana   | Jones   |
		| Adam      | Sandler |
	When User clicks on website url icon 
	Then url opens in new tab
	When User clicks save company button on create company page
	Then  View company page is displayed with following details
	| Name         | WebsiteUrl                 | ClientCarePageUrl   |
	| Knight Frank | http://www.knightfrank.com | http://www.test.com |
	
