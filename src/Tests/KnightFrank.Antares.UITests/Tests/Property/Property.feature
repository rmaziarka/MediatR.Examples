Feature: Property UI tests

@Property
Scenario: Update residential property
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Sir   | Alex      | Johnson |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 4           | 1             | 3             | 2            | 3            | 2000 | 4000 | 6000     | 10000     | 3                   | 5                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2        | Postcode | City   | County           |
			| 55             | Knight Frank | Baker Street | W1U 8AN  | London | County of London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2014   | 1000000  |
		And Property Freehold Sale activity is defined
	When User navigates to view property page with id
		And User opens navigation drawer menu
		And User selects Properties menu item
		And User clicks edit property button on view property page
		And User selects Residential property and House type on edit property page
		And User fills in property details on edit property page
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 2           | 1             | 2             | 3            | 4            | 1000    | 3000 | 5000     | 9000      | 2                   | 3                   |
		And User fills in address details on edit property page
			| PropertyNumber | PropertyName | Line2 | Line3          | Postcode | City | County |
			|                |              |       | Address line 3 | W2U 8AN  |      |        |
		And User selects property characteristics on edit property page
			| Name     | Comment  |
			| Detached | Detached |
		And User selects property characteristics on edit property page
			| Name     |
			| Terraced |
		And User clicks save property button on edit property page
	Then Property should be updated with address details 
		| PropertyNumber | PropertyName | Line2 | Line3          | Postcode | City | County |
		|                |              |       | Address line 3 | W2U 8AN  |      |        |
		And Latest 1 property should contain following data
			| LatestData |
			| -          |
		And Property should be updated with House property type and following attributes
			| Bedrooms | Receptions | Bathrooms | LandArea                  | PropertyArea            | CarParkingSpaces |
			| 1 - 2    | 1 - 2      | 3 - 4     | 5,000 - 9,000 sq. ft | 1,000 - 3,000 sq. ft | 2 - 3            |
		And Characteristics are displayed on view property page
			| Name                   | Comment  |
			| Detached               | Detached |
			| Terraced               |          |
			| Conservatory           | Comment  |
			| Garden                 | Comment  |
			| Land                   | Comment  |
			| Patio / Terrace        | Comment  |
			| Island                 | Comment  |
			| Rural                  | Comment  |
			| Town/City              | Comment  |
			| Village                | Comment  |
			| Coastal                | Comment  |
			| Fishing                | Comment  |
			| Equestrian             | Comment  |
			| Golf Course            | Comment  |
			| Leisure Facilities     | Comment  |
			| Listed                 | Comment  |
			| New Development        | Comment  |
			| Secondary accomodation | Comment  |
			| Swimming Pool          | Comment  |
			| Tennis Court           | Comment  |
			| Waterside              | Comment  |
			| Fair                   | Comment  |
			| Good                   | Comment  |
			| Unmodernised           | Comment  |
			| Very Good              | Comment  |
		And Ownership details should contain following data on view property page
			| Position | ContactName | ContactSurname | Type     | PurchaseDate |
			| 1        | Alex        | Johnson        | Freehold | 05-01-2014   |
		And Activity details are set on view property page
			| Vendor       | Status        | Type          |
			| Alex Johnson | Pre-appraisal | Freehold Sale |
	When User clicks activity details link on view property page
		And User selects Activities menu item
	Then Latest 1 activity should contain following data
		| LatestData |
		| -          |

