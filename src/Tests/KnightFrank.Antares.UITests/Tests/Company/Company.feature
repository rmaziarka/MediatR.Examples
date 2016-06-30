Feature: Company UI tests

@Company
Scenario: Create company
	Given Contacts are created in database
		| FirstName | LastName | Title |
		| Indiana   | Jones    | Dr    |
		| Adam      | Sandler  | Sir   |
	When User navigates to create company page
		And User fills in company details on create company page 
			| Name         | WebsiteUrl          | ClientCarePageUrl | ClientCareStatus      |
			| Knight Frank | www.knightfrank.com | www.test.com      | Massive Action Client |
		And User selects contacts on create company page
			| FirstName | LastName |
			| Indiana   | Jones    |
			| Adam      | Sandler  |
	Then List of company contacts should contain following contacts on create company page
		| FirstName | LastName |
		| Indiana   | Jones    |
		| Adam      | Sandler  |
	When User clicks on website url icon
	Then url opens in new tab
	When User clicks save company button on create company page
	Then View company page should be displayed
		And Company should have following details on view company page
			| Name         | WebsiteUrl                 | ClientCarePageUrl   | ClientCareStatus      |
			| Knight Frank | http://www.knightfrank.com | http://www.test.com | Massive Action Client |
		And Company contacts should have following contacts on view company page
			| FirstName | LastName |
			| Indiana   | Jones    |
			| Adam      | Sandler  |

@Company
Scenario: Edit company
	Given Contacts are created in database
		| FirstName | LastName | Title |
		| Marlon    | Brando   | Dr    |
		| Robert    | De Niro  | Sir   |
		And Company is created in database
			| Name   | WebsiteUrl             | ClientCarePageUrl      |
			| Adidas | https://www.google.com | https://www.google.com |
		And Contacts are created in database
			| FirstName | LastName | Title |
			| Humpty    | Dumpty   | Dr    |
			| Rudolf    | Rednose  | Sir   |
	When User navigates to view company page with id
		And User clicks edit company button on view company page
		And User fills in company details on edit company page 
			| Name        | WebsiteUrl          | ClientCarePageUrl | ClientCareStatus |
			| Objectivity | www.objectivity.com | www.test-2.com    | Principal Client |
		And User selects contacts on edit company page
			| FirstName | LastName |
			| Humpty    | Dumpty   |
			| Rudolf    | Rednose  |
	Then List of company contacts should contain following contacts on edit company page
		| FirstName | LastName |
		| Humpty    | Dumpty  |
		| Rudolf    | Rednose |
		| Marlon    | Brando  |
		| Robert    | De Niro |
	When User clicks on website url icon 
	Then url opens in new tab
	When User clicks save company button on edit company page
	Then View company page should be displayed
		And Company should have following details on view company page
			| Name        | WebsiteUrl                 | ClientCarePageUrl     | ClientCareStatus |
			| Objectivity | http://www.objectivity.com | http://www.test-2.com | Principal Client |
		And Company contacts should have following contacts on view company page
			| FirstName | LastName |
			| Humpty    | Dumpty  |
			| Rudolf    | Rednose |
			| Marlon    | Brando  |
			| Robert    | De Niro |
