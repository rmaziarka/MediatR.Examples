Feature: Requirement attachments UI tests

@Requirement
Scenario: Upload attachment for requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Miss  | Alana     | Duran   |
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 1000     | 5000     | 1           | 1           | 1                 | 1                 | 1            | 1            | 1                | 1                | 900     | 1500    | 2000        | 3000        | Note        |
	When User navigates to view requirement page with id
