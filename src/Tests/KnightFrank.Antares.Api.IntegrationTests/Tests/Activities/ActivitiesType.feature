Feature: Activities types

@Activity
Scenario Outline: Get activity types
	Given User gets <propertyType> for PropertyType
	When User gets activity types for property and <countryCode> country
	Then User should get <statusCode> http status code

	Examples: 
	| propertyType  | countryCode | statusCode |
	| House         | GB          | OK         |
	| Retail.Retail | GB          | OK         |
	| invalid       | GB          | BadRequest |
	| House         | invalid     | BadRequest |
	| invalid       | invalid     | BadRequest |
