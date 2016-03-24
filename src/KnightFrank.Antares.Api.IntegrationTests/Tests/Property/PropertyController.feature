Feature: Add, update and view property

@ignore
@Property
Scenario: Create property
	When User gets GB address form for Property and country details
	When User creates property with following data 
		| PropertyName    | PropertyNumber | Line1              | Line2      | Line3 | Postcode | City   | County         | Street | Country |
		| Beautifull Flat | 1              | Lewis Cubit Square | King Cross |       | N1C      | London | Greater London |        |         |
	Then User should get <statusCode> http status code

@Property
@ignore
Scenario Outline: Try to create property with invalid data
	When User gets <country> address form for <itemType> and country details
	When User creates property with following data 
		| PropertyName | PropertyNumber | Line1 | Line2 | Line3 | Postcode   | City | County | Street | Country |
		|              |                |       |       |       | <postCode> |      |        |        |         |
	Then User should get <statusCode> http status code

	Examples: 
	| country | itemType | postCode    | statusCode |
	| GB      | bla      | 777         | BadRequest |
	| bla     | bla      | 777         | BadRequest |
	| bla     | Property | 777         | BadRequest |
	| GB      | Property |             | BadRequest |
	| GB      | Property | 12345678901 | BadRequest |



@Property
@ignore
Scenario: Update property in DB
	Given User gets GB address form for Property and country details
		And Property with Address is in data base
			| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Address is defined
			| PropertyName | PropertyNumber | Line1     | Line2           | Line3 | Postcode | City | County |
			| updated abc  | 2              | ugly Flat | 55 Baker Street |       | N1C      |      |        |
	When Users updates property with defined address for latest id
	Then User should get OK http status code
		And The updated Property is saved in data base


@Property
@ignore
Scenario Outline: Update non exisitng property
	Given User gets <country> address form for <itemType> and country details
		And Address is defined
			| PropertyName | PropertyNumber | Line1 | Line2 | Line3 | Postcode   | City | County |
			|              |                |       |       |       | <postCode> |      |        |
		And Property with Address is in data base
			| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
	When Users updates property with defined address for <id> id
	Then User should get <statusCode> http status code

	Examples: 
	| id                                   | country | itemType | postCode    | statusCode |
	| latest                               | GB      | bla      | 777         | BadRequest |
	| latest                               | bla     | bla      | 777         | BadRequest |
	| latest                               | bla     | Property | 777         | BadRequest |
	| latest                               | GB      | Property |             | BadRequest |
	| latest                               | GB      | Property | 12345678901 | BadRequest |
	| 00000000-0000-0000-0000-000000000000 | GB      | Property | 123456      | BadRequest |
