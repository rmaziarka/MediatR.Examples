Feature: Latest views

@LatestViews
Scenario: Create latest view
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Residential      |
        And User gets House for PropertyType
		And Property with Address and Residential division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			| 1            | 1              | 1     | 1     | 1        | 1    | 1      |
	When User creates latest view
	Then User should get OK http status code
		And Retrieved latest view should contain property

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
		And Latest viewed properties details should match viewed properties
