Feature: Viewing UI tests

@Requirement
@Viewing
Scenario: Create viewing on residential sale requirement
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Dr    | Amber     | Brooks   |
		| Dr    | Sarah     | Knight   |
		| Dr    | Kennedi   | Hyde     |
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
			| Type             | Description |
			| Residential Sale | Description |
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
		And User clicks save viewing button on view requirement page
	Then Side panel should not be displayed on view requirement page
		And Viewing details on 1 position on view requirement page are same as the following
			| Date       | Time          | Name                           |
			| 12-05-2016 | 10:00 - 11:00 | Knight Lancelot, 120 Baker Str |
	When User clicks 1 viewings details on view requirement page
	Then Viewing details on view requirement page are same as the following
		| Name                           | Date       | StartTime | EndTime | Negotiator | Attendees                 | InvitationText | PostViewingComment |
		| Knight Lancelot, 120 Baker Str | 12-05-2016 | 10:00     | 11:00   | John Smith | Amber Brooks;Kennedi Hyde | Text           |                    |
	When User clicks view activity from viewing on view requirement page
	Then View activity page should be displayed
		And Viewing details on 1 position on view activity page are same as the following
			| Date       | Time          | Name                                     |
			| 12-05-2016 | 10:00 - 11:00 | Amber Brooks, Sarah Knight, Kennedi Hyde |
	When User clicks 1 viewings details link on view activity page
	Then Viewing details on view activity page are same as the following
		| Name                                     | Date       | StartTime | EndTime | Negotiator | Attendees                 | InvitationText | PostViewingComment |
		| Amber Brooks, Sarah Knight, Kennedi Hyde | 12-05-2016 | 10:00     | 11:00   | John Smith | Amber Brooks;Kennedi Hyde | Text           |                    |
	When User clicks requirement details from viewing on view activity page
	Then View requirement page should be displayed

@Requirement
@Viewing
Scenario: Update viewing on residential sale requirement
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Dr    | Alan      | Baker    |
		| Sir   | Martin    | Jackson  |
		| Dr    | Alex      | Baldwin  |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 4           | 2             | 4             | 2            | 3            | 1500.1  | 3000.2  | 2200.15     | 10000.1     | 1                   | 2                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2        | Postcode | City       | County     |
			| 6              | My House     | Blisworth Cl | M4 7DT   | Manchester | Manchester |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-2013   | 10000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks 1 viewings details on view requirement page
		And User clicks edit viewing button on view requirement page
		And User fills in viewing details on view requirement page
			| Date       | StartTime | EndTime | InvitationText | PostViewingComment   |
			| 12-12-2016 | 12:00     | 14:00   | Text           | Post Viewing Comment |
		And User unselects attendees for viewing on view requirement page
			| Attendee     |
			| Alan Baker   |
			| Alex Baldwin |
		And User clicks save viewing button on view requirement page
	Then Viewing details on 1 position on view requirement page are same as the following
		| Date       | Time          | Name                     |
		| 12-12-2016 | 12:00 - 14:00 | My House, 6 Blisworth Cl |
	And Viewing details on view requirement page are same as the following
		| Name                     | Date       | StartTime | EndTime | Negotiator | Attendees      | InvitationText | PostViewingComment   |
		| My House, 6 Blisworth Cl | 12-12-2016 | 12:00     | 14:00   | John Smith | Martin Jackson | Text           | Post Viewing Comment |

