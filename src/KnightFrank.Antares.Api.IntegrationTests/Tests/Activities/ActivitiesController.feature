Feature: ActivitiesController

@Activities
Scenario Outline: Retrieve error messages for improper data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode       | enumTypeItemCode |
			| OwnershipType      | Freeholder       |
			| <activityStatusId> | PreAppraisal     |
			| Division           | Residential      |
		And Property with Address and Residential is in data base
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London | 
	When User creates activity for given <propertyId> property id
	Then User should get <statusCode> http status code

	Examples: 
	| propertyId                           | activityStatusId                     | statusCode |
	| 00000000-0000-0000-0000-000000000002 | ActivityStatus                       | BadRequest |
	| latest                               | 00000000-0000-0000-0000-000000000001 | BadRequest |

@Activities
Scenario: Create Activity for an existing property
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| OwnershipType  | Freeholder       |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And Property with Address and Residential is in data base
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
	When User creates activity for given latest property id
	Then User should get OK http status code
		And The created Activity is saved in data base
		
@Activities
Scenario Outline: Get Activity by incorrect activity id
	When User retrieves activity details for given <activityId>
	Then User should get <expectedStatusCode> http status code

	Examples: 
	| activityId                           | expectedStatusCode |
	|                                      | MethodNotAllowed   |
	| a                                    | BadRequest         |
	| 00000000-0000-0000-0000-000000000001 | NotFound           |


@Activities
Scenario: Get Activity by correct activity id
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And Property with Address and Residential is in data base
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |  
		And User creates activity for given latest property id
	When User retrieves activity
	Then User should get OK http status code
		And The received Activities should be the same as in DB


@Activities
Scenario: record and update residential sale valuation
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And Property with Address and Residential is in data base
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for 'latest' property exists in data base
	When User updates activity 'added' id and 'added' status with following sale valuation
		| MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice | 
		| 1                    | 2                | 3                    | 
	Then User should get OK http status code
		And The received Activities should be the same as in DB


@Activities
Scenario Outline: try record and update residential sale valuation for improper data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Residential      |
			| ActivityStatus | PreAppraisal     |
		And Property with Address and Residential is in data base
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for 'latest' property exists in data base
	When User updates activity '<activityId>' id and '<activityStatusID>' status with following sale valuation
		| MarketAppraisalPrice   | RecommendedPrice | VendorEstimatedPrice |
		| <marketAppraisalPrice> | 2                | 3                    | 
	Then User should get <statusCode> http status code

	Examples: 
	| activityId                           | activityStatusID                     | marketAppraisalPrice | statusCode |
	| 00000000-0000-0000-0000-000000000002 | added                                | 1                    | BadRequest |
	| added                                | 00000000-0000-0000-0000-000000000001 | 2                    | BadRequest |
	#| added                                | added                                | a                    | BadRequest |
