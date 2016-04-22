Feature: CharacteristicGroupsController


Scenario: Get charactersitics
	Given I have House property type id
		And I have GB country id
	When User retrieves characteristics for GB country and defined property type
	Then User should get OK http status code


Scenario Outline: Check error codes for get charactersitics method
	When User try to retrieves characteristics for <country> country and <propertyType> property type
	Then User should get <statusCode> http status code

	Examples: 
	| country | propertyType | statusCode |
	|         | bla          | BadRequest |
	| bla     | bla          | BadRequest |
	|         |              | BadRequest |

