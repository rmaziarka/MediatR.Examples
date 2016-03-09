Feature: Residential Sales Requirement UI tests

@ignore
Scenario: Create new residential sales requirement
	Given User navigates to create residential sales requirement page
	When User fills in location details on create residential sales requirement page
		| Country        | StreetName   | Postcode | Town   |
		| United Kingdom | Upper Ground | SE1 9PP  | London |
		And User fills in property details on create residential sales requirement page
			| Type | PriceMin | PriceMax | BedroomsMin | BedroomsMax | ReceptionRoomsMin | ReceptionRoomsMax | BathroomsMin | BathroomsMax | ParkingSpacesMin | ParkingSpacesMax | AreaMin | AreaMax | LandAreaMin | LandAreaMax | RequirementsNote |
			| Flat | 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note             |
		And User clicks save button on create residential sales requirement page
	Then New residential sales requirement should be created
