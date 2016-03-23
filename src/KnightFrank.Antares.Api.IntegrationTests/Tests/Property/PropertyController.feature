Feature: Add, update and view property

@Property
@ignore
Scenario: Create property
	When User gets GB address form for Property and country details
	When User creates property with following data 
		| PropertyName    | PropertyNumber | Line1              | Line2      | Line3 | Postcode | City   | County         | Street | Country |
		| Beautifull Flat | 1              | Lewis Cubit Square | King Cross |       | N1C      | London | Greater London |        |         |
	Then User should get <statusCode> http status code

@Property
@ignore
Scenario: Update property in DB
	Given User gets GB address form for Property and country details
		And Property with Address is in data base
			| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
			| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
	When Users updates property
		| PropertyName | PropertyNumber | Line1     | Line2           | Line3 | Postcode | City | County |
		| updated abc  | 2              | ugly Flat | 55 Baker Street |       | N1C      |      |        |
	Then User should get OK http status code
		And The updated Property is saved in data base


@Property
@ignore
Scenario: Update non exisitng property
	When Users tries to updates property
		| Id | Type | Country        | PropertyNumber | PropertyName    | AddressLine1       | AddressLine2 | Postcode | Town   | County         |
			| -4 | Flat | United Kingdom | 1000           | Apartment    | Baker Street |              | W2W      | Liverpool |        |
	Then User should get OK http status code
