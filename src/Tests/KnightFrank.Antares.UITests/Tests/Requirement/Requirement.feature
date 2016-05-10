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
		And User clicks save button on create requirement page
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
		And Requirement create date is equal to today

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
		
@Requirement
Scenario: Create viewing on requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Dr    | Amber     | Brooks  |
		| Dr    | Sarah     | Knight  |
		| Dr    | Kennedi   | Hyde    |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 4           | 1             | 3             | 2            | 3            | 2000.12 | 4000.12 | 6000.13     | 10000.1     | 3                   | 5                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName    | Line2     | Postcode | City   | County           |
			| 120            | Knight Lancelot | Baker Str | A1O &YT  | London | County Of London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2014   | 1000000  |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 100000   | 500000   | 2           | 3           | 2                 | 4                 | 1            | 3            | 2                | 2                | 90000   | 150000  | 200000      | 300000      | Note        |
	When User navigates to view requirement page with id
		And User clicks add viewings button on view requirement page
		And User selects activity on view requirement page
			| PropertyName    | PropertyNumber | Line2     |
			| Knight Lancelot | 120            | Baker Str |
		And User fills in viewing details on view requirement page
			| Date       | StartTime | EndTime | InvitationText |
			| 12-05-2016 | 10:00     | 11:00   | Text           |
		And User selects attendees for viewing on view requirement page
			| Attendee     |
			| Amber Brooks |
			| Kennedi Hyde |
		And User clicks save activity button on view requirement page
	Then Viewing details on 1 position on view requirement page are same as the following
		| Date       | Time          | Name                           |
		| 12-05-2016 | 10:00 - 11:00 | Knight Lancelot, 120 Baker Str |
	When User clicks 1 viewings details link on view requirement page
	Then Viewing details on view requirement page are same as the following
		| Activity                       | Date       | StartTime | EndTime | Negotiator | Attendees                 | InvitationText |
		| Knight Lancelot, 120 Baker Str | 12-05-2016 | 10:00     | 11:00   | John Smith | Amber Brooks;Kennedi Hyde | Text           |
