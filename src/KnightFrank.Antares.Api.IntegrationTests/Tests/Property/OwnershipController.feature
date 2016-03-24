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
	When User creates an ownership for existing property
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2010   | 01-04-2012 | 1000000  | 1200000   |
        And User creates an ownership for existing property
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  | 1200000   |
	Then User should get BadRequest http status code

@ignore
@Property
Scenario: Get property ownership list for existing property and ownership with no entry
	Given Property exist in DB
		But property has not any ownership
	When User send GET request to view property details
	Then ownership from DB should be visible as empty list on property details page

@ignore
@Property
Scenario: Get property ownership list for existing property and ownership with one entry
	Given Property exist in DB
		And property has already ownership
			| Id | Type     | Purchasing date | Selling date | Name     |
			| 1  | Freehold | 01-05-2010      | 01-04-2012   | Jon Snow |
	When User send GET request to view property details
	Then ownership from DB should be visible as list on property details page

@ignore
@Property
Scenario: Get property ownership list for existing property and ownership with more than 2 entry
	Given Property exist in DB
		And property has already ownership
			| Id | Type     | Purchasing date | Selling date | Name          |
			| 1  | Freehold | 01-05-2010      | 01-04-2012   | Jon Snow      |
			| 2  | Freehold | 01-07-1990      | 01-12-1998   | Bruce Willice |
			| 3  | Freehold | 04-04-2001      | 24-04-2003   | Frodo Baggins |
			| 4  | Freehold | 13-11-2004      | 29-02-2008   | Bruce Wayne   |
	When User send GET request to view property details
	Then ownership from DB should be visible as list on property details page

@ignore
@Property
Scenario: Get property ownership list for non existing property
	Given Property with id=10 not exist in DB
	When User send GET request for ownership list for property with id=10
	Then HTTP 404 Not found should be received by user

@ignore
@Property	
Scenario: Get property ownership details for non existing ownership
	Given contect with id=10 not exist in DB
	When Users send GET request for ownership with id=10
	Then HTTP 404 Not found should be received by user
