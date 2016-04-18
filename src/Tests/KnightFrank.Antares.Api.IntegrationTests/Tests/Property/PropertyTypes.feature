Feature: Property types

@Property
Scenario Outline: Get property types
	When User gets property types for <divisionCode> division and <countryCode> country
	Then User should get <statusCode> http status code

	Examples:
	| countryCode | divisionCode | statusCode |
	| GB          | Residential  | Ok         |
	| GB          | Commercial   | Ok         |
	| GB          |              | BadRequest |
	|             | Residential  | BadRequest |
	|             | Commercial   | BadRequest |
	| ZZZZZ       | Residential  | BadRequest |
	| GB          | Circus       | BadRequest |
	|             |              | BadRequest |
