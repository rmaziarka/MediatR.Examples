Feature: Search UI tests

@Property
@Ignore
Scenario: Property search
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Sir   | John      | Johnson |
		And Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2           | Postcode |
			| 25             | Westpac House | 91 King William | W1U 8AN  |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 02-06-2013   | 2000000  |
	When User navigates to search property page
		And User waits 75 seconds
		And User searches for added property on search property page
	Then Proper address details are displayed on first property on search property page
		And Ownership details FREEHOLD - John Johnson are displayed on first property on search property page
	When User clicks on first property on search property page
	Then View property page should be displayed
		And Property should be displayed with address details
			| PropertyNumber | PropertyName  | Line2           | Postcode |
			| 25             | Westpac House | 91 King William | W1U 8AN  |
