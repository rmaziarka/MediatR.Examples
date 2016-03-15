Feature: Create Property UI tests

@ignore
Scenario: Create new property for UK Address
	Given User navigates to create property page
	When User selects country on create property page
	| Country        |
	| United Kingdom |
		And User fills in address details on create property page
		| PropertyNumber | PropertyName | AddressLine2 | AddressLine3 | Postcode | City   | County |
		| 55             | Knight Frank | Baker Street |              | W1U 8AN  | London | London |
		And User clicks save button on create property page
	Then New property should be created