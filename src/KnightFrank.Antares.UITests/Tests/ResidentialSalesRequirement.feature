Feature: Residential Sales Requirement

Scenario: Create new residential sales requirement
	Given User navigates to create residential sales requirement page
	When User fills in requirement details on create residential sales requirement page
		| Country        | StreetName   | Postcode | Town   |
		| United Kingdom | Upper Ground | SE1 9PP  | London |
		And User clicks save button on create residential sales requirement page
	Then New residential sales requirement should be created