@Requirement
@Viewing
Scenario: Create viewing on residential letting requirement
	Given Contacts are created in database
		| Title | FirstName | LastName   |
		| Lady  | Maggie    | Smith      |
		| Lady  | Judi      | Dench      |
		| Lady  | Cate      | Blanchett  |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 3           | 5           | 2             | 4             | 3            | 4            | 2500.12 | 4500.12 | 6500.13     | 11000.1     | 3                   | 5                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName        | Line2      | Postcode | City   | County           |
			| 1              | Judi Dench Property | Stanway St | N1 6RJ   | London | County Of London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2015   | 1999999  |
		And Property Open Market Letting activity is defined
		And Requirement for GB is created in database
			| Type                | Description |
			| Residential Letting | Description |
	When User navigates to view requirement page with id
		And User clicks add viewings button on view requirement page
		And User selects activity on view requirement page
			| PropertyName        | PropertyNumber | Line2      |
			| Judi Dench Property | 1              | Stanway St |
		And User fills in viewing details on view requirement page
			| Date       | StartTime | EndTime | InvitationText |
			| 12-06-2016 | 10:00     | 11:00   | Text           |
		And User selects attendees for viewing on view requirement page
			| Attendee      |
			| Maggie Smith  |
			| Cate Blanchet |
		And User clicks save viewing button on view requirement page
	Then Side panel should not be displayed on view requirement page
		And Viewing details on 1 position on view requirement page are same as the following
			| Date       | Time          | Name                              |
			| 12-06-2016 | 10:00 - 11:00 | Judi Dench Property, 1 Stanway St |
	When User clicks 1 viewings details on view requirement page
	Then Viewing details on view requirement page are same as the following
		| Name                              | Date       | StartTime | EndTime | Negotiator | Attendees                   | InvitationText | PostViewingComment |
		| Judi Dench Property, 1 Stanway St | 12-06-2016 | 10:00     | 11:00   | John Smith | Maggie Smith;Cate Blanchett | Text           |                    |
	When User clicks view activity from viewing on view requirement page
	Then View activity page should be displayed
		And Viewing details on 1 position on view activity page are same as the following
			| Date       | Time          | Name                                   |
			| 12-06-2016 | 10:00 - 11:00 | Maggie Smith, Judi Dench, Cate Blanchett |
	When User clicks 1 viewings details link on view activity page
	Then Viewing details on view activity page are same as the following
		| Name                                     | Date       | StartTime | EndTime | Negotiator | Attendees                   | InvitationText | PostViewingComment |
		| Maggie Smith, Judi Dench, Cate Blanchett | 12-06-2016 | 10:00     | 11:00   | John Smith | Maggie Smith;Cate Blanchett | Text           |                    |
	When User clicks requirement details from viewing on view activity page
	Then View requirement page should be displayed

@Requirement
@Viewing
Scenario: Update viewing on residential letting requirement
	Given Contacts are created in database
		| Title | FirstName | LastName  |
		| Lady  | Audrey    | Hepburn   |
		| Lady  | Diane     | Keaton    |
		| Dr    | Michelle  | Pfeiffer  |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 4           | 2             | 4             | 2            | 3            | 1500.1  | 3000.2  | 2200.15     | 10000.1     | 1                   | 2                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2    | Postcode | City       | County         |
			| 95             | Bradley House | Heath Rd | TW1 4DH  | Twickenham | Greater London |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-2010   | 10000000 |
		And Property Open Market Letting activity is defined
		And Requirement for GB is created in database
			| Type                | Description |
			| Residential Letting | Description |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks 1 viewings details on view requirement page
		And User clicks edit viewing button on view requirement page
		And User fills in viewing details on view requirement page
			| Date       | StartTime | EndTime | InvitationText | PostViewingComment   |
			| 01-01-2017 | 13:00     | 16:00   | Text           | Post Viewing Comment |
		And User unselects attendees for viewing on view requirement page
			| Attendee          |
			| Audrey Hepburn    |
			| Michelle Pfeiffer |
		And User clicks save viewing button on view requirement page
	Then Viewing details on 1 position on view requirement page are same as the following
		| Date       | Time          | Name                       |
		| 01-01-2017 | 13:00 - 16:00 | Bradley House, 95 Heath Rd |
	And Viewing details on view requirement page are same as the following
		| Name                       | Date       | StartTime | EndTime | Negotiator | Attendees    | InvitationText | PostViewingComment   |
		| Bradley House, 95 Heath Rd | 01-01-2017 | 13:00     | 16:00   | John Smith | Diane Keaton | Text           | Post Viewing Comment |
