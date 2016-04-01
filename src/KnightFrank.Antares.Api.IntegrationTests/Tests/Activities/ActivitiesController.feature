Feature: ActivitiesController

@ignore
@Activities
Scenario Outline: Retrieve error messages for improper data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode       | enumTypeItemCode |
			| OwnershipType      | Freeholder       |
			| <activityStatusId> | PreAppraisal     |
		And Property with Address is in data base
        		| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        		| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London | 
	When User creates activity for given <propertyId> property id
	Then User should get <statusCode> http status code

	Examples: 
	| propertyId                           | activityStatusId                     | statusCode |
	| 00000000-0000-0000-0000-000000000000 | ActivityStatus                       | BadRequest |
	| latest                               | 00000000-0000-0000-0000-000000000000 | BadRequest |


@ignore
@Activities
Scenario: Create Activity for an existing property
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
				| 01-05-2011   | 01-04-2013 | 1000000  |           |
				| 01-05-2014   | 01-04-2015 | 1000000  |           |
		When User creates activity for given latest property id
		Then User should get OK http status code

