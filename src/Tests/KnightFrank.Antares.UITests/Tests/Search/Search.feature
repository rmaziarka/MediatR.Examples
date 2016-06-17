Feature: Search UI tests

@Property
@Ignore
Scenario: Property search
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Sir   | John      | Johnson |
		And Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2           | Line3           | Postcode | City   | County |
			| 25             | Westpac House | 91 King William | 4 Albert Street | W1U 8AN  | London | London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 02-06-2013   | 2000000  |
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2           | Line3           | Postcode | City      | County    |
			| 25             | Westpac House | 91 King William | 4 Albert Street | W1U 8AN  | Liverpool | Liverpool |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 08-01-2011   | 3500000  |
	When User navigates to search property page
		And User searches for 25, Westpac House, 91 King William, 4 Albert Street, W1U 8AN, London, London on search property page
	Then Address details 25, Westpac House, 91 King William, 4 Albert Street, W1U 8AN, London, London are displayed on first property on search property page
		And Ownership details FREEHOLD - John Johnson are displayed on first property on search property page
	When User clicks on first property on search property page
	Then View property page should be displayed
		And Property should be displayed with address details
			| PropertyNumber | PropertyName  | Line2           | Line3           | Postcode | City   | County |
			| 25             | Westpac House | 91 King William | 4 Albert Street | W1U 8AN  | London | London |
