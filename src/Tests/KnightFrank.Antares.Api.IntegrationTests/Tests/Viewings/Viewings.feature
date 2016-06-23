Feature: Viewings

@Viewings
Scenario: Create viewing
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
	When User creates viewing using api
	Then User should get OK http status code
		And Viewing details should be the same as already added

@Viewings
Scenario: Create viewing with mandatory fields
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
	When User creates viewing with mandatory fields using api
	Then User should get OK http status code
		And Viewing details should be the same as already added

@Viewings
Scenario Outline: Create viewing with invalid data
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
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
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| Tom       | Jones   | Sir    |
		And Requirement exists in database
		And Viewing exists in database
	When User updates viewing 
	Then User should get OK http status code
		And Viewing details should be the same as already added

@Viewings
Scenario Outline: Update viewing with invalid data
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |		
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
			| Tom       | Jones   | Sir    |
		And Requirement exists in database
		And Viewing exists in database
	When User updates viewing with invalid <data> data
	Then User should get BadRequest http status code

	Examples: 
	| data        |
	| attendee    |
	| viewing     |
