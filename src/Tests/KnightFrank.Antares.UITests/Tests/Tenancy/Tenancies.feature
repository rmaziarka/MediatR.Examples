Feature: Tenancies UI steps
	

@Tenancy
Scenario: Create tenancy for residential letting offer
Given Contacts are created in database	
		| Title | FirstName | LastName |
		| Sir   | Michael   | Jordan   |
		| Sir   | Jon       | King     |
		And Property with Residential division and House type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2      | Line3 | Postcode | City   | County |
			| 44             | Johnson Tower | Johnson St |       | L2       | London | London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 08-07-2008   | 300000   |
		And Property Open Market Letting activity is defined
		And Requirement for GB is created in database
			| Type                | Description |
			| Residential Letting | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type                | Status   |
			| Residential Letting | Accepted |
	When User navigates to view requirement page with id
		And User clicks create tenancy link for 1 offer on view requirement page
	Then Create tenancy page should be displayed
		And Tenants are displayed on create tenancy page
			| Tenants        |
			| Michael Jordan |
			| Jon King       |
		And Landlords are displayed on create tenenacy page
			| Landlords      |
			| Michael Jordan |
			| Jon King       |
		And Offer activity details on create tenancy page are same as the following
			| Details                                      |
			| 44 Johnson Tower Johnson St L2 London London |
		And Offer requirement details on create tenancy page are same as the following
			| Details                  |
			| Michael Jordan, Jon King |
	When Users fills in terms on create tenancy page
		| StartDate  | EndDate    | AggredRent |
		| 12-08-2016 | 20-08-2017 | 1000       |
		And User clicks save button on create tenancy page
	Then View tenancy page should be displayed
		And Terms details are following on view tenancy page
			| StartDate  | EndDate    | AggredRent |
			| 12-08-2016 | 20-08-2017 | 1000       |
		And Tenants are displayed on view tenancy page
			| Tenants        |
			| Michael Jordan |
			| Jon King       |
		And Landlords are displayed on view tenenacy page
			| Landlords      |
			| Michael Jordan |
			| Jon King       |
		And Offer activity details on view tenancy page are same as the following
			| Details                                      |
			| 44 Johnson Tower Johnson St L2 London London |
		And Offer requirement details on view tenancy page are same as the following
			| Details                  |
			| Michael Jordan, Jon King |
		And Tenancy title on view tenancy page is following
			| Title                                                   |
			| Michael Jordan, Jon King - Johnson Tower, 44 Johnson St |
	When User clicks details link from requirement card on view tenancy page
	Then View requirement page should be displayed
		And Tenancy details are displayed on view requirement page
			| Title                        | Date                    |
			| Johnson Tower, 44 Johnson St | 12-08-2016 - 20-08-2017 |
	When User clicks details link from tenancy card on view requirement page
		Then View tenancy page should be displayed   
	
@Tenancy
Scenario: Update tenancy for residential letting offer
Given Contacts are created in database	
		| Title | FirstName | LastName |
		| Lady  | Irena     | Parker   |
		| Dr    | Sebastian | Langdon  |
		And Property with Residential division and House type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2     | Line3 | Postcode | City      | County    |
			| 12             | Sky Tower    | Dustin St |       | L8       | Liverpool | Liverpool |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-12-1997   | 500000   |
		And Property Open Market Letting activity is defined
		And Requirement for GB is created in database
			| Type                | Description |
			| Residential Letting | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type                | Status   |
			| Residential Letting | Accepted |
		And Tenancy is created in database
	When User navigates to view tenancy page with id
		And User clicks edit button on view tenancy page
	Then Edit tenancy page should be displayed	
		And Offer activity details on edit tenancy page are same as the following
			| Details                                      |
			| 12 Sky Tower Dustin St L8 Liverpool Liverpool|
		And Offer requirement details on edit tenancy page are same as the following
			| Details                         |
			| Irena Parker, Sebastian Langdon |
	When User updates terms on edit tenancy page
		| StartDate  | EndDate    | AggredRent |
		| 15-07-2015 | 21-10-2019 | 1500       |
		And User clicks save button on edit tenancy page
	Then View tenancy page should be displayed
		And Terms details are following on view tenancy page
			| StartDate  | EndDate    | AggredRent |
			| 15-07-2015 | 21-10-2019 | 1500       |
		And Offer activity details on view tenancy page are same as the following
			| Details                                       |
			| 12 Sky Tower Dustin St L8 Liverpool Liverpool |
		And Offer requirement details on view tenancy page are same as the following
			| Details                         |
			| Irena Parker, Sebastian Langdon |
		And Tenancy title on view tenancy page is following
			| Title                   |
			| Sky Tower, 12 Dustin St |
	When User clicks details link from requirement card on view tenancy page
	Then View requirement page should be displayed
		And Tenancy details are displayed on view requirement page
			| Title                   | Date                    |
			| Sky Tower, 12 Dustin St | 15-07-2015 - 21-10-2019 |
	When User clicks edit link from tenancy card on view requirement page
		Then Edit tenancy page should be displayed    
