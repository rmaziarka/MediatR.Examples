Feature: Property attachments UI tests

@Property
Scenario: Upload attachment for property
	Given Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName      | Line2       | Postcode | City | County |
			| 391            | The Estate Office | Aliquet Ave | 90002    | York | York   |
	When User navigates to view property page with id
		And User clicks add attachment button on view property page
		And User adds Floor Plan.PDF file with Floor Plan type to property on attach file page
	Then Attachment should be displayed on view poperty page
		| FileName       | Type       | Size   |
		| Floor Plan.PDF | Floor Plan | 2.9 MB |
	When User clicks attachment card on view property page
	Then Attachment details on attachment preview page are the same like on view property page
		And Property attachment Floor Plan.pdf should be downloaded
		And User closes attachment preview page on view property page
