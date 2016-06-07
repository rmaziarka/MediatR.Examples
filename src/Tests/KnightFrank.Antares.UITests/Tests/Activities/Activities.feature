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
	Then John Smith is set as lead negotiator on view activity page
	When User clicks edit button on view activity page
		And User edits activity details on edit activity page
			| ActivityStatus   | MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
			| Market appraisal | 4000                 | 5000             | 6000                 |
		And User changes lead negotiator to Adam Williams on edit activity page
        And User adds secondary negotiators on edit activity page
            | Name            |
            | Eva Sandler     |
            | John Doe        |
            | Martha Williams |
            | Edward Griffin  |          
		And User removes 3 secondary negotiator from edit activity page
		And User clicks save button on edit activity page
	Then View activity page should be displayed
		And Activity details on view activty page are following
			| ActivityStatus   | MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
			| Market appraisal | 4000                 | 5000             | 6000                 |
		And Adam Williams is set as lead negotiator on view activity page
        And Secondary negotiators are set on view activity page
            | Name            | NextCall |
            | Edward Griffin  | -        |
            | Eva Sandler     | -        |
            | Martha Williams | -        |
	When User clicks edit button on view activity page
		And User sets 3 secondary negotiator as lead negotiator on edit activity page
		And User clicks save button on edit activity page
	Then View activity page should be displayed
		And Martha Williams is set as lead negotiator on view activity page
        And Secondary negotiators are set on view activity page
            | Name           | NextCall |
            | Adam Williams  | 14       |
            | Edward Griffin | -        |
            | Eva Sandler    | -        |

@Activity
Scenario: Edit negotiators next call dates 
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Mr    | Michael   | Johnson |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 450     | 1500    | 1500        | 32000       | 2                   | 3                   | 1           | 2           | 1             | 2             | 7            | 10           |
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2        | Postcode | City   | County |
			| 124            | Duke House   | Baker Street | NR5 2ZX  | London | London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-03-2010   | 25000000 |
		And Property Long Leasehold Sale activity with negotiators is defined
	When User navigates to view activity page with id
	Then Lead negotiator next call is set to 14 days from current day on view activity page
		And Secondary negotiators are set on view activity page
			| Name            | NextCall |
			| Eva Sandler     | -        |
			| John Doe        | -        |
			| Martha Williams | -        |
	When User edits lead negotiator next call to 0 days from current day on view activity page
		And User edits secondary negotiator next call on view activity page
			| Name            | NextCall |
            | Eva Sandler     | 1        |
            | John Doe        | 10       |
		And User clicks edit button on view activity page
	Then Lead negotiator next call is set to current date on edit activity page
		And Secondary negotiators next calls are displayed on edit activity page
			| Name            | NextCall |
			| Eva Sandler     | 1        |
			| John Doe        | 10       |
			| Martha Williams |          |
	When User changes lead negotiator to Adam Williams on edit activity page
        And User edits secondary negotiators dates on edit activity page
            | Name            | NextCall |
            | Eva Sandler     | 20       |
            | John Doe        |          |
            | Martha Williams | 30       |
		And User clicks save button on edit activity page
	Then View activity page should be displayed
		And Lead negotiator next call is set to 0 days from current day on view activity page
        And Secondary negotiators are set on view activity page
            | Name            | NextCall |
            | Eva Sandler     | 20       |
            | John Doe        | -        |
            | Martha Williams | 30       |
	When User clicks edit button on view activity page
		And User sets 3 secondary negotiator as lead negotiator on edit activity page
		And User clicks save button on edit activity page
	Then View activity page should be displayed
		And Lead negotiator next call is set to 30 days from current day on view activity page
        And Secondary negotiators are set on view activity page
            | Name          | NextCall |
            | Adam Williams | 0        |
            | Eva Sandler   | 20       |
            | John Doe      | -        |

@Activity
Scenario: Edit negotiators departments 
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Mr    | Michael   | Jordan |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 250     | 1600    | 1500        | 32000       | 4                   | 5                   | 1           | 3           | 1             | 2             | 7            | 10           |
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2       | Postcode | City   | County |
			| 12             | Big House    | Bath Street | NR5 3ZX  | London | London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-03-2010   | 25000000 |
		And Property Long Leasehold Sale activity with negotiators is defined
	When User navigates to view activity page with id
	Then Departments are displayed on view activity page
		| Department  |
		| Aldgate     |
		| Residential |
	When User clicks edit button on view activity page
		And User removes 1 secondary negotiator from edit activity page
		And User removes Residential department from edit activity page
		And User adds secondary negotiators on edit activity page
            | Name           |
            | Jeam Beam      |
            | Helen Williams |
            | Thomas Miller  | 
		And User clicks save button on edit activity page 
	Then Departments are displayed on view activity page
		| Department |
		| Aldgate    |
		| Bath       |
		| Bristol    |
		| Chelsea    |
	When User clicks edit button on view activity page
		And User set Bath department as managing department on edit activity page
		And User clicks save button on edit activity page
	Then Departments are displayed on view activity page
		| Department |
		| Bath       |
		| Aldgate    |
		| Bristol    |
		| Chelsea    |