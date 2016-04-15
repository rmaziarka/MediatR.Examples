Feature: Activity type

@Activity
@ignore
Scenario Outline: Get activity types
	When User gets activity types for <propertyType> property type and <countryCode> country
	Then User should get <statusCode> http status code

	Examples: 
	| propertyType | countryCode | statusCode |
	| House        | GB          | OK         |
	| Office       | GB          | OK         |
	| Agricultural | GB          | OK         |
	| Invalid      | GB          | BadRequest |
	| House        | Invalid     | BadRequest |
