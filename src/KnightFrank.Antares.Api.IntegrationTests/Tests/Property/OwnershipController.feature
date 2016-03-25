Feature: Property ownership

@Property
Scenario: Save ownership
    Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId for OwnershipType EnumType code and Freeholder EnumTypeItem code
        And Property with Address is in data base
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
        And User creates contacts in database with following data
		    | FirstName | Surname | Title |
		    | Michael   | Angel   | cheef |
	When User creates an ownership for existing property
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2010   | 01-04-2012 | 1000000  | 1200000   |
	Then User should get OK http status code

@Property
Scenario: Save ownership where dates are overlapping
    Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId for OwnershipType EnumType code and Freeholder EnumTypeItem code
        And Property with Address is in data base
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

@Property
Scenario: Get property with ownership list
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId for OwnershipType EnumType code and Freeholder EnumTypeItem code
        And Property with Address is in data base
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |  
        And User creates contacts in database with following data
		    | FirstName | Surname | Title |
		    | Michael   | Angel   | cheef | 
		And Ownership exists in database
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  | 1200000   |
			| 01-05-2014   | 01-04-2015 | 1000000  | 1200000   |
	When User retrieves property details
	Then User should get OK http status code
        And Ownership list should be the same as in DB
