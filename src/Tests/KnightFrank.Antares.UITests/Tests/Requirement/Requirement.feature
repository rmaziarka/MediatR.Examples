Feature: Requirement UI tests

@Requirement
Scenario: Create residential sale requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Miss  | Alana     | Jones   |
	When User navigates to create requirement page
		And User selects contacts on create requirement page
			| FirstName | Surname |
			| Alana     | Jones   |
	Then List of applicants should contain following contacts
		| FirstName | Surname |
		| Alana     | Jones   |
	When User fills in sale requirement details on create requirement page
		| Type             | Description |
		| Residential Sale | Description |
		And User fills in location details on create requirement page
			| Country        | Line2        | Postcode | City   |
			| United Kingdom | Upper Ground | SE1 9PP  | London |
		And User clicks save requirement button on create requirement page
	Then New requirement should be created
		And Requirement location details on view requirement page are same as the following
			| Line2        | Postcode | City   |
			| Upper Ground | SE1 9PP  | London |
		And Sale requirement details on view requirement page are same as the following
			| Type             | Description |
			| Residential Sale | Description |
		And Requirement applicants on view requirement page are same as the following
			| FirstName | Surname |
			| Alana     | Jones   |

@Requirement
Scenario: Create residential letting requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Dr    | Alan      | Harper  |
	When User navigates to create requirement page
		And User selects contacts on create requirement page
			| FirstName | Surname |
			| Alan      | Harper  |
	Then List of applicants should contain following contacts
		| FirstName | Surname |
		| Alan      | Harper  |
	When User fills in letting requirement details on create requirement page
		| Type                | Description | RentMin | RentMax |
		| Residential Letting | Description | 1000    | 2000    |
		And User fills in location details on create requirement page
			| Country        | Line2        | Postcode | City   |
			| United Kingdom | Lower Ground | ES1 P9P  | London |
		And User clicks save requirement button on create requirement page
	Then New requirement should be created
		And Requirement location details on view requirement page are same as the following
			| Line2        | Postcode | City   |
			| Lower Ground | ES1 P9P  | London |
		And Letting requirement details on view requirement page are same as the following
			| Type                | Description | RentMin | RentMax |
			| Residential Letting | Description | 1000    | 2000    |
		And Requirement applicants on view requirement page are same as the following
			| FirstName | Surname |
			| Alan      | Harper  |

@Requirement
Scenario: Create note on residential letting requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Miss  | Anna      | Wilder  |
		And Requirement for GB is created in database
			| Type                | Description |
			| Residential Letting | Description |
	When User navigates to view requirement page with id
	When User clicks notes button on view requirement page
		And User adds note on view requirement page
			| Description                                                            |
			| This is an example text of note. Text was created for testing purposes |
	Then Note should be displayed in recent notes area on view requirement page
		And Notes number should increase on view requirement page
