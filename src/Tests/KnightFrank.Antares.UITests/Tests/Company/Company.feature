@ignore("test should be fixed after merges")
Feature: Company UI tests

@Company
Scenario: Create, view and edit company
	Given Contacts are created in database
		| FirstName | Surname | Title |
		| Indiana   | Jones   | Dr    |
		| Adam      | Sandler | Sir   |
		| Humpty    | Dumpty  | Sir   |
		| Rudolf    | Rednose | Mr    |
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
	When User clicks edit company button on view company page
	Then Edit company page is displayed with following details
		| Name         | WebsiteUrl                 | ClientCarePageUrl   |
		| Knight Frank | http://www.knightfrank.com | http://www.test.com |
	When User fills in company details on edit company page 
		| Name        | WebsiteUrl         | ClientCarePageUrl |
		| Objectivity | www.objectivity.com | www.test-2.com      | 
		And User selects contacts on edit company page
			| FirstName | Surname |
			| Humpty    | Dumpty  |
			| Rudolf    | Rednose |
	Then List of updated company contacts should contain following contacts
		| FirstName | Surname |
		| Indiana   | Jones   |
		| Adam      | Sandler |
		| Humpty    | Dumpty  |
		| Rudolf    | Rednose |
	When User clicks on website url icon 
	Then url opens in new tab
	When User clicks save company button on edit company page
	Then  View company page is displayed with following details
		| Name        | WebsiteUrl         | ClientCarePageUrl |
		| Objectivity | http://www.objectivity.com | http://www.test-2.com    |
