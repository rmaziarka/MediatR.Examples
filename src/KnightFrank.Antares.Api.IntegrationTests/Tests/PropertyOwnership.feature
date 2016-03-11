Feature: Property ownership

@ignore	
Scenario: Save ownership where dates are not overlapping
	Given Property exist in DB
		And property has already ownership
			| Id | Type     | Purchasing date | Selling date | Name     |
			| 1  | Freehold | 01-05-2010      | 01-04-2012   | Jon Snow |
		When User update the property
		| Id | Type     | Purchasing date | Selling date | Name        |
		| 2  | Freehold | 01-05-2012      | 01-07-2015   | Sarah Conor |
	Then HTTP 200 OK should be received
		And Entry should be added to DB
@ignore	
Scenario: Save ownership where dates are overlapping
	Given Property exist in DB
		And has ownership
			| Id | Type     | Purchasing date | Selling date | Name     |
			| 1  | Freehold | 01-05-2010      | 01-04-2012   | Jon Snow |
	When User update the property
		| Id | Type     | Purchasing date | Selling date | Name        |
		| 2  | Freehold | 01-01-2012      | 01-07-2015   | Sarah Conor |
	Then HTTP 403 Frobidden should be received

@ignore	
Scenario: Get property ownership list for existing property and ownership with no entry
	Given Property exist in DB
		But property has not any ownership
	When User send GET request to view property details
	Then ownership from DB should be visible as empty list on property details page

@ignore	
Scenario: Get property ownership list for existing property and ownership with one entry
	Given Property exist in DB
		And property has already ownership
			| Id | Type     | Purchasing date | Selling date | Name     |
			| 1  | Freehold | 01-05-2010      | 01-04-2012   | Jon Snow |
	When User send GET request to view property details
	Then ownership from DB should be visible as list on property details page

@ignore	
Scenario: Get property ownership list for existing property and ownership with more than 2 entry
	Given Property exist in DB
		And property has already ownership
			| Id | Type     | Purchasing date | Selling date | Name          |
			| 1  | Freehold | 01-05-2010      | 01-04-2012   | Jon Snow      |
			| 2  | Freehold | 01-07-1990      | 01-12-1998   | Bruce Willice |
			| 3  | Freehold | 04-04-2001      | 24-04-2003   | Frodo Baggins |
			| 4  | Freehold | 13-11-2004      | 29-02-2008   | Bruce Wayne   |
	When User send GET request to view property details
	Then ownership from DB should be visible as list on property details page

@ignore	
Scenario: Get property ownership list for non existing property
	Given Property with id=10 not exist in DB
	When User send GET request for ownership list for property with id=10
	Then HTTP 404 Not found should be received by user

@ignore	
Scenario: Get property ownership details for non existing ownership
	Given contect with id=10 not exist in DB
	When Users send GET request for ownership with id=10
	Then HTTP 404 Not found should be received by user