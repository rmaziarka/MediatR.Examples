Feature: Address form

@AddressForm
Scenario Outline: Retrieve error messages for improper EntityType and CountryCode
	When User retrieves address template for <entityType> entity type and <countryCode> contry code
	Then User should get <statusCode> http status code

	Examples: 
	| entityType  | countryCode | statusCode |
	|             |             | BadRequest |
	| bla         | bla         | BadRequest |

@AddressForm
Scenario: Get proper address template for Great Britain
	Given There is AddressForm for GB country code		  	  
	When User retrieves address template for Property entity type and GB contry code
	Then User should get OK http status code
