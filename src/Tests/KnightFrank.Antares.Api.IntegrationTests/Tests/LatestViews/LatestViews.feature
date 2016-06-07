Feature: Latest views

@LatestViews
Scenario: Create latest viewed property
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
        And User gets House for PropertyType
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 1            | 1              | 1     | 1     | 1        | 1    | 1      |
	When User adds Property to latest viewed entities using api
	Then User should get OK http status code
		And Retrieved latest view should contain Property entity

@LatestViews
Scenario: Create latest viewed activity
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| Division               | Residential      |
			| ActivityStatus         | PreAppraisal     |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
        And User gets House for PropertyType
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 1            | 1              | 1     | 1     | 1        | 1    | 1      |
		And User gets Freehold Sale for ActivityType
		And Activity for latest property and PreAppraisal activity status exists in database
	When User adds Activity to latest viewed entities using api
	Then User should get OK http status code
		And Retrieved latest view should contain Activity entity

@LatestViews
Scenario: Create latest viewed requirement
	Given User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| Adam      | Malysz  | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
 			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | Description |
	When User adds Requirement to latest viewed entities using api
	Then User should get OK http status code
		And Retrieved latest view should contain Requirement entity

@LatestViews
Scenario: Create latest view using invalid entity type
	When User creates latest view using invalid entity type
	Then User should get BadRequest http status code

@LatestViews
Scenario: Get latest viewed properties
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
        And User gets House for PropertyType
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 1            | 1              | 1     | 1     | 1        | 1    | 1      |
		And Property is added to latest views
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 2            | 2              | 2     | 2     | 2        | 2    | 2      |
		And Property is added to latest views
		And Property is added to latest views
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 3            | 3              | 3     | 3     | 3        | 3    | 3      |
		And Property is added to latest views
	When User gets latest viewed entities
	Then User should get OK http status code
		And Latest viewed details should match Property entities

@LatestViews
Scenario: Get latest viewed activities
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| Division               | Residential      |
			| ActivityStatus         | PreAppraisal     |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
        And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 10           | 10             | 10    | 10    | 10       | 10   | 10     |
		And Activity for latest property and PreAppraisal activity status exists in database
		And Activity is added to latest views
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 20           | 20             | 20    | 20    | 20       | 20   | 20     |
		And Activity for latest property and PreAppraisal activity status exists in database
		And Activity is added to latest views
		And Activity is added to latest views
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 30           | 30             | 30    | 30    | 30       | 30   | 30     |
		And Activity for latest property and PreAppraisal activity status exists in database
		And Activity is added to latest views
	When User gets latest viewed entities
	Then User should get OK http status code
		And Latest viewed details should match Activity entities

@LatestViews
Scenario: Get latest viewed requirements
	Given User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
 			| 1        | 1        | 1           | 1           | 1                 | 1                 | 1            | 1            | 1                | 1                | 1       | 1       | 1           | 1           | 1           |
		And Requirement is added to latest views
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
 			| 2        | 2        | 2           | 2           | 2                 | 2                 | 2            | 2            | 2                | 2                | 2       | 2       | 2           | 2           | 2           |
		And Requirement is added to latest views
		And Requirement is added to latest views
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
 			| 3        | 3        | 3           | 3           | 3                 | 3                 | 3            | 3            | 3                | 3                | 3       | 3       | 3           | 3           | 3           |
		And Requirement is added to latest views
	When User gets latest viewed entities
	Then User should get OK http status code
		And Latest viewed details should match Requirement entities
