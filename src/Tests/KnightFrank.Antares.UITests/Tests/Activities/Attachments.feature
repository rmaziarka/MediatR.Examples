Feature: Activities attachments UI tests

@Activity
Scenario: Upload attachment for activity
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Sir   | Felix     | Jordan  |
		And Property with Residential division and Development Plot type is defined
		And Property attributes details are defined
			| MinLandArea | MaxLandArea |
			| 25000.19    | 40000.1     |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName                       | Line2      | Postcode | City        | County             |
			| 391            | Field Palmer Estate Agents Shirley | Shirley Rd | SO15 3JD | Southampton | Southampton County |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2015   | 1000000  |
		And Property Long Leasehold Sale activity is defined
	When User navigates to view activity page with id
		And User switches to attachments tab on view activity page
		And User clicks add attachment button on attachments tab on view activity page
		And User adds PDF document.pdf file with Brochure type on attachments tab on view activity page
	Then Attachment should be displayed on attachments tab on view activity page
		| FileName         | Type     | Size   |
		| PDF document.pdf | Brochure | 2.9 MB |
	When User clicks attachment card on attachments tab on view activity page
	Then Attachment preview details are the same like on attachments tab on view activity page
		And Activity attachment PDF document.pdf should be downloaded
		And User closes attachment preview page on attachments tab on view activity page
