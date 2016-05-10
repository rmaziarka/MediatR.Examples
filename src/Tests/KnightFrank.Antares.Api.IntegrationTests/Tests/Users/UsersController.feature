Feature: Users
@Users
Scenario Outline: Retrieve users by query
	Given User creates users in database with the following data
		| activeDirectoryDomain | activeDirectoryLogin | firstName | lastName | 
			| AD                    | jsmith               | John      | Smith    | 
			| AD                    | jjohns               | John      | Johns    |
			| AD                    | dparks               | Dave      | Parks    |
	When User inputs <query> query
	Then User should get <statusCode>  http status code
		And User should get <matchCount> number of results returned
		And User should get results in correct format
	Examples: 
	| query | statusCode | matchCount |
	| j     | OK         | 2          |
	 
@Users
Scenario Outline: Retrieve error message for improper input
	When User inputs <query> query
	Then User should get <statusCode> http status code

	Examples:
	| query | statusCode |
	|       | BadRequest |
