Feature: Property attributes

@Property
Scenario Outline: Create property with attributes and characteristics
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | <divisionCode>   |
		And Address for add/update property is defined with max length fields
        And User gets <propertyType> for PropertyType
		And User sets attributes for property in Api
			| MinBedrooms   | MaxBedrooms   | MinReceptions   | MaxReceptions   | MinBathrooms   | MaxBathrooms   | MinArea   | MaxArea   | MinLandArea   | MaxLandArea   | MinCarParkingSpaces   | MaxCarParkingSpaces   | MinGuestRooms   | MaxGuestRooms   | MinFunctionRooms   | MaxFunctionRooms   |
			| <MinBedrooms> | <MaxBedrooms> | <MinReceptions> | <MaxReceptions> | <MinBathrooms> | <MaxBathrooms> | <MinArea> | <MaxArea> | <MinLandArea> | <MaxLandArea> | <MinCarParkingSpaces> | <MaxCarParkingSpaces> | <MinGuestRooms> | <MaxGuestRooms> | <MinFunctionRooms> | <MaxFunctionRooms> |
		And Property characteristics are set for given property type
	When User creates property with defined address and <divisionCode> division by Api
	Then User should get OK http status code
		And The created Property is saved in database

	Examples:
	| propertyType  | divisionCode | MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinGuestRooms | MaxGuestRooms | MinFunctionRooms | MaxFunctionRooms |
	| House         | Residential  | 1           | 3           | 1             | 3             | 2            | 3            | 1000.1  | 3000.1  | 500.1       | 4000.1      | 1                   | 3                   |               |               |                  |                  |
	| Leisure.Hotel | Commercial   |             |             |               |               |              |              | 10000.1 | 50000.1 | 9000.1      | 70000.1     | 40                  | 60                  | 20            | 50            | 15               | 20               |

@Property
Scenario Outline: Create property with invalid attributes
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
		And Property characteristics are set for given property type
	When User creates property with defined address and <divisionCode> division by Api
	Then User should get <statusCode> http status code

	Examples:
	| country | itemType | postCode | propertyType  | divisionCode | MinBedrooms | MinArea | MinGuestRooms | statusCode |
	| GB      | Property | 777      | House         | Residential  | 1           | 1000.8  | 1             | BadRequest |
	| GB      | Property | 777      | Leisure.Hotel | Commercial   | 1           | 20000.9 | 1             | BadRequest |

@Property
Scenario Outline: Get property attributes
	Given User gets <propertyType> for PropertyType
		And User retrieves <countryCode> country id
	When User retrieves attributes for given property type id and country id
	Then User should get <statusCode> http status code

	Examples:
	| propertyType  | countryCode | statusCode |
	| House         | GB          | OK         |
	| Leisure.Hotel | GB          | OK         |
	| Leisure.Hotel | invalid     | BadRequest |
	| invalid       | GB          | BadRequest |
	
@Property
Scenario Outline: Update property with attributes and characteristics
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
			| Division     | Commercial       |
        And User gets <propertyType1> for PropertyType
		And Property characteristics are set for given property type
		And Property attributes exists in database
			| MinBedrooms   | MaxReceptions   | MaxArea   | MinGuestRooms   | MaxFunctionRooms   |
			| <MinBedrooms> | <MaxReceptions> | <MaxArea> | <MinGuestRooms> | <MaxFunctionRooms> |
		And Property exists in database
			| PropertyType    | Division        |
			| <propertyType1> | <divisionCode1> |
		And Address for add/update property is defined with max length fields
		And User gets <propertyType2> for PropertyType
		And User sets attributes for property in Api
			| MinBedrooms    | MaxReceptions    | MaxArea    | MinGuestRooms    | MaxFunctionRooms    |
			| <MinBedrooms2> | <MaxReceptions2> | <MaxArea2> | <MinGuestRooms2> | <MaxFunctionRooms2> |
		And Property characteristics are set for given property type
	When User updates property with defined address for latest id and <divisionCode2> division by Api
	Then User should get OK http status code
		And The updated Property is saved in database

	Examples:
	| propertyType1 | propertyType2 | divisionCode1 | divisionCode2 | MinBedrooms | MaxReceptions | MaxArea | MinGuestRooms | MaxFunctionRooms | MinBedrooms2 | MaxReceptions2 | MaxArea2 | MinGuestRooms2 | MaxFunctionRooms2 |
	| Leisure.Hotel | Leisure.Hotel | Commercial    | Commercial    |             |               | 10000.1 | 10            | 10               |              |                | 20000.2  | 20             | 20                |
	| House         | House         | Residential   | Residential   | 2           | 3             | 900.2   |               |                  | 3            | 4              | 1000.3   |                |                   |
	| Leisure.Hotel | Flat          | Commercial    | Residential   |             |               | 10000.3 | 20            | 30               | 2            | 3              | 1000.4   |                |                   |
	| Houseboat     | Leisure.Hotel | Residential   | Commercial    | 2           | 3             | 500.4   |               |                  |              |                | 10000.5  | 20             | 30                |

@Property
Scenario Outline: Update property with invalid attributes data
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
			| Division     | Commercial       |
        And User gets House for PropertyType
		And Property characteristics are set for given property type
		And Property attributes exists in database
			| MinBedrooms   | MaxReceptions   | MaxArea   | MinGuestRooms   | MaxFunctionRooms   |
			| <MinBedrooms> | <MaxReceptions> | <MaxArea> | <MinGuestRooms> | <MaxFunctionRooms> |
		And Property exists in database
			| PropertyType | Division        |
			| House        | <divisionCode1> |
		And User gets <propertyType> for PropertyType
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode   | City | County |
			|              |                |       |       | <postCode> |      |        |
		And User sets attributes for property in Api
			| MinBedrooms    | MaxReceptions    | MaxArea    | MinGuestRooms    | MaxFunctionRooms    |
			| <MinBedrooms2> | <MaxReceptions2> | <MaxArea2> | <MinGuestRooms2> | <MaxFunctionRooms2> |
		And Property characteristics are set for given property type
	When User updates property with defined address for <id> id and <divisionCode2> division by Api
	Then User should get <statusCode> http status code

	Examples:
	| id     | country | itemType | postCode | propertyType  | divisionCode1 | divisionCode2 | MinBedrooms | MaxReceptions | MaxArea | MinGuestRooms | MaxFunctionRooms | MinBedrooms2 | MaxReceptions2 | MaxArea2 | MinGuestRooms2 | MaxFunctionRooms2 | statusCode |
	| latest | GB      | Property | 123456   | House         | Residential   | Residential   | 2           | 3             | 1000.1  |               |                  | 2            | 3              | 1000.2   | 1              | 2                 | BadRequest |
	| latest | GB      | Property | 123456   | Leisure.Hotel | Residential   | Commercial    | 2           | 3             | 1000.1  |               |                  | 2            | 3              | 1000.2   | 1              | 2                 | BadRequest |