@Property
Scenario: Create residential property
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Dr    | Eva       | Queen   |
		And User navigates to create property page
	When User selects United Kingdom country on create property page
		And User selects Residential property and Flat type on create property page
		And User fills in property details on create property page
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 4           | 1             | 3             | 2            | 3            | 2000 | 4000 | 6000     | 10000     | 3                   | 5                   |
		And User fills in address details on create property page
			| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
			| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
		And User selects property characteristics on create property page
			| Name         | Comment              |
			| Conservatory | Size about 20 sq. ft |
			| Island       | Tropic island        |
		And User selects property characteristics on create property page
			| Name          |
			| Balcony       |
			| Duplex        |
			| Coastal       |
			| Swimming Pool |
		And User clicks save property button on create property page
	Then Property created success message should be displayed
		And New property should be created with address details 
			| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
			| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
		And New property should be created with Flat property type and following attributes
			| Bedrooms | Receptions | Bathrooms | PropertyArea               | LandArea                   | CarParkingSpaces |
			| 2 - 4    | 1 - 3      | 2 - 3     | 2,000 - 4,000 sq. ft | 6,000 - 10,000 sq. ft | 3 - 5            |
		And Characteristics are displayed on view property page
			| Name          | Comment              |
			| Conservatory  | Size about 20 sq. ft |
			| Island        | Tropic island        |
			| Balcony       |                      |
			| Duplex        |                      |
			| Coastal       |                      |
			| Swimming Pool |                      |
	When User selects contacts for ownership on view property page
		| FirstName | Surname |
		| Eva       | Queen   |
		When User fills in ownership details on view property page
			| Type     | Current | PurchasePrice | PurchaseDate |
			| Freehold | true    | 1000000       | 15-01-2014   |
	Then Ownership details should contain following data on view property page
		| Position | ContactName | ContactSurname | Type     | PurchaseDate |
		| 1        | Eva         | Queen          | Freehold | 15-01-2014   |
	When User clicks add activites button on view property page	
		And User selects Open Market Letting type on create activity page
		And User edits activity details on edit activity page
			| ActivityStatus |
			| Pre-appraisal  |                         
		And User selects KF PR from source list on create activity page
		And User selects John Smith from attendees on create activity page
		And User clicks save button on create activity page
	Then View activity page should be displayed
	When User clicks property details on view activity page
		And User clicks view property link from property on view activity page
	Then Activity details are set on view property page
		| Vendor    | Status        | Type                |
		| Eva Queen | Pre-appraisal | Open Market Letting |
	When User clicks activity details link on view property page
		And User clicks view activity link from activity on view property page
	Then Address details on view activity page are following
		| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
		| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
	When User switches to attachments tab on view activity page
		And User clicks add attachment button on attachments tab on view activity page
		And User adds PDF document.pdf file with Brochure type on attachments tab on view activity page
		And User clicks edit button on view activity page
		And User edits activity details on edit activity page
			| ActivityStatus   | ShortLetPricePerWeek | KfValuationPricePerWeek |
			| Market appraisal | 3000                 | 100                     |
		And User clicks save button on edit activity page
	Then View activity page should be displayed
	When User clicks property details on view activity page
		And User clicks view property link from property on view activity page
	Then View property page should be displayed

@Property
Scenario: Create commercial property
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Dr    | Alexander | London  |
		And User navigates to create property page
	When User selects United Kingdom country on create property page
		And User selects Commercial property and Hotel type on create property page
		And User fills in address details on create property page
			| PropertyNumber | PropertyName        | Line2    | Postcode | City      | County    |
			| 104            | Malmaison Newcastle | Quayside | NE1 3DX  | Newcastle | Newcastle |
		And User clicks save property button on create property page
	Then Property created success message should be displayed
		And New property should be created with address details 
			| PropertyNumber | PropertyName        | Line2    | Postcode | City      | County    |
			| 104            | Malmaison Newcastle | Quayside | NE1 3DX  | Newcastle | Newcastle |
	When User clicks add area breakdown button on view property page
		And User fills in area details on view property page
			| Name               | Size |
			| First floor        | 100  |
			| Second floor       | 150  |
			| Third floor area A | 70   |
			| Third floor area B | 30   |
		And User clicks save area button on view property page
	Then Area breakdown order is following on view property page
		| Name               | Size |
		| First floor        | 100  |
		| Second floor       | 150  |
		| Third floor area A | 70   |
		| Third floor area B | 30   |

@Property
Scenario: Update commercial property area breakdown
	Given Property with Commercial division and Leisure.Hotel type is defined
		And Property attributes details are defined
			| MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinGuestRooms | MaxGuestRooms | MinFunctionRooms | MaxFunctionRooms |
			| 20000   | 30000   | 25000       | 40000       | 30                  | 40                  | 60            | 180           | 15               | 25               |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName         | Line2          | Postcode | City     | County |
			| 1              | Boringdon Hall Hotel | Boringdon Hill | PL7 4DP  | Plymouth | Devon  |
		And Property area breakdown is defined
	When User navigates to view property page with id
		And User clicks edit area button for 1 area on view property page
		And User updates area details on view property page
			| Name        | Size  |
			| First floor | 20000 |
		And User clicks save area button on view property page
	Then Area breakdown order is following on view property page
		| Name        | Size  |
		| First floor | 20000 |
		| 2nd floor   | 10000 |
