Feature: Configure property types for a Division and a Country

@ignore
Scenario: Get list of configured property types for a division and country
	When Country UK is selected
		And Division Residential is selected
	Then list of property types should be received
		| Property types   |
		| House            |
		| Flat             |
		| Bungalow         |
		| Maisonette       |
		| Studio Flat      |
		| Development Plot |
		| Farm/Estate      |
		| Garage Only      |
		| Parking Space    |
		| Land             |
		| Houseboat        |

@ignore
Scenario: Negative - Division is not specify
	When Division is not specify
		But Country is specify
	Then error 400 or 404 should be received

@ignore
Scenario: Negative - Country is not specify
	When Country is not specify
		But Division Residential is selected
	Then error 400 or 404 should be received

@ignore
Scenario: Negative - Country is not existing in DB
	When Division Residential is selected
		And Country NZ is selected
	Then error 400 or 404 should be received

@ignore
Scenario: Negative - Division is not existing in DB
	When Country UK is selected
		And Division Circus is selected
	Then error 400 or 404 should be received
