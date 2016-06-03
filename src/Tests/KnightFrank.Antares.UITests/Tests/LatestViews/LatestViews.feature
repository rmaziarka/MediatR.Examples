Feature: Latest views

@LatestViews
Scenario: Display latest viewed properties
	Given Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2       | Postcode | City          | County          |
			| 70             | Condo        | Longford St | TS1 4RN  | Middlesbrough | North Yorkshire |
		And Property Freehold Sale activity is defined
	When User navigates to view activity page with id
		And User clicks property details link on view activity page
		And User opens navigation drawer menu
		And User selects Properties menu item
	Then Latest 1 property should contain following data
		| LatestData            |
		| Condo, 70 Longford St |
	When Property in GB is created in database
		| PropertyNumber | PropertyName | Line2       | Postcode | City            | County |
		| 203            | Copier King  | Sherwood Rd | TN2 3LF  | Tunbridge Wells | Kent   |
		And User navigates to view property page with id
	Then View property page should be displayed
		And Latest 2 properties should contain following data
			| LatestData                                  |
			| Copier King, 203 Sherwood Rd                |
			| Condo, 70 Longford St                       |
	When User navigates to create property page
		And User selects United Kingdom country on create property page
		And User selects Residential property and Flat type on create property page
		And User fills in address details on create property page
			| PropertyNumber | PropertyName                     | Line2  | Postcode | City    | County     |
			| 10             | Tulketh Community Sports College | Tag Ln | PR2 3TX  | Preston | Lancashire |
		And User clicks save property button on create property page
	Then View property page should be displayed
	And Latest 3 properties should contain following data
		| LatestData                                  |
		| Tulketh Community Sports College, 10 Tag Ln |
		| Copier King, 203 Sherwood Rd                |
		| Condo, 70 Longford St                       |
	When User navigates to view property page with id
	Then View property page should be displayed
		And Latest 3 properties should contain following data
			| LatestData                                  |
			| Copier King, 203 Sherwood Rd                |
			| Tulketh Community Sports College, 10 Tag Ln |
			| Condo, 70 Longford St                       |
	When Property in GB is created in database
		| PropertyNumber | PropertyName | Line2   | Postcode | City      | County |
		| 98             | Cooper House | High St | ME14 1SA | Maidstone | Kent   |
		And User navigates to edit property page with id
	Then Latest 4 properties should contain following data
		| LatestData                                  |
		| Cooper House, 98 High St                    |
		| Copier King, 203 Sherwood Rd                |
		| Tulketh Community Sports College, 10 Tag Ln |
		| Condo, 70 Longford St                       |
	When User clicks latest property on 2 position in drawer menu
	Then View property page should be displayed
		And Property should be displayed with address details 
			| PropertyNumber | PropertyName | Line2       | Postcode | City            | County |
			| 203            | Copier King  | Sherwood Rd | TN2 3LF  | Tunbridge Wells | Kent   |
		And Latest 4 properties should contain following data
			| LatestData                                  |
			| Copier King, 203 Sherwood Rd                |
			| Cooper House, 98 High St                    |
			| Tulketh Community Sports College, 10 Tag Ln |
			| Condo, 70 Longford St                       |
