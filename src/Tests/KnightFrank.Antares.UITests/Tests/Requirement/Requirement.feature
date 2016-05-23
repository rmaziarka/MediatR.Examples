Feature: Requirement UI tests

@Requirement
Scenario: Create requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Miss  | Alana     | Jones   |
	When User navigates to create requirement page
		And User selects contacts on create requirement page
			| FirstName | Surname |
			| Alana     | Jones   |
		And User selects contacts on create requirement page
			| FirstName | Surname |
			| Alana     | Jones   |
	Then List of applicants should contain following contacts
		| FirstName | Surname |
		| Alana     | Jones   |
	When User fills in location details on create requirement page
		| Country        | Line2        | Postcode | City   |
		| United Kingdom | Upper Ground | SE1 9PP  | London |
	When User fills in property details on create requirement page
		| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
		| 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note        |
		And User clicks save requirement button on create requirement page
	Then New requirement should be created
		And Requirement location details on view requirement page are same as the following
			| Line2        | Postcode | City   |
			| Upper Ground | SE1 9PP  | London |
		And Requirement property details on view requirement page are same as the following
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note        |
		And Requirement applicants on view requirement page are same as the following
			| FirstName | Surname |
			| Alana     | Jones   |

@Requirement
Scenario: Create note on requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Miss  | Anna      | Wilder  |
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note        |
	When User navigates to view requirement page with id
	When User clicks notes button on view requirement page
		And User adds note on view requirement page
			| Description                                                            |
			| This is an example text of note. Text was created for testing purposes |
	Then Note is displayed in recent notes area on view requirement page
		And Notes number increased on view requirement page
