Feature: Property ownership

@Property
@Ownership
Scenario Outline: Create ownership
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
        And Contacts exists in database
		    | FirstName | LastName | Title |
		    | Michael   | Angel    | cheef |
	When User creates Freeholder ownership using api
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
Scenario Outline: Create ownership with mandatory fields
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
        And Contacts exists in database
		    | FirstName | LastName | Title |
		    | Michael   | Angel    | cheef |
	When User creates <ownershipType> ownership with mandatory fields using api
	Then User should get OK http status code
        And Created Ownership is saved in database
        And Response contains property with ownership

	Examples: 
	| ownershipType |
	| Freeholder    |
	| Leaseholder   |

@Property
@Ownership
Scenario: Create ownership where dates are overlapping
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
        And Contacts exists in database
		    | FirstName | LastName | Title |
		    | Michael   | Angel    | cheef |
	    And Ownership Freeholder exists in database
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2010   | 01-04-2012 | 1000000  | 1200000   |
    When User creates Freeholder ownership using api
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  | 1200000   |
	Then User should get BadRequest http status code
