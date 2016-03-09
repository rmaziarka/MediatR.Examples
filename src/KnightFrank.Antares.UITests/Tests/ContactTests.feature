Feature: Create Contact UI tests


Scenario: Create contact using valid details
	Given User navigates to create contact page
	When User fills in contact details on create contact page
		| Title | FirstName | Surname |
		| Miss  | Sarah     | Conor   |
		And User clicks save button on create contact page
	Then New contact should be created

