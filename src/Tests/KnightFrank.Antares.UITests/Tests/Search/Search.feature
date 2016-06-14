Feature: Search UI tests

@Property
Scenario: Property search
	Given Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2           | Line3           | Postcode | City   | County |
			| 25             | Westpac House | 91 King William | 4 Albert Street | W1U 8AN  | London | London |
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2           | Line3           | Postcode | City      | County    |
			| 25             | Westpac House | 91 King William | 4 Albert Street | W1U 8AN  | Liverpool | Liverpool |
	When User navigates to search property page
		And User searches for 25, Westpac House, 91 King William, 4 Albert Street, W1U 8AN, London, London on search property page
		And User clicks on first found property on search property page
	Then View property page should be displayed
		And Property should be displayed with address details
			| PropertyNumber | PropertyName  | Line2           | Line3           | Postcode | City   | County |
			| 25             | Westpac House | 91 King William | 4 Albert Street | W1U 8AN  | London | London |
