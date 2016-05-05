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
		And User details should have the expected values
	
	Examples: 
	| query | statusCode |
	| j     | OK         |
	| smith | OK         |

	
	

@Users
Scenario: Retrieve users by name query
	Given All users have been deleted
		And User creates users in database with the following data
			| activeDirectoryDomain | activeDirectoryLogin | firstName | lastName | 
			| AD                    | jsmith               | John      | Smith    | 
			| AD                    | jjohns               | John      | Johns    |
			| AD                    | dparks               | Dave      | Parks    |
	When User searches for users with the following query
		| query  |
		| j| 
	Then User should get OK http status code


@Users
Scenario: Retrieve no users by name query
	Given All users have been deleted
		And User creates users in database with the following data
			| activeDirectoryDomain | activeDirectoryLogin | firstName | lastName | 
			| AD                    | jsmith               | John      | Smith    | 
			| AD                    | jjohns               | John      | Johns    |
			| AD                    | dparks               | Dave      | Parks    |
	When User searches for users with the following query
		| query |
		| bob   |
	Then User should get OK http status code
		And User details should have the expected values

@Users
Scenario Outline: Retrieve error message for improper input
	When User inputs <query> query
	Then User should get <statusCode> http status code

	Examples:
	| query | statusCode |
	|       | BadRequest |
	| NULL  | BadRequest |
