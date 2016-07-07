Feature: Address form

@AddressForm
Scenario Outline: Get address template for invalid country
	When User retrieves address template for <entityType> entity type and <countryCode> contry code
	Then User should get <statusCode> http status code

	Examples: 
	| entityType  | countryCode | statusCode |
	|             |             | BadRequest |
	| bla         | bla         | BadRequest |

@AddressForm
Scenario: Get address template for Great Britain
	Given There is an AddressForm for GB country code		  	  
	When User retrieves address template for Property entity type and GB contry code
	Then User should get OK http status code

@AddressForm
Scenario Outline: Get list of all address template	  	  
	When User retrieves all address templates for <entityType> entity
	Then User should get <statusCode> http status code

Examples: 
	| entityType | statusCode |
	| Property   | OK         |
	| bla        | BadRequest |
