Feature: Viewings

@Viewings
Scenario: Create viewing
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
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
	When User creates viewing using api
	Then User should get OK http status code
		And Viewing details should be the same as already added

@Viewings
Scenario: Create viewing with mandatory fields
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
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
	When User creates viewing with mandatory fields using api
	Then User should get OK http status code
		And Viewing details should be the same as already added

@Viewings
Scenario Outline: Create viewing with invalid data
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
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
	When User creates viewing with invalid <data> using api
	Then User should get BadRequest http status code

	Examples: 
	| data        |
	| requirement |
	| activity    |
	| attendee    |

@Viewings
Scenario: Update viewing
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
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| Tom       | Jones   | Sir    |
		And Requirement exists in database
		And User creates viewing in database
	When User updates viewing 
	Then User should get OK http status code
		And Viewing details should be the same as already added

@Viewings
Scenario Outline: Update viewing with invalid data
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
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets GB address form for Requirement and country details
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| Tom       | Jones   | Sir    |
		And Requirement exists in database
		And User creates viewing in database
	When User updates viewing with invalid <data> data
	Then User should get BadRequest http status code

	Examples: 
	| data        |
	| attendee    |
	| viewing     |
