Feature: Activities

@Activity
Scenario Outline: Create activity with invalid data
	Given User gets <activityTypeCode> for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| <activityStatusId>     | PreAppraisal     |
			| UserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
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
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| UserType               | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Contacts exists in database
			| FirstName | Surname | Title |
			| Michael   | Angel   | cheef |
			| Michael   | Angel   | cook  |
		And Ownership Freeholder exists in database
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
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| UserType               | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User gets activity with latest id
	Then User should get OK http status code
		And Activity details should be the same as already added

@Activity
Scenario: Record and update residential sale valuation
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| UserType               | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User updates activity latest id and latest status with following sale valuation
		| MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
		| 1                    | 2                | 3                    |
	Then User should get OK http status code
		And Activity details should be the same as already added

@Activity
Scenario Outline: Record and update residential sale valuation using invalid data
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| Division               | Residential      |
			| ActivityStatus         | PreAppraisal     |
			| UserType               | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
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
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | NotSelling       |
			| UserType               | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and NotSelling activity status exists in database
	When User gets activities
	Then User should get OK http status code
		And Retrieved activities should be the same as in database

@Activity
Scenario: Get Activity with viewing and offer
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| UserType               | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Viewing exists in database
		And Offer with New status exists in database
	When User gets activity with latest id
	Then User should get OK http status code
		And Retrieved activity should have expected viewing
		And Retrieved activity should have expected offer
