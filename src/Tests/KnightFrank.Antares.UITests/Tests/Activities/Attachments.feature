Feature: Activities attachments UI tests

@Activity
Scenario: Upload attachment for activity
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Sir   | Felix     | Jordan   |
		And Property with Commercial division and Leisure.Hotel type is defined
		And Property attributes details are defined
			| MinArea  | MaxArea  | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces | MinGuestRooms | MaxGuestRooms | MinFunctionRooms | MaxFunctionRooms |
			| 20000.25 | 30000.45 | 25000.19    | 40000.1     | 30                  | 50                  | 100           | 200           | 10               | 20               |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName                       | Line2      | Postcode | City        | County             |
			| 391            | Field Palmer Estate Agents Shirley | Shirley Rd | SO15 3JD | Southampton | Southampton County |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2015   | 1000000  |
		And Property Open Market Letting activity is defined
	When User navigates to view activity page with id
		And User clicks add attachment button on view activity page
		And User adds PDF document.pdf file with Brochure type on attach file page
	Then Attachment should be displayed on view activity page
		| FileName         | Type     | Size   |
		| PDF document.pdf | Brochure | 2.9 MB |
	When User clicks attachment details link on view activity page
	Then Attachment details on attachment preview page are the same like on view activity page
		And Attachment PDF document.pdf should be downloaded
		And User closes attachment preview page on view activity page
