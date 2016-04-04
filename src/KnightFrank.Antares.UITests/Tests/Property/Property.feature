Feature: Property UI tests

@Property
Scenario: Create and update property with UK address
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
			| PropertyNumber | PropertyName | Line2 | Line3          | Postcode | City | County |
			|                |              |       | Address line 3 | W1U 8AN  |      |        |
		And User clicks save button on create property page
	Then Property should be updated with address details 
		| PropertyNumber | PropertyName | Line2 | Line3          | Postcode | City | County |
		|                |              |       | Address line 3 | W1U 8AN  |      |        |

@Property
@Ownership
@Activity
@ignore
Scenario: Property ownership and activity
	Given User navigates to create contact page
		And User creates contacts on create contact page
			| Title | FirstName | Surname   |
			| King  | Arthur    | Pendragon |
	When User navigates to create property page
		And User selects 'United Kingdom' country on create property page
		And User fills in address details on create property page
			| PropertyNumber | PropertyName      | Line2    | Line3 | Postcode | City   | County      |
			| 20             | Westminster Abbey | Deans Yd |       | SW1P 3PA | London | Westminster |
		And User clicks save button on create property page
	Then New property should be created with address details 
		| PropertyNumber | PropertyName      | Line2    | Postcode | City   | County      |
		| 20             | Westminster Abbey | Deans Yd | SW1P 3PA | London | Westminster |
	When User selects contacts for ownership on view property page
		| FirstName | Surname   |
		| Arthur    | Pendragon |
		And User fills in ownership details on ownership details page
			| Type       | Current | PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| Freeholder | false   | 01-03-2015   | 01-02-2016 | 1000000  | 1200000   |
	#Then Ownership details on position 1 should contain following data on view property page
	#	| Contact          | Type       | PurchaseDate | SellDate   |
	#	| Arthur Pendragon | Freeholder | 01-03-2015   | 01-02-2016 |
	#When User clicks add activites button on property details page		
	#Then Activity details are set on activity panel
	#	| Vendor | Status        |
	#	|        | Pre-appraisal |
	#When User clicks save button on activity panel
	#Then Activity creation date is set to current date on property details page
	#	And Activity details are set on property details page
	#	| Vendor | Status        |
	#	|        | Pre-appraisal |
	#When User clicks activity's details link on property details page
	#Then Activity preview panel is displayed with details the same like details on activity tile
