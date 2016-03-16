Feature: Note on residential sales requirement

@ignore	
@ResidentialSalesRequirements 
Scenario: Get list of all notes for existing residential sales requirement
	When User creates a contact with following data 
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister | 
		And User creates following requirement with given contact
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
 			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
		And User creates note with following details for given residential sales requirement
			| Description           |
			| Test note description |
			| Second test note description |
		And User retrieves notes for residential sales requirement
	Then User should get OK http status code
		And Notes details should be the same as already added

@ignore	
@ResidentialSalesRequirements 
Scenario: Save note to non existing residential sales requirement
	When User creates note with following details for non existing residential sales requirement
		| Description           |
		| Test note description |
	Then User should get BadRequest http status code

@ignore	
@ResidentialSalesRequirements 
Scenario: Get list of all notes for non existing residential sales requirement
	When User retrieves notes for non existing residential sales requirement
	Then User should get BadRequest http status code
