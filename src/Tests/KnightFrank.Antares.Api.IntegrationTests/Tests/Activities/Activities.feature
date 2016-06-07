Feature: Activities

@Activity
Scenario Outline: Create activity with invalid data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets <activityTypeCode> for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| OwnershipType          | Freeholder       |
			| <activityStatusId>     | PreAppraisal     |
			| Division               | Residential      |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
	When User creates activity for given <propertyId> property id using api 
	Then User should get <statusCode> http status code

	Examples:
	| propertyId                           | activityStatusId                     | activityTypeCode | statusCode |
	| 00000000-0000-0000-0000-000000000002 | ActivityStatus                       | Freehold Sale    | BadRequest |
	| latest                               | 00000000-0000-0000-0000-000000000001 | Freehold Sale    | BadRequest |
	| latest                               | ActivityStatus                       | Assignment       | BadRequest |
	| latest                               | ActivityStatus                       | invalid          | BadRequest |

@Activity
Scenario: Create Activity
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| OwnershipType          | Freeholder       |
			| ActivityStatus         | PreAppraisal     |
			| Division               | Residential      |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And User creates contacts in database with following data
			| FirstName | Surname | Title |
			| Michael   | Angel   | cheef |
			| Michael   | Angel   | cook  |
		And Ownership exists in database
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  |           |
			| 01-05-2014   |            | 1000000  |           |
	When User creates activity for given latest property id using api
	Then User should get OK http status code
		And Activity details should be the same as already added

@Activity
Scenario Outline: Get Activity using invalid data
	When User gets activity with <activityId> id
	Then User should get <statusCode> http status code

	Examples:
	| activityId                           | statusCode |
	| a                                    | BadRequest |
	| 00000000-0000-0000-0000-000000000001 | NotFound   |

@Activity
Scenario: Get Activity
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| Division               | Residential      |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
		And Property characteristics are set for given property type
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User gets activity with latest id
	Then User should get OK http status code
		And Activity details should be the same as already added

@Activity
Scenario: Record and update residential sale valuation
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| Division               | Residential      |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User updates activity latest id and latest status with following sale valuation
		| MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
		| 1                    | 2                | 3                    |
	Then User should get OK http status code
		And Activity details should be the same as already added

@Activity
Scenario Outline: Record and update residential sale valuation using invalid data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| Division               | Residential      |
			| ActivityStatus         | PreAppraisal     |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User updates activity <activityId> id and <activityStatusID> status with following sale valuation
		| MarketAppraisalPrice   | RecommendedPrice | VendorEstimatedPrice |
		| <marketAppraisalPrice> | 2                | 3                    |
	Then User should get <statusCode> http status code

	Examples:
	| activityId                           | activityStatusID                     | marketAppraisalPrice | statusCode |
	| 00000000-0000-0000-0000-000000000002 | latest                               | 1                    | BadRequest |
	| latest                               | 00000000-0000-0000-0000-000000000001 | 2                    | BadRequest |

@Activity
Scenario: Get all activities
	Given All activities have been deleted from database
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| Division               | Residential      |
			| ActivityStatus         | NotSelling       |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions |
			| 1           | 3           | 1             | 3             |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and NotSelling activity status exists in database
	When User gets activities
	Then User should get OK http status code
		And Retrieved activities should be the same as in database
			| PropertyName | PropertyNumber | Line2              |
			| abc          | 1              | Lewis Cubit Square |

@Activity
Scenario: Get Activity with viewing and offer
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| Division               | Residential      |
			| ActivityUserType       | LeadNegotiator   |
			| OfferStatus            | New              |
			| ActivityDepartmentType | Managing         |
			| ActivityDepartmentType | Standard         |
		And Property characteristics are set for given property type
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
 			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | Description |
		And User creates viewing in database
		And User creates New offer in database
	When User gets activity with latest id
	Then User should get OK http status code
		And Retrieved activity should have expected viewing
		And Retrieved activity should have expected offer
