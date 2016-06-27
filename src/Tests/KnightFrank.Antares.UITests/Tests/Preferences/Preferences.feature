Feature: Preferences

@Preferences
Scenario Outline: Set user preferences for new user
	Given User navigates to Edit Preferences page
	When User selects <setSalutation> format 
		And User saves preferences
	Then User is taken to the contact add page
	When User navigates to Edit Preferences page
	Then User salutation is set to  <setSalutation>

	Examples: 
		| setSalutation   |
		| Mr John Smith   |
		| John Smith, Esq |