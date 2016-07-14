Feature: Tenancy 

@Tenancies
Scenario: Create tenancy
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
		| Title  | FirstName | LastName |
		| Mister | Tomasz    | Bien     |
		And Requirement of type ResidentialLetting exists in database
	When User creates tenancy for latest requirement and activity
	Then User should get OK http status code
		And Tenancy should be the same as added