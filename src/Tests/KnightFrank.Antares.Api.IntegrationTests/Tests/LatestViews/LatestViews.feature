Feature: Latest views

@LatestViews
Scenario: Create latest viewed property
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
	When User adds Property to latest viewed entities using api
	Then User should get OK http status code
		And Retrieved latest view should contain Property entity

@LatestViews
Scenario: Create latest viewed activity
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
	When User adds Activity to latest viewed entities using api
	Then User should get OK http status code
		And Retrieved latest view should contain Activity entity

@LatestViews
Scenario: Create latest viewed requirement
	Given Contacts exists in database
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister |
		| Adam      | Malysz  | Mister |
		And Requirement exists in database
	When User adds Requirement to latest viewed entities using api
	Then User should get OK http status code
		And Retrieved latest view should contain Requirement entity

@LatestViews
Scenario: Create latest view using invalid entity type
	When User creates latest view using invalid entity type
	Then User should get BadRequest http status code

@LatestViews
Scenario: Get latest viewed properties
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Property is added to latest views
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Property is added to latest views
		And Property is added to latest views
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Property is added to latest views
	When User gets latest viewed entities
	Then User should get OK http status code
		And Latest viewed details should match Property entities

@LatestViews
Scenario: Get latest viewed activities
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Activity is added to latest views
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Activity is added to latest views
		And Activity is added to latest views
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Activity is added to latest views
	When User gets latest viewed entities
	Then User should get OK http status code
		And Latest viewed details should match Activity entities

@LatestViews
Scenario: Get latest viewed requirements
	Given Contacts exists in database 
		| FirstName | Surname | Title |
		| Tomasz    | Bien    | Sir   |
		And Requirement exists in database
		And Requirement is added to latest views
		And Contacts exists in database
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Requirement is added to latest views
		And Requirement is added to latest views
		And Contacts exists in database
			| FirstName | Surname | Title |
			| Tomasz    | Bien    | Dude  |
		And Requirement exists in database
		And Requirement is added to latest views
	When User gets latest viewed entities
	Then User should get OK http status code
		And Latest viewed details should match Requirement entities