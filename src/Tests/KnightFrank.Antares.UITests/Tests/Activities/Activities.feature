Feature: Activities UI tests

@Activity
Scenario: Create freehold sale activity for residential property
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Lady  | Sarah     | Chatto  |
		And Property with Residential division and Bungalow type is defined
		And Property attributes details are defined
			| MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 1000    | 2000    | 1500        | 3000        | 1                   | 2                   | 1           | 2           | 1             | 2             | 1            | 2            |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2       | Postcode | City        | County  |
			| 4              | Hotel Park   | Waterloo St | PE30 1NZ | King's Lynn | Norfolk |
		And Property ownership is defined
			| PurchaseDate | BuyPrice  |
			| 01-01-2005   | 100000000 |
	When User navigates to view property page with id
		And User clicks add activites button on view property page
		And User selects activity status and type on create actvity page
			| Type          | Status           |
			| Freehold Sale | Market appraisal |	
	Then Activity details are set on create activity page
	    | Vendor       | Negotiator | ActivityTitle             | Department |
	    | Sarah Chatto | John Smith | Hotel Park, 4 Waterloo St | Aldgate    |
		And Property details are set on create activity page
			| PropertyNumber | PropertyName | Line2       | Postcode | City        | County  |
			| 4              | Hotel Park   | Waterloo St | PE30 1NZ | King's Lynn | Norfolk |
		And Attendees are set on create activity page
			| Attendees    |
			| John Smith   |
			| Sarah Chatto |
	When User selects Divorce from selling reason list on create activity page
		And User fills in basic information on create activity page
			| DisposalType | Source            | SourceDescription | PitchingThreats |
			| Auction      | Direct phone call | Text              | Text            |
		And User fills in additional information on create activity page
			| KeyNumber | AccessArrangements |
			| 123456    | Text               |
		And User fills in appraisal meeting information on create activity page
			| StartTime | Endtime |
			| 10:00     | 12:00   |
		And User fills in attendees on create activity page
			| Attendees    | InvitationText |
			| Sarah Chatto | Text           |
		And User fills in valuation information for activity with freehold sale type on create activity page
			| KfValuation | VendorValuation | AgreedInitialMarketingPrice |
			| 1000        | 2000            | 3000                        |
		And User fills in other information on create activity page
			| Decoration | OtherConditions |
			| Good       | Text            |
		And User clicks save button on create activity page
	Then Activity details should be displayed on view activity page
		| ActivityTitle             | Status           | Type          |
		| Hotel Park, 4 Waterloo St | Market appraisal | Freehold Sale |
		And Property details should be displayed in overview tab on view activity page
			| PropertyNumber | PropertyName | Line2       | Postcode | City        | County  |
			| 4              | Hotel Park   | Waterloo St | PE30 1NZ | King's Lynn | Norfolk |
		And Activity details should be displayed in overview tab on view activity page
			| Vendor       | Negotiator | Attendees    |
			| Sarah Chatto | John Smith | Sarah Chatto |
		And Appraisal meeting date is set to tomorrow date with start time 10:00 - 12:00 in overview tab on view activity page
	When User switches to details tab on view activity page
	Then Activity details should be displayed in details tab on view activity page
		| Vendor       | Negotiator | Department | Source            | SourceDescription | SellingReason | PitchingThreats | KeyNumber | AccessArrangements | Decoration | OtherConditions |
		| Sarah Chatto | John Smith | Aldgate    | Direct phone call | Text              | Divorce       | Text            | 123456    | Text               | Good       | Text            |
		And Details specific for freehold sale activity type are displayed in details tab on view activity page
			| DisposalType | KfValuation | VendorValuation | AgreedInitialMarketingPrice |
			| Auction      | 1,000 GBP   | 2,000 GBP       | 3,000 GBP                   |
		And Property details should be displayed in details tab on view activity page
			| PropertyNumber | PropertyName | Line2       | Postcode | City        | County  |
			| 4              | Hotel Park   | Waterloo St | PE30 1NZ | King's Lynn | Norfolk |

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
			| ActivityStatus   | AskingPrice |
			| Market appraisal | 4000        |
		And User fills in KF Valuation 500 on edit activity page
		And User selects Private Treaty disposal type on edit activity page
		And User changes lead negotiator to Adam Williams on edit activity page
        And User adds secondary negotiators on edit activity page
            | Name            |
            | Eva Sandler     |
            | John Doe        |
            | Martha Williams |
            | Edward Griffin  |          
		And User removes 3 secondary negotiator on edit activity page
		And User clicks save button on edit activity page
	Then View activity page should be displayed
		#And Activity details on view activty page are following
		#	| ActivityStatus   | AskingPrice |
		#	| Market appraisal | 4000        |
		And Adam Williams is set as lead negotiator on view activity page
	When User switches to details tab on view activity page
    Then Secondary negotiators are set on view activity page
        | Name            | NextCall |
        | Edward Griffin  | -        |
        | Eva Sandler     | -        |
        | Martha Williams | -        |
	When User clicks edit button on view activity page
		And User sets 3 secondary negotiator as lead negotiator on edit activity page
		And User clicks save button on edit activity page
	Then View activity page should be displayed
		And Martha Williams is set as lead negotiator on view activity page
	When User switches to details tab on view activity page
    Then Secondary negotiators are set on view activity page
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
	When User switches to details tab on view activity page
	Then Secondary negotiators are set on view activity page
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
	When User switches to details tab on view activity page
    Then Secondary negotiators are set on view activity page
            | Name            | NextCall |
            | Eva Sandler     | 20       |
            | John Doe        | -        |
            | Martha Williams | 30       |
	When User clicks edit button on view activity page
		And User sets 3 secondary negotiator as lead negotiator on edit activity page
		And User clicks save button on edit activity page
	Then View activity page should be displayed
		And Lead negotiator next call is set to 30 days from current day on view activity page
	When User switches to details tab on view activity page
    Then Secondary negotiators are set on view activity page
            | Name          | NextCall |
            | Adam Williams | 0        |
            | Eva Sandler   | 20       |
            | John Doe      | -        |
	When User switchs to overview tab on view activity page
	Then Lead negotiator next call is set to 30 days from current day on view activity page

@Activity
Scenario: Edit negotiators departments 
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Mr    | Michael   | Jordan  |
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
	Then View activity page should be displayed
	When User switches to details tab on view activity page
	Then Departments should be displayed on view activity page
			| Name        |
			| Aldgate     |
			| Residential |
	When User clicks edit button on view activity page
		And User removes 1 secondary negotiator on edit activity page
		And User removes Residential department on edit activity page
		And User adds secondary negotiators on edit activity page
            | Name           |
            | Jeam Beam      |
            | Helen Williams |
            | Thomas Miller  | 
		And User clicks save button on edit activity page
		And User switches to details tab on view activity page 
	Then Departments should be displayed on view activity page
		| Name    |
		| Aldgate |
		| Bath    |
		| Bristol |
		| Chelsea |
	When User clicks edit button on view activity page
		And User sets Bath department as managing department on edit activity page
		And User clicks save button on edit activity page
		And User switches to details tab on view activity page 
	Then Departments should be displayed on view activity page
		| Name    |
		| Bath    |
		| Aldgate |
		| Bristol |
		| Chelsea |
