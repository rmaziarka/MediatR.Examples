Feature: Offers

@Offers
Scenario: Create residential sales offer
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Requirement exists in database
	When User creates New offer using api
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Create residential sales offer with mandatory fields
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database 
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Requirement exists in database
	When User creates Accepted offer with mandatory fields using api
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario Outline: Create residential sales offer with invalid data
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Requirement exists in database
	When User creates offer with invalid <data> using api
	Then User should get BadRequest http status code

	Examples: 
	| data        |
	| requirement |
	| activity    |
	| status      |

@Offers
Scenario: Get residential sales offer
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Offer with New status exists in database
	When User gets offer for latest id
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Get Accepted residential sales offer
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Jon       | Lajoie  | Dude  |
		And Company exists in database
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Requirement exists in database
		And Offer with Accepted status exists in database
	When User gets offer for latest id
	Then User should get OK http status code
		And Offer details should be the same as already added

@Offers
Scenario: Get residential sales offer with invalid data
	When User gets offer for invalid id
	Then User should get NotFound http status code

@Offers
Scenario: Update residential sales offer
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Offer with New status exists in database
	When User updates offer with New status
	Then User should get OK http status code
		And Offer details should be updated

@Offers
Scenario Outline: Update accepted residential sales offer
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Jon       | Lajoie  | Dude  |
		And Company exists in database
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Requirement exists in database
		And Offer with <offerStatus> status exists in database
		And Contacts exists in database
			| FirstName | LastName | Title |
			| Adam      | Lajoie  | Sir   |
		And Company exists in database
	When User updates offer with <newOfferStatus> status
	Then User should get OK http status code
		And Offer details should be updated

	Examples:
	| offerStatus | newOfferStatus |
	| New         | Accepted       |
	| Accepted    | Accepted       |
	| Accepted    | New            |

@Offers
Scenario Outline: Update residential sales offer with invalid data
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien     | Mister |
		And Requirement exists in database
		And Offer with New status exists in database
	When User updates New offer with invalid <data> data
	Then User should get BadRequest http status code

	Examples: 
	| data   |
	| status |
	| offer  |

@Offers
Scenario Outline: Update accepted residential sales offer with invalid data
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Jon       | Lajoie  | Dude  |
		And Company exists in database
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| FirstName | LastName | Title  |
			| Tomasz    | Bien    | Mister |
		And Requirement exists in database
		And Offer with Accepted status exists in database
	When User updates Accepted offer with invalid <data> data
	Then User should get BadRequest http status code

	Examples: 
	| data                      |
	| broker                    |
	| brokerCompany             |
	| lender                    |
	| lenderCompany             |
	| surveyor                  |
	| surveyorCompany           |
	| additionalSurveyor        |
	| additionalSurveyorCompany |
	| mortgageStatus            |
	| mortgageSurveyStatus      |
	| additionalSurveyStatus    |
	| searchStatus              |
	| enquiries                 |
