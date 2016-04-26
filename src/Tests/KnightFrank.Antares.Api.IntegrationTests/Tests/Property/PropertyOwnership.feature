Feature: Property ownership

@Property
@Ownership
Scenario Outline: Save ownership with valid dates
    Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode  | enumTypeItemCode |
			| OwnershipType | Freeholder       |
			| Division      | Residential      |
        And User gets House for PropertyType
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000.1  | 3000.2  | 500.1       | 4000.1      | 1                   | 3                   |
        And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
        And User creates contacts in database with following data
		    | FirstName | Surname | Title |
		    | Michael   | Angel   | cheef |
	When User creates an ownership for existing property
		| PurchaseDate   | SellDate   | BuyPrice   | SellPrice   |
		| <purchaseDate> | <sellDate> | <buyPrice> | <sellPrice> |
	Then User should get OK http status code
        And Created Ownership is saved in database
        And Response contains property with ownership

	Examples: 
	| purchaseDate | sellDate   | buyPrice | sellPrice |
	| 01-05-2010   | 01-04-2012 | 1000000  | 1200000   |
	| 01-05-0010   | 01-04-1010 | 1000000  | 1200000   |

@Property
@Ownership
Scenario: Save ownership where dates are overlapping
    Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode  | enumTypeItemCode |
			| OwnershipType | Freeholder       |
			| Division      | Residential      |
        And User gets House for PropertyType
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000.1  | 3000.2  | 500.1       | 4000.1      | 1                   | 3                   |
        And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
        And User creates contacts in database with following data
		    | FirstName | Surname | Title |
		    | Michael   | Angel   | cheef |
	    And Ownership exists in database
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2010   | 01-04-2012 | 1000000  | 1200000   |
    When User creates an ownership for existing property
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  | 1200000   |
	Then User should get BadRequest http status code
