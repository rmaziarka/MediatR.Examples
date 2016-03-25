Feature: Create Property UI tests

Scenario: Create new property for UK Address
	Given User navigates to create property page
	When User selects 'United Kingdom' country on create property page
		And User fills in address details on create property page
			| PropertyNumber | PropertyName | Line2        | Line3 | Postcode | City   | County |
			| 55             | Knight Frank | Baker Street |       | W1U 8AN  | London | London |
		And User clicks save button on create property page
	Then New property should be created with address details 
		| PropertyNumber | PropertyName | Line2        | Postcode | City   | County |
		| 55             | Knight Frank | Baker Street | W1U 8AN  | London | London |
	When User clicks edit button on property details page
		And User fills in address details on create property page
			| PropertyNumber | PropertyName | Line2 | Line3 | Postcode | City | County |
			|                |              |       |       | W1U 8AN  |      |        |
		And User clicks save button on create property page
	Then New property should be created with address details 
		| PropertyNumber | PropertyName | Line2 | Postcode | City | County |
		|                |              |       | W1U 8AN  |      |        |
	#When User cliks add activites button on property details page		
	#Then Activity details are set on activity panel
	#	| Vendor | Status        |
	#	|        | Pre-appraisal |
	#When User selects save button on activity panel
	#Then Activity creation date is set to current date on property details page
#		And Activity details are set on property details page
#		| Vendor | Status        |
#		|        | Pre-appraisal |
