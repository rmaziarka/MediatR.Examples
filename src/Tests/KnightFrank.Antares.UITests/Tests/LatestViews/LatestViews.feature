Feature: Latest views UI tests

@LatestViews
Scenario: Display latest viewed properties
	Given Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2       | Postcode | City          | County          |
			| 70             | Condo        | Longford St | TS1 4RN  | Middlesbrough | North Yorkshire |
		And Property Freehold Sale activity is defined
	When User navigates to view activity page with id
		And User clicks property details on view activity page
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

@LatestViews
Scenario: Display latest viewed activities
	Given Property with Residential division and House type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName              | Line2       | Postcode | City    | County      |
			| 24             | The Alternative Tuck Shop | Holywell St | OX1 3SB  | Oksford | Oxfordshire |
	When User navigates to view property page with id
		And User clicks add activites button on view property page	
		And User selects Freehold Sale activity type on create activity page
		And User clicks save button on create activity page
		And User opens navigation drawer menu
		And User selects Activities menu item
	Then Latest 1 activity should contain following data
		| LatestData                                |
		| The Alternative Tuck Shop, 24 Holywell St |
	When Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2     | Postcode | City    | County      |
			| 2              | St John Flat | Walton St | OX1 2HD  | Oksford | Oxfordshire |
		And Property Long Leasehold Sale activity is defined
	When User navigates to edit activity page with id
	Then Latest 2 activities should contain following data
		| LatestData                                |
		| St John Flat, 2 Walton St                 |
		| The Alternative Tuck Shop, 24 Holywell St |
	When User goes back to previous page
		And User clicks activity details link on view property page
	Then Latest 2 activities should contain following data
		| LatestData                                |
		| The Alternative Tuck Shop, 24 Holywell St |
		| St John Flat, 2 Walton St                 |
	When Contacts are created in database
		| Title | FirstName | LastName  |
		| Lady  | Joanna    | Thornhill |
		And Property with Residential division and House type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2      | Postcode | City     | County |
			| 237            | Duke Flat    | Margate Rd | CT12 6TA | Ramsgate | Kent   |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-01-2015   | 10000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice |
			| 100000   | 500000   |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Status |
			| New    |
	When User navigates to view offer page with id
		And User clicks activity details on view offer page
	Then Latest 3 activities should contain following data
		| LatestData                                |
		| Duke Flat, 237 Margate Rd                 |
		| The Alternative Tuck Shop, 24 Holywell St |
		| St John Flat, 2 Walton St                 |
	When User clicks latest activity on 3 position in drawer menu
	Then Latest 3 activities should contain following data
		| LatestData                                |
		| St John Flat, 2 Walton St                 | 
		| Duke Flat, 237 Margate Rd                 |
		| The Alternative Tuck Shop, 24 Holywell St |
		And Address details on view activity page are following
			| PropertyNumber | PropertyName | Line2     | Postcode | City    | County      |
			| 2              | St John Flat | Walton St | OX1 2HD  | Oksford | Oxfordshire |
	When User navigates to edit offer page with id
		And User clicks activity details on edit offer page
	Then Latest 3 activities should contain following data
		| LatestData                                |
		| Duke Flat, 237 Margate Rd                 |
		| St John Flat, 2 Walton St                 |
		| The Alternative Tuck Shop, 24 Holywell St |

@LatestViews
Scenario: Display latest viewed requirements
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Miss  | Triss     | Merigold |
	When User navigates to create requirement page
		And User opens navigation drawer menu
		And User selects Requirements menu item
		And User selects contacts on create requirement page
			| FirstName | LastName |
			| Triss     | Merigold |
		And User fills in location details on create requirement page
			| Country        | Line2    | Postcode | City   |
			| United Kingdom | Gower St | WC1E 6BT | London |
		And User clicks save requirement button on create requirement page
	Then New requirement should be created
		And Latest 1 requirement should contain following data
			| LatestData             |
			| Triss Merigold         |
	When Contacts are created in database
		| Title | FirstName | LastName |
		| Dr    | Van       | Wilder   |
		| Sir   | Van       | Wilder   |
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note        |
	When User navigates to view requirement page with id
	Then Latest 2 requirements should contain following data
		| LatestData             |
		| Van Wilder, Van Wilder |
		| Triss Merigold         |
	When User clicks latest requirement on 2 position in drawer menu
	Then View requirement page should be displayed
		And Requirement location details on view requirement page are same as the following
			| Line2    | Postcode | City   |
			| Gower St | WC1E 6BT | London |
		And Requirement applicants on view requirement page are same as the following
			| FirstName | LastName |
			| Triss     | Merigold |
		And Latest 2 requirements should contain following data
			| LatestData             |
			| Triss Merigold         |
			| Van Wilder, Van Wilder |
