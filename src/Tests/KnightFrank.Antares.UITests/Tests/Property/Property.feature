﻿Feature: Property UI tests

@Property
Scenario: Update property
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Sir   | Alex      | Johnson |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 4           | 1             | 3             | 2            | 3            | 2000.12 | 4000.12 | 6000.13     | 10000.1     | 3                   | 5                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2        | Postcode | City   | County           |
			| 55             | Knight Frank | Baker Street | W1U 8AN  | London | County of London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2014   | 1000000  |
		And Property Freehold Sale activity is defined
	When User navigates to view property page with id
		And User clicks edit property button on view property page
		And User selects Commercial property and Hotel type on edit property page
		And User fills in property details on edit property page
			| MinGuestRooms | MaxGuestRooms | MinFunctionRooms | MaxFunctionRooms | MinArea | MaxArea |
			| 120           | 200           | 10               | 20               | 4000.5  | 5500.0  |
		And User fills in address details on edit property page
			| PropertyNumber | PropertyName | Line2 | Line3          | Postcode | City | County |
			|                |              |       | Address line 3 | W2U 8AN  |      |        |
		And User selects property characteristics on edit property page
			| Name    | Comment |
			| Airport | Airport |
		And User selects property characteristics on edit property page
			| Name |
			| Spa  |
		And User clicks save property button on edit property page
	Then Property should be updated with address details 
		| PropertyNumber | PropertyName | Line2 | Line3          | Postcode | City | County |
		|                |              |       | Address line 3 | W2U 8AN  |      |        |
		And Property should be updated with Hotel property type and following attributes
			| PropertyArea           | LandArea                   | CarParkingSpaces | GuestRooms | FunctionRooms |
			| 4,000.5 - 5,500 sq. ft | 6,000.13 - 10,000.1 sq. ft | 3 - 5            | 120 - 200  | 10 - 20       |
		And Characteristics are displayed on view property page
			| Name        | Comment |
			| Airport     | Airport |
			| Coastal     | Comment |
			| Island      | Comment |
			| Town/City   | Comment |
			| Village     | Comment |
			| Golf Course | Comment |
			| Spa         |         |
		And Ownership details should contain following data on view property page
			| Position | ContactName | ContactSurname | Type     | PurchaseDate |
			| 1        | Alex        | Johnson        | Freehold | 05-01-2014   |
		And Activity details are set on view property page
			| Vendor       | Status        | Type          |
			| Alex Johnson | Pre-appraisal | Freehold Sale |

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
			| 2           | 4           | 1             | 3             | 2            | 3            | 2000.12 | 4000.12 | 6000.13     | 10000.1     | 3                   | 5                   |
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
	Then New property should be created with address details 
		| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
		| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
		And New property should be created with Flat property type and following attributes
			| Bedrooms | Receptions | Bathrooms | PropertyArea               | LandArea                   | CarParkingSpaces |
			| 2 - 4    | 1 - 3      | 2 - 3     | 2,000.12 - 4,000.12 sq. ft | 6,000.13 - 10,000.1 sq. ft | 3 - 5            |
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
		And User fills in ownership details on view property page
			| Type     | Current | PurchasePrice | PurchaseDate |
			| Freehold | true    | 1000000       | 15-01-2014   |
	Then Ownership details should contain following data on view property page
		| Position | ContactName | ContactSurname | Type     | PurchaseDate |
		| 1        | Eva         | Queen          | Freehold | 15-01-2014   |
	When User clicks add activites button on view property page	
		And User selects Open Market Letting activity type on activity panel
		And User clicks save button on activity panel
	Then Activity details are set on view property page
		| Vendor    | Status        | Type                |
		| Eva Queen | Pre-appraisal | Open Market Letting |
	When User clicks activity's details link on view property page
		And User clicks view activity link on activity preview page
	Then Address details on view activity page are following
		| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
		| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
	When User clicks add attachment button on view activity page
		And User adds PDF document.pdf file with Brochure type on attach file panel
		And User clicks edit button on view activity page
		And User edits activity details on edit activity page
			| ActivityStatus   | MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
			| Market appraisal | 3000                 | 4000             | 5000                 |
	Then View activity page is displayed
	When User clicks property details link on view activity page
		And User clicks view property link on property preview page
	Then View property page is displayed

@Property
Scenario: Create commercial property
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Dr    | Alexander | London  |
		And User navigates to create property page
	When User selects United Kingdom country on create property page
		And User selects Commercial property and Hotel type on create property page
		And User fills in address details on create property page
			| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
			| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
		And User clicks save property button on create property page
	Then New property should be created with address details 
		| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
		| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
	When User clicks add area breakdown button on view property page
		And User fills in area details on create area panel
			| Name               | Size |
			| First floor        | 100  |
			| Second floor       | 150  |
			| Third floor area A | 70   |
			| Third floor area B | 30   |
			| Third floor area C | 50   |
		And User clicks save button on create area panel
	Then Area breakdown order is following on view property page
		| Name               | Size |
		| First floor        | 100  |
		| Second floor       | 150  |
		| Third floor area A | 70   |
		| Third floor area B | 30   |
		| Third floor area C | 50   |
