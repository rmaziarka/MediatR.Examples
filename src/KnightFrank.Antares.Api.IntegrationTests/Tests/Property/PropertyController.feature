Feature: Add, update and view property

@Property
Scenario Outline: Create property
	Given User gets GB address form for Property and country details
		And Address for add/update property is defined with max length fields
        And User gets <propertyType> for PropertyType
	When User creates property with defined address by Api
	Then User should get OK http status code
		And The created Property is saved in data base

	Examples:
	| propertyType      |
	| House             |
	| Farm/Estate       |
	| Office            |
	| Department Stores |
	| Retail            |

@Property
Scenario Outline: Create property with invalid data
	Given User gets <country> address form for <itemType> and country details
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2           | Line3 | Postcode   | City | County |
			| updated abc  | 2              | 55 Baker Street |       | <postCode> |      |        |
        And User gets <propertyType> for PropertyType
	When User creates property with defined address by Api
	Then User should get <statusCode> http status code

	Examples: 
	| country | itemType | postCode    | propertyType | statusCode |
	| GB      | bla      | 777         | House        | BadRequest |
	| bla     | bla      | 777         | Car Park     | BadRequest |
	| bla     | Property | 777         | Other        | BadRequest |
	| GB      | Property |             | Office       | BadRequest |
	| GB      | Property | 12345678901 | Bungalow     | BadRequest |
	| GB      | Property | 777         | invalid      | BadRequest |

@Property
@ignore
Scenario Outline: Update property
	Given User gets GB address form for Property and country details
        And User gets <propertyType1> for PropertyType
		And Property with Address is in data base
			| PropertyName | PropertyNumber | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Address for add/update property is defined with max length fields
		And User gets <propertyType2> for PropertyType
	When Users updates property with defined address for latest id by Api
	Then User should get OK http status code
		And The updated Property is saved in data base

	Examples:
	| propertyType1           | propertyType2  |
	| Farm/Estate             | Farm/Estate    |
	| Office                  | Office         |
	| Retail                  | Car Showroom   |
	| Retail Unit A1          | Retail Unit A3 |
	| Industrial/Distribution | Industrial     |
	| Office                  | Other          |

@Property
Scenario Outline: Update property with invalid data
	Given User gets GB address form for Property and country details
        And User gets House for PropertyType
		And Property with Address is in data base
			| PropertyName | PropertyNumber | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And User gets <country> address form for <itemType> and country details
		And User gets <propertyType> for PropertyType
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode   | City | County |
			|              |                |       |       | <postCode> |      |        |
	When Users updates property with defined address for <id> id by Api
	Then User should get <statusCode> http status code

	Examples: 
	| id                                   | country | itemType | postCode    | propertyType | statusCode |
	| latest                               | GB      | bla      | 777         | House        | BadRequest |
	| latest                               | bla     | bla      | 777         | House        | BadRequest |
	| latest                               | bla     | Property | 777         | House        | BadRequest |
	| latest                               | GB      | Property |             | House        | BadRequest |
	| latest                               | GB      | Property | 12345678901 | House        | BadRequest |
	| 00000000-0000-0000-0000-000000000000 | GB      | Property | 123456      | House        | BadRequest |
	| latest                               | GB      | Property | 123456      | invalid      | BadRequest |

@Property
Scenario: Get non existing property
	Given Property does not exist in DB
	When User retrieves property details
	Then User should get NotFound http status code

@Property
@Ownership
Scenario: Get property
	Given User gets GB address form for Property and country details
        And User gets House for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| OwnershipType  | Freeholder       |
			| ActivityStatus | PreAppraisal     |
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
		And Activity for 'latest' property exists in data base 
	When User retrieves property details
	Then User should get OK http status code
		And The created Property is saved in data base
        And Ownership list should be the same as in DB
		And Activities list should be the same as in DB
