Feature: Requirements 

@ResidentialSalesRequirements 
Scenario: Save residential sales requirement
	Given User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User sets locations details for the requirement
		| Postcode | City   | Line2   |
		| 1234     | London | Big Ben |
		And User creates following requirement using api
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
 			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
	Then User should get OK http status code
		And Requirement should be the same as added

@ResidentialSalesRequirements 
Scenario: Get residential sales requirement
	Given User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User sets locations details for the requirement
		| Postcode | City   | Line2   |
		| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
 			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
		And User retrieves requirement for latest id
	Then User should get OK http status code
		And Requirement should be the same as added

@ResidentialSalesRequirements 
Scenario: Save residential sales requirement without contact
	Given User gets GB address form for Property and country details
	When User sets locations details for the requirement
		| Postcode | City   | Line2   |
		| 1234     | London | Big Ben |
		And User creates following requirement without contact using api
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
			| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
	Then User should get BadRequest http status code

@ResidentialSalesRequirements
Scenario: Save residential sales requirement without country
	Given User gets GB address form for Property and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User creates following requirement without country using api			
		| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
		| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
	Then User should get BadRequest http status code

@ResidentialSalesRequirements
Scenario: Save residential sales requirement without address form
	Given User gets GB address form for Property and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
	When User creates following requirement without address form using api			
		| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description            |
		| 1000000  | 4000000  | 1           | 5           | 0                 | 2                 | 1            | 3            | 1                | 2                | 1200    | 2000    | 10000       | 20000       | RequirementDescription |
	Then User should get BadRequest http status code

@ResidentialSalesRequirements
Scenario: Save residential sales requirement with invalid contact
	Given User gets GB address form for Property and country details
	When User sets locations details for the requirement
		| Postcode | City   | Line2   |
		| 1234     | London | Big Ben |
		And User creates following requirement with invalid contact using api			
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
