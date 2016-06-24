Feature: Property

@Property
Scenario Outline: Create property
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | <divisionCode>   |
		And Address for add/update property is defined with max length fields
        And User gets <propertyType> for PropertyType
		And Property characteristics are set for given property type
	When User creates property with defined address and <divisionCode> division by Api
	Then User should get OK http status code
		And The created Property is saved in database

	Examples:
	| propertyType             | divisionCode |
	| House                    | Residential  |
	| Leisure.Hotel            | Commercial   |
	| Farm/Estate              | Residential  |
	| Retail.Department Stores | Commercial   |

@Property
Scenario: Create property with mandatory fields
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
        And User gets House for PropertyType
	When User creates property with defined address and Residential division with mandatory fields by Api
	Then User should get OK http status code
		And The created Property is saved in database

@Property
Scenario Outline: Create property with invalid data
	Given User gets <country> address form for <itemType> and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | <divisionCode>   |
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2           | Line3 | Postcode   | City | County |
			| updated abc  | 2              | 55 Baker Street |       | <postCode> |      |        |
        And User gets <propertyType> for PropertyType
		And Property characteristics are set for given property type
	When User creates property with defined address and <divisionCode> division by Api
	Then User should get <statusCode> http status code

	Examples:
	| country | itemType | postCode    | propertyType  | divisionCode | statusCode |
	| GB      | invalid  | 777         | House         | Residential  | BadRequest |
	| invalid | invalid  | 777         | Leisure.Hotel | Commercial   | BadRequest |
	| invalid | Property | 777         | Leisure.Hotel | Commercial   | BadRequest |
	| GB      | Property |             | Leisure.Hotel | Commercial   | BadRequest |
	| GB      | Property | 12345678901 | Bungalow      | Residential  | BadRequest |
	| GB      | Property | 777         | invalid       | Commercial   | BadRequest |
	| GB      | Property | 777         | House         | Commercial   | BadRequest |

@Property
Scenario Outline: Update property
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
			| Division     | Commercial       |
		And Property exists in database
			| PropertyType    | Division        |
			| <propertyType1> | <divisionCode1> |
		And Address for add/update property is defined with max length fields
		And User gets <propertyType2> for PropertyType
		And Property characteristics are set for given property type
	When User updates property with defined address for latest id and <divisionCode2> division by Api
	Then User should get OK http status code
		And The updated Property is saved in database

	Examples:
	| propertyType1                      | propertyType2         | divisionCode1 | divisionCode2 |
	| Leisure.Hotel                      | Leisure.Hotel         | Commercial    | Commercial    |
	| House                              | House                 | Residential   | Residential   |
	| Leisure.Hotel                      | Flat                  | Commercial    | Residential   |
	| Houseboat                          | Leisure.Hotel         | Residential   | Commercial    |
	| Retail                             | Retail.Car Showroom   | Commercial    | Commercial    |
	| Retail.Retail Unit A1              | Retail.Retail Unit A3 | Commercial    | Commercial    |
	| Industrial.Industrial Distribution | Industrial            | Commercial    | Commercial    |
	| Office                             | Other                 | Commercial    | Commercial    |

@Property
Scenario Outline: Update property with invalid data
    Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode | enumTypeItemCode |
		| Division     | Residential      |
		| Division     | Commercial       |
		And Property exists in database
			| PropertyType | Division        |
			| House        | <divisionCode1> |
		And User gets <country> address form for <itemType> and country details
		And User gets <propertyType> for PropertyType
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode   | City | County |
			|              |                |       |       | <postCode> |      |        |
		And Property characteristics are set for given property type
	When User updates property with defined address for <id> id and <divisionCode2> division by Api
	Then User should get <statusCode> http status code

	Examples:
	| id                                   | country | itemType | postCode    | propertyType | divisionCode1 | divisionCode2 | statusCode |
	| latest                               | GB      | invalid  | 777         | House        | Residential   | Residential   | BadRequest |
	| latest                               | invalid | invalid  | 777         | House        | Residential   | Residential   | BadRequest |
	| latest                               | invalid | Property | 777         | House        | Residential   | Residential   | BadRequest |
	| latest                               | GB      | Property |             | House        | Residential   | Residential   | BadRequest |
	| latest                               | GB      | Property | 12345678901 | House        | Residential   | Residential   | BadRequest |
	| 00000000-0000-0000-0000-000000000000 | GB      | Property | 123456      | House        | Residential   | Residential   | BadRequest |
	# Invalid type
	| latest                               | GB      | Property | 123456      | invalid      | Residential   | Residential   | BadRequest |
	| latest                               | GB      | Property | 123456      | House        | Residential   | Commercial    | BadRequest |
	| latest                               | GB      | Property | 123456      | Office       | Residential   | Residential   | BadRequest |

@Property
Scenario: Get non existing property
	When User retrieves property details
	Then User should get NotFound http status code

@Property
Scenario: Get property
	Given User gets GB address form for Property and country details
		And User gets Freehold Sale for ActivityType
		And User gets House for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property attributes exists in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000.1  | 3000.1  | 500.1       | 4000.1      | 1                   | 3                   |
        And Property characteristics are set for given property type
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
        And Contacts exists in database
		    | FirstName | LastName | Title |
		    | Michael   | Angel    | cheef |
		And Ownership Freeholder exists in database
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  | 1200000   |
			| 01-05-2014   | 01-04-2015 | 1000000  | 1200000   |
		And Activity for latest property and PreAppraisal activity status exists in database
		And Property has following charactersitics
			| CharacteristicCode | Text         |
			| Detached           | DetachedText |
			| Garden             | GardenText   |
	When User retrieves property details
	Then User should get OK http status code
		And The created Property is saved in database
        And Ownership list should be the same as in database
		And Activities list should be the same as in database
		And Characteristics list should be the same as in database

@Property
Scenario Outline: Create property with invalid characteristics data
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
		And Address for add/update property is defined with max length fields
        And User gets House for PropertyType
		And Followed property characteristics are chosen
			| Code   |
			| <code> | 
	When User creates property with defined address and Residential division by Api
	Then User should get <statusCode> http status code

	Examples:
	| code     | statusCode |
	| Offices  | BadRequest | 
	| invalid  | BadRequest |
