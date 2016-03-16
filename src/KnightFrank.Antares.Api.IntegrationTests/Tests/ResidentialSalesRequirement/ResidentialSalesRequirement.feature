Feature: Residential sales requirements 

@ResidentialSalesRequirements 
Scenario: Save requirement to DB with contact and all detailed fields fulfilled
	When User creates a contact with following data 
		| FirstName | Surname | Title  |
		| Tomasz    | Bien    | Mister | 
		And User creates following requirement with given contact
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
 			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
		And User retrieves requirement that he saved
	Then User should get OK http status code
		And Requirement should be the same as added 

@ResidentialSalesRequirements 
Scenario: Negative - Save requirement to DB without contact, all detailed fields fulfilled
	When User creates following requirement without contact			
		| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
		| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
	Then User should get BadRequest http status code

@Requirements
Scenario Outline: Retrieve error messages for improper requirement id		
		When User retrieves requirement for <id> id
		Then User should get <statusCode> http status code

		Examples: 
		| id                                   | statusCode |
		| 00000000-0000-0000-0000-000000000000 | NotFound   |
		| A                                    | BadRequest |
