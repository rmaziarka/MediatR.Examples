Feature: Add, update and view property

@Property
@Attributes
Scenario Outline: Create property
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | <divisionCode>   |
		And Address for add/update property is defined with max length fields
        And User gets <propertyType> for PropertyType
		And User sets attributes for property in Api
			| MinBedrooms   | MaxBedrooms   | MinReceptions   | MaxReceptions   | MinBathrooms   | MaxBathrooms   | MinArea   | MaxArea   | MinLandArea   | MaxLandArea   | MinCarParkingSpaces   | MaxCarParkingSpaces   | MinGuestRooms   | MaxGuestRooms   | MinFunctionRooms   | MaxFunctionRooms   |
			| <MinBedrooms> | <MaxBedrooms> | <MinReceptions> | <MaxReceptions> | <MinBathrooms> | <MaxBathrooms> | <MinArea> | <MaxArea> | <MinLandArea> | <MaxLandArea> | <MinCarParkingSpaces> | <MaxCarParkingSpaces> | <MinGuestRooms> | <MaxGuestRooms> | <MinFunctionRooms> | <MaxFunctionRooms> |
	When User creates property with defined address and <divisionCode> division by Api
	Then User should get OK http status code
		And The created Property is saved in database

	Examples:
	| propertyType      | divisionCode | MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinGuestRooms | MaxGuestRooms | MinFunctionRooms | MaxFunctionRooms |
	| House             | Residential  | 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |               |               |                  |                  |
	| Hotel             | Commercial   |             |             |               |               |              |              | 10000   | 50000   | 9000        | 70000       | 40                  | 60                  | 20            | 50            | 15               | 20               |
	# No attributes - no configuration
	| Farm/Estate       | Residential  |             |             |               |               |              |              |         |         |             |             |                     |                     |               |               |                  |                  |
	| Department Stores | Commercial   |             |             |               |               |              |              |         |         |             |             |                     |                     |               |               |                  |                  |

@Property
@Attributes
Scenario Outline: Create property with invalid data
	Given User gets <country> address form for <itemType> and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | <divisionCode>   |
		And User sets attributes for property in Api
			| MinBedrooms   | MinArea   | MinGuestRooms   |
			| <MinBedrooms> | <MinArea> | <MinGuestRooms> |
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2           | Line3 | Postcode   | City | County |
			| updated abc  | 2              | 55 Baker Street |       | <postCode> |      |        |
        And User gets <propertyType> for PropertyType
	When User creates property with defined address and <divisionCode> division by Api
	Then User should get <statusCode> http status code

	Examples: 
	| country | itemType | postCode    | propertyType | statusCode | divisionCode | MinBedrooms | MinArea | MinGuestRooms |
	| GB      | invalid  | 777         | House        | BadRequest | Residential  | 1           | 1000    |               |
	| invalid | invalid  | 777         | Hotel        | BadRequest | Commercial   |             | 10000   | 20            |
	| invalid | Property | 777         | Hotel        | BadRequest | Commercial   |             | 10000   | 20            |
	| GB      | Property |             | Hotel        | BadRequest | Commercial   |             | 10000   | 30            |
	| GB      | Property | 12345678901 | Bungalow     | BadRequest | Residential  | 1           | 1000    |               |
	| GB      | Property | 777         | invalid      | BadRequest | Commercial   | 1           | 1000    |               |
	| GB      | Property | 777         | House        | BadRequest | Commercial   | 1           | 1000    |               |
	# Invalid attributes only
	| GB      | Property | 777         | House        | BadRequest | Residential  | 1           | 1000    | 1             |
	| GB      | Property | 777         | Hotel        | BadRequest | Commercial   | 1           | 20000   | 1             |

