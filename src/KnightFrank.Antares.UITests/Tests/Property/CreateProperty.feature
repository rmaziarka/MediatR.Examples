Feature: Create Property UI tests

@ignore
Scenario: Create new property for UK Address
	Given User navigates to create property page
	When User selects country on create property page
		| Country        |
		| United Kingdom |
		And User selects property types on create property page
			| Type        |
			| Flat        |
			| Studio flat |
		And User fills in address details on create property page
			| PropertyNumber | PropertyName | AddressLine2 | AddressLine3 | Postcode | City   | County |
			| 55             | Knight Frank | Baker Street |              | W1U 8AN  | London | London |
		And User clicks save button on create property page
	Then New property should be created with address details 
		| Country        | PropertyNumber | PropertyName | AddressLine2 | Postcode | City   | County |
		| United Kingdom | 55             | Knight Frank | Baker Street | W1U 8AN  | London | London |
	When User cliks add activites button on property details page		
	Then Activity details are set on activity panel
		| Vendor | Status        |
		|        | Pre-appraisal |
	When User selects save button on activity panel
	Then Activity creation date is set to current date on property details page
		And Activity details are set on property details page
		| Vendor | Status        |
		|        | Pre-appraisal |
