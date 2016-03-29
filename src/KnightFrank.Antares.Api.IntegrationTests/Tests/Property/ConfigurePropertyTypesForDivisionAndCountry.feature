Feature: Configure property types for a Division and a Country

Scenario Outline: Get property types from web api
	When user gets property types for <DivisionCode> division and <CountryCode> country
	Then User should get <StatusCode> http status code

	Examples:
	| CountryCode | DivisionCode | StatusCode |
	| GB          | Residential  | Ok         |
	| GB          |              | BadRequest |
	|             | Residential  | BadRequest |
	| ZZZZZ       | Residential  | BadRequest |
	| GB          | Circus       | BadRequest |
	|             |              | BadRequest |
