Feature: Companies

@Company
Scenario: Create new company with required fields
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | cheef | 
	When User creates company with required fields using api
	Then User should get OK http status code
		And Company should be same as in database

@Company
Scenario: Create new company with all fields
	Given Contacts exists in database
		| FirstName | LastName | Title | 
		| Michael   | Angel    | Mr    |
	And Users exists in database
		| activeDirectoryDomain | activeDirectoryLogin | firstName | lastName |
		| AD                    | tester               | John      | Smith    |
	 When User creates company using api
	 	| Name         | WebsiteUrl  | ClientCarePageUrl  |
	 	| Test Company | www.api.com | www.clientcare.com | 
	 Then User should get OK http status code
		 And Company should be same as in database

@Company
Scenario Outline: Create company with invalid data
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel    | ceo   |
	And Users exists in database
		| activeDirectoryDomain | activeDirectoryLogin | firstName | lastName |
		| AD                    | tester               | John      | Smith    |
	When User creates company with invalid <data> using api
	Then User should get BadRequest http status code

	Examples:
	| data |
	| name         |
	| status       |
	| contact      |
	| company type |
	| category     |
	| relationship |

@Company
Scenario: Get non existing company
	When User gets company details
	Then User should get NotFound http status code

@Company
Scenario: Get company with invalid query
	When User gets company details with invalid query
	Then User should get BadRequest http status code

@Company
Scenario: Get company details
	Given Contacts exists in database
			| FirstName | LastName | Title |
			| Michael   | Angel    | ceo   |
		And Company exists in database
	When User gets company details
	Then User should get OK http status code
		And Company should be same as in database

@Company
Scenario: Update company
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | cheef | 
		And Company exists in database
	When User updates company using api
	Then User should get OK http status code
		And Company should be updated

@Company
Scenario Outline: Update company with invalid data
	Given Contacts exists in database
		| FirstName | LastName | Title |
		| Michael   | Angel   | cheef | 
		And Users exists in database
		| activeDirectoryDomain | activeDirectoryLogin | firstName | lastName |
		| AD                    | tester               | John      | Smith    |
		And Company exists in database
	When User updates company with invalid <data> using api
	Then User should get BadRequest http status code

	Examples: 
	| data         |
	| name         |
	| status       |
	| contact      |
	| company type |
	| category     |
	| relationship |