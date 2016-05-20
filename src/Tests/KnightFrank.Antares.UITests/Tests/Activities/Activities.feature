Feature: Activities UI tests

@Activity
Scenario: Create activity
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Lady  | Sarah     | Chatto  |
		And Property with Commercial division and Leisure.Hotel type is defined
		And Property attributes details are defined
			| MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinGuestRooms | MaxGuestRooms | MinFunctionRooms | MaxFunctionRooms |
			| 20000   | 30000   | 25000       | 40000       | 30                  | 40                  | 60            | 180           | 15               | 25               |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2       | Postcode | City        | County  |
			| 4              | Hotel Park   | Waterloo St | PE30 1NZ | King's Lynn | Norfolk |
		And Property ownership is defined
			| PurchaseDate | BuyPrice  |
			| 01-01-2005   | 100000000 |
	When User navigates to view property page with id
		And User clicks add activites button on view property page	
	Then Activity details are set on create activity page
	    | Vendor       | Status        |
	    | Sarah Chatto | Pre-appraisal |
	When User selects Freehold Sale activity type on create activity page
		And User selects Market appraisal activity status on create activity page
		And User clicks save button on create activity page
	Then Activity details are set on view property page
		| Vendor       | Status           | Type          |
		| Sarah Chatto | Market appraisal | Freehold Sale |

@Activity
Scenario: Edit activity
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Lady  | Amanda    | Harlech |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 2000    | 3000    | 2500        | 40000       | 1                   | 3                   | 2           | 4           | 1             | 2             | 2            | 2            |
		And Property in GB is created in database
			| PropertyNumber | PropertyName      | Line2       | Postcode | City    | County  |
			| 12             | St Crispins House | Duke Street | NR3 1PD  | Norwich | Norfolk |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2012   | 10000000 |
		And Property Long Leasehold Sale activity is defined
	When User navigates to view activity page with id
		And User clicks edit button on view activity page
		And User edits activity details on edit activity page
			| ActivityStatus   | MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
			| Market appraisal | 4000                 | 5000             | 6000                 |
	Then View activity page is displayed
		And Activity details on view activty page are following
			| ActivityStatus   | MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
			| Market appraisal | 4000                 | 5000             | 6000                 |
