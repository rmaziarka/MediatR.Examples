Feature: Property attributes

@Property
@Attributes
Scenario Outline: Get property attributes
	Given User gets <propertyType> for PropertyType
	When User retrieves attributes for given property type and <countryCode> address
	Then User should get <statusCode> http status code

	Examples:
	| propertyType  | countryCode | statusCode |
	| House         | GB          | OK         |
	| Leisure.Hotel | GB          | OK         |
	| Leisure.Hotel | invalid     | BadRequest |
	| invalid       | GB          | BadRequest |
