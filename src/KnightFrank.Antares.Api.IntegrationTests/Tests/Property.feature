Feature: Add, update and view Property

@ignore
Scenario: Create property in DB
	Given User is adding a new property
		| Id | Type | Country        | PropertyNumber | PropertyName    | AddressLine1       | AddressLine2 | Postcode | Town   | County         |
		| 3  | Flat | United Kingdom | 1              | Beautifull Flat | Lewis Cubit Square | King Cross   | N1C      | London | Greater London |
	When User retrevies data from DB
	Then the result should be same as
		| Id | Type | Country        | PropertyNumber | PropertyName    | AddressLine1       | AddressLine2 | Postcode | Town   | County         |
		| 3  | Flat | United Kingdom | 1              | Beautifull Flat | Lewis Cubit Square | King Cross   | N1C      | London | Greater London |

@ignore	
Scenario: Update property in DB
	Given Details of property are provided
		| Id | Type | Country        | PropertyNumber | PropertyName    | AddressLine1       | AddressLine2 | Postcode | Town   | County         |
		| 3  | Flat | United Kingdom | 1              | Beautifull Flat | Lewis Cubit Square | King Cross   | N1C      | London | Greater London |
		And Users updates property
			| Id | Type | Country        | PropertyNumber | PropertyName         | AddressLine1       | AddressLine2 | Postcode | Town   | County         |
			| 3  | Flat | United Kingdom | 100            | Gasstation Apartment | Lewis Cubit Square | King Cross   | N1C      | London | Greater London |
	When User retrevies data from DB
	Then the results should be same as
			| Id | Type | Country        | PropertyNumber | PropertyName         | AddressLine1       | AddressLine2 | Postcode | Town   | County         |
			| 3  | Flat | United Kingdom | 100            | Gasstation Apartment | Lewis Cubit Square | King Cross   | N1C      | London | Greater London |

@ignore	
Scenario: Update non exisitng property
	Given Property existing in DB
		| Id | Type | Country        | PropertyNumber | PropertyName    | AddressLine1       | AddressLine2 | Postcode | Town   | County         |
		| 3  | Flat | United Kingdom | 1              | Beautifull Flat | Lewis Cubit Square | King Cross   | N1C      | London | Greater London |
		And Users updates property which is not existing in DB
			| Id | Type | Country        | PropertyNumber | PropertyName | AddressLine1 | AddressLine2 | Postcode | Town      | County |
			| -4 | Flat | United Kingdom | 1000           | Apartment    | Baker Street |              | W2W      | Liverpool |        |
	When User retrevies data from DB
	Then Property should not be updated in DB
		And HTTP 404 Not found should be received by user