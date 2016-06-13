Feature: Users

@Users
Scenario Outline: Get users
	Given All users have been deleted
		And Users exists in database
			| activeDirectoryDomain | activeDirectoryLogin | firstName | lastName |
			| AD                    | jsmith               | John      | Smith    |
			| AD                    | jjohns               | John      | Johns    |
			| AD                    | dparks               | Dave      | Parks    |
	When User inputs <query> query
	Then User should get <statusCode> http status code
		And User should get <matchCount> number of results returned
		And User should get results in correct format

	Examples:
	| query | statusCode | matchCount |
	| j     | OK         | 2          |
	| J     | OK         | 2          |
	| d p   | OK         | 1          |
	| D p   | OK         | 1          |
	| d P   | OK         | 1          |
	| D P   | OK         | 1          |

@Users
Scenario Outline: Get users with invalid data
	When User inputs <query> query
	Then User should get <statusCode> http status code

	Examples:
	| query | statusCode |
	|       | BadRequest |

	
@Users
Scenario: Get user
Given All users have been deleted
	And Users exists in database
         | activeDirectoryDomain | activeDirectoryLogin | firstName | lastName |
         | AD                    | jsmith               | John      | Smith    |
When User gets user with latest Id
Then User should get OK http status code
	And User details should be same as in database