Feature: Metadata

@Metadata
Scenario: Get activity attributes
	Given User gets Open Market Letting for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| Flat         | Residential |
	When User gets activity preview attributes
	Then User should get OK http status code

@Metadata
Scenario Outline: Get activity attributes with invalid data
	Given User gets Open Market Letting for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| Flat         | Residential |
	When User gets activity preview attributes with invalid <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data         | statusCode |
	| propertyType | BadRequest |
	| activityType | BadRequest |
	| page         | BadRequest |

	
@Metadata
Scenario Outline: Get activity attributes with empty data
	Given User gets Open Market Letting for ActivityType
		And Property exists in database
			| PropertyType | Division    |
			| Flat         | Residential |
	When User gets activity preview attributes with empty <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data     | statusCode |
	| property | BadRequest |
	| activity | BadRequest |
	| page     | BadRequest |
