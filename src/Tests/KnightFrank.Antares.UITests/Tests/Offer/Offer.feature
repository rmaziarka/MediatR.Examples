Feature: Offer UI tests

@Offer
Scenario: Create offer
	Given Contacts are created in database
		| Title | FirstName | Surname  |
		| Sir   | John      | Soane    |
		| Sir   | Robert    | McAlpine |
		| Sir   | Edward    | Graham   |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 10          | 12          | 2             | 4             | 8            | 10           | 20000   | 25000   | 30000       | 50000       | 10                  | 20                  |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName       | Line2                | Postcode | City   | County        |
			| 13             | John Soane’s house | Lincoln’s Inn Fields | WC2A 3BP | London | London county |
		And Property ownership is defined
			| PurchaseDate | BuyPrice  |
			| 10-01-1998   | 100000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice  | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 90000000 | 120000000 | 5           | 15          | 2                 | 4                 | 2            | 10           | 15               | 25               | 20000   | 25000   | 30000       | 50000       | Note        |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
	Then Activity details on view requirement page are same as the following
		| Activity                                    |
		| John Soane’s house, 13 Lincoln’s Inn Fields |
	When User fills in offer details on view requirement page
		| Status | Offer  | SpecialConditions |
		| New    | 100000 | Text              |
		And User clicks save offer button on view requirement page