@Property
Scenario Outline: Update property
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
			| Division     | Commercial       |
        And User gets <propertyType1> for PropertyType
		And User sets attributes for property in database
			| MinBedrooms   | MaxReceptions   | MaxArea   | MinGuestRooms   | MaxFunctionRooms   |
			| <MinBedrooms> | <MaxReceptions> | <MaxArea> | <MinGuestRooms> | <MaxFunctionRooms> |
		And Property with Address and <divisionCode1> division is in database
			| PropertyName | PropertyNumber | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Address for add/update property is defined with max length fields
		And User gets <propertyType2> for PropertyType
		And User sets attributes for property in Api
			| MinBedrooms    | MaxReceptions    | MaxArea    | MinGuestRooms    | MaxFunctionRooms    |
			| <MinBedrooms2> | <MaxReceptions2> | <MaxArea2> | <MinGuestRooms2> | <MaxFunctionRooms2> |
	When Users updates property with defined address for latest id and <divisionCode2> division by Api
	Then User should get OK http status code
		And The updated Property is saved in database

	Examples:
	| propertyType1           | propertyType2  | divisionCode1 | divisionCode2 | MinBedrooms | MaxReceptions | MaxArea | MinGuestRooms | MaxFunctionRooms | MinBedrooms2 | MaxReceptions2 | MaxArea2 | MinGuestRooms2 | MaxFunctionRooms2 |
	| Hotel                   | Hotel          | Commercial    | Commercial    |             |               | 10000   | 10            | 10               |              |                | 20000    | 20             | 20                |
	| House                   | House          | Residential   | Residential   | 2           | 3             | 900     |               |                  | 3            | 4              | 1000     |                |                   |
	| Hotel                   | Flat           | Commercial    | Residential   |             |               | 10000   | 20            | 30               | 2            | 3              | 1000     |                |                   |
	| Houseboat               | Hotel          | Residential   | Commercial    | 2           | 3             | 500     |               |                  |              |                | 10000    | 20             | 30                |
	# No attributes - no configuration
	| Retail                  | Car Showroom   | Commercial    | Commercial    |             |               |         |               |                  |              |                |          |                |                   |
	| Retail Unit A1          | Retail Unit A3 | Commercial    | Commercial    |             |               |         |               |                  |              |                |          |                |                   |
	| Industrial/Distribution | Industrial     | Commercial    | Commercial    |             |               |         |               |                  |              |                |          |                |                   |
	| Office                  | Other          | Commercial    | Commercial    |             |               |         |               |                  |              |                |          |                |                   |

@Property
Scenario Outline: Update property with invalid data
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
			| Division     | Commercial       |
        And User gets House for PropertyType
		And User sets attributes for property in database
			| MinBedrooms   | MaxReceptions   | MaxArea   | MinGuestRooms   | MaxFunctionRooms   |
			| <MinBedrooms> | <MaxReceptions> | <MaxArea> | <MinGuestRooms> | <MaxFunctionRooms> |
		And Property with Address and <divisionCode1> division is in database
			| PropertyName | PropertyNumber | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And User gets <country> address form for <itemType> and country details
		And User gets <propertyType> for PropertyType
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode   | City | County |
			|              |                |       |       | <postCode> |      |        |
		And User sets attributes for property in Api
			| MinBedrooms    | MaxReceptions    | MaxArea    | MinGuestRooms    | MaxFunctionRooms    |
			| <MinBedrooms2> | <MaxReceptions2> | <MaxArea2> | <MinGuestRooms2> | <MaxFunctionRooms2> |
	When Users updates property with defined address for <id> id and <divisionCode2> division by Api
	Then User should get <statusCode> http status code

	Examples: 
	| id                                   | country | itemType | postCode    | propertyType | statusCode | divisionCode1 | divisionCode2 | MinBedrooms | MaxReceptions | MaxArea | MinGuestRooms | MaxFunctionRooms | MinBedrooms2 | MaxReceptions2 | MaxArea2 | MinGuestRooms2 | MaxFunctionRooms2 |
	| latest                               | GB      | invalid  | 777         | House        | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	| latest                               | invalid | invalid  | 777         | House        | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	| latest                               | invalid | Property | 777         | House        | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	| latest                               | GB      | Property |             | House        | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	| latest                               | GB      | Property | 12345678901 | House        | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	| 00000000-0000-0000-0000-000000000000 | GB      | Property | 123456      | House        | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	# Invalid type
	| latest                               | GB      | Property | 123456      | invalid      | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	| latest                               | GB      | Property | 123456      | House        | BadRequest | Residential   | Commercial    |             |               |         |               |                  |              |                |          |                |                   |
	| latest                               | GB      | Property | 123456      | Office       | BadRequest | Residential   | Residential   |             |               |         |               |                  |              |                |          |                |                   |
	#invalid attributes
	| latest                               | GB      | Property | 123456      | House        | BadRequest | Residential   | Residential   | 2           | 3             | 1000    |               |                  | 2            | 3              | 1000     | 1              | 2                 |
	| latest                               | GB      | Property | 123456      | Hotel        | BadRequest | Residential   | Commercial    | 2           | 3             | 1000    |               |                  | 2            | 3              | 1000     | 1              | 2                 |

@Property
Scenario: Get non existing property
	Given Property does not exist in DB
	When User retrieves property details
	Then User should get NotFound http status code

@Property
@Ownership
@Attributes
Scenario: Get property
	Given User gets GB address form for Property and country details
        And User gets House for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| OwnershipType  | Freeholder       |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
        And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |  
        And User creates contacts in database with following data
		    | FirstName | Surname | Title |
		    | Michael   | Angel   | cheef | 
		And Ownership exists in database
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  | 1200000   |
			| 01-05-2014   | 01-04-2015 | 1000000  | 1200000   |
		And Activity for 'latest' property exists in database
	When User retrieves property details
	Then User should get OK http status code
		And The created Property is saved in database
        And Ownership list should be the same as in database
		And Activities list should be the same as in database
