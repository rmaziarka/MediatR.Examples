Feature: Residential sales requirements 

Scenario: Save requirement to DB with contact and all detailed fields fulfilled
	When User creates a contact with following data 
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister | 
		And User creates following requirement with given contact
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
 			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
		# And User retrieves requirement that he saved
	Then User should get OK http status code
		# And Requirement should be the same as added 

Scenario: Negative - Save requirement to DB without contact, all detailed fields fulfilled
	When User creates following requirement without contact			
		| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
		| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
	Then User should get BadRequest http status code
	# And error message should be displayed - ask dev