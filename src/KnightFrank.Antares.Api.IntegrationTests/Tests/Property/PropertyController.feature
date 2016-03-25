Feature: Add, update and view property

@Property
Scenario: Create property
	Given User gets GB address form for Property and country details
		And Address for add/update property is defined
				| PropertyName | PropertyNumber | Line2           | Line3 | Postcode | City | County |
				| updated abc  | 2              | 55 Baker Street |       | N1C      |      |        |
	When User creates property with defined address by Api
	Then User should get OK http status code
		And The created Property is saved in data base

@Property
Scenario Outline: Try to create property with invalid data
	Given User gets <country> address form for <itemType> and country details
		And Address for add/update property is defined
				| PropertyName | PropertyNumber | Line2           | Line3 | Postcode   | City | County |
				| updated abc  | 2              | 55 Baker Street |       | <postCode> |      |        |
	When User creates property with defined address by Api
	Then User should get <statusCode> http status code

	Examples: 
	| country | itemType | postCode    | statusCode |
	| GB      | bla      | 777         | BadRequest |
	| bla     | bla      | 777         | BadRequest |
	| bla     | Property | 777         | BadRequest |
	| GB      | Property |             | BadRequest |
	| GB      | Property | 12345678901 | BadRequest |



@Property
Scenario: Update property in DB
	Given User gets GB address form for Property and country details
		And Property with Address is in data base
			| PropertyName | PropertyNumber | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2           | Line3 | Postcode | City | County |
			| updated abc  | 2              | 55 Baker Street |       | N1C      |      |        |
	When Users updates property with defined address for latest id by Api
	Then User should get OK http status code
		And The updated Property is saved in data base


@Property
Scenario Outline: Update non exisitng property
	Given User gets GB address form for Property and country details
		And Property with Address is in data base
				| PropertyName | PropertyNumber | Line2              | Line3      | Postcode | City   | County         |
				| abc          | 1              | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And User gets <country> address form for <itemType> and country details
		And Address for add/update property is defined
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode   | City | County |
			|              |                |       |       | <postCode> |      |        |
	When Users updates property with defined address for <id> id by Api
	Then User should get <statusCode> http status code

	Examples: 
	| id                                   | country | itemType | postCode    | statusCode |
	| latest                               | GB      | bla      | 777         | BadRequest |
	| latest                               | bla     | bla      | 777         | BadRequest |
	| latest                               | bla     | Property | 777         | BadRequest |
	| latest                               | GB      | Property |             | BadRequest |
	| latest                               | GB      | Property | 12345678901 | BadRequest |
	| 00000000-0000-0000-0000-000000000000 | GB      | Property | 123456      | BadRequest |

@Property
Scenario: Get non existing property
	Given Property does not exist in DB
	When User retrieves property details
	Then User should get NotFound http status code