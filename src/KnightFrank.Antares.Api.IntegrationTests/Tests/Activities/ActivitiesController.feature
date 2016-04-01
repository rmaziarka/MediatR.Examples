Feature: ActivitiesController

@ignore
@Activities
Scenario Outline: Retrieve error messages for improper data
	Given There is activity type id and activity status id defined
	When User creates activity with following data
		| PropertyId   | ActivityTypeId   | ActivityStatusId   |
		| <propertyId> | <activityTypeId> | <activityStatusId> |
	Then User should get <statusCode> http status code

	Examples: 
	| propertyId                           | activityTypeId                       | activityStatusId | statusCode |
	| 00000000-0000-0000-0000-000000000000 |                                      |                  | BadRequest |
	| bla                                  | 00000000-0000-0000-0000-000000000000 |                  | BadRequest |
	|                                      |                                      |                  | BadRequest |

@ignore
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
				| 01-05-2011   | 01-04-2013 | 1000000  | 1200000   |
				| 01-05-2014   | 01-04-2015 | 1000000  | 1200000   |
		When User creates activity with following data
			| PropertyId | ActivityTypeId | ActivityStatusId |
			| latest     | latest         | latest           |
