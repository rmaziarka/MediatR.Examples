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
			| enumTypeCode     | enumTypeItemCode |
			| Division         | Residential      |
			| ActivityStatus   | PreAppraisal     |
			| ActivityUserType | LeadNegotiator   |
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
			| enumTypeCode     | enumTypeItemCode |
			| Division         | Residential      |
			| ActivityStatus   | PreAppraisal     |
			| ActivityUserType | LeadNegotiator   |
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
