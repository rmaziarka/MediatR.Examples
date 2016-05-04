Feature: Characteristic Groups

Scenario: Get charactersitics
	Given User gets House for PropertyType
		And User retrieves GB country id
	When User retrieves characteristics for given country and defined property type
	Then User should get OK http status code

Scenario Outline: Check error codes for get charactersitics method
	Given User gets House for PropertyType
		And User retrieves GB country id
	When User tries to retrieves characteristics for <country> country and <propertyType> property type
	Then User should get <statusCode> http status code

	Examples: 
	| country | propertyType | statusCode |
	|         | valid        | BadRequest |
	| bla     | valid        | BadRequest |
	| valid   |              | BadRequest |
	| valid   | bla          | BadRequest |
