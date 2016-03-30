Feature: Residential Sales Requirement UI tests

Scenario: Create new residential sales requirement
	Given User navigates to create contact page
		And User creates contacts on create contact page
			| Title | FirstName | Surname |
			| Miss  | Alana     | Jones   |
	When User navigates to create residential sales requirement page
		And User selects contacts on create residential sales requirement page
			| FirstName | Surname |
			| Alana     | Jones   |
		And User selects contacts on create residential sales requirement page
			| FirstName | Surname |
			| Alana     | Jones   |
	Then list of applicants should contain following contacts
		| FirstName | Surname |
		| Alana     | Jones   |
	When User fills in location details on create residential sales requirement page
		| Country        | Line2        | Postcode | City   |
		| United Kingdom | Upper Ground | SE1 9PP  | London |
		When User fills in property details on create residential sales requirement page
			| Type | MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| Flat | 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note        |
		And User clicks save button on create residential sales requirement page
	Then New residential sales requirement should be created
		And residential sales requirement location details are same as the following
			| Country | PropertyName | PropertyNumber | Line1 | Line2        | Line3 | Postcode | City   | County |
			|         |              |                |       | Upper Ground |       | SE1 9PP  | London |        |
		And residential sales requirement property details are same as the following
			| Type | MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| Flat | 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note        |
		And residential sales requirement applicants are same as the following
			| FirstName | Surname |
			| Alana     | Jones   |
		And residential sales requirement create date is equal to today
