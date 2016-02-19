Feature: Google

Scenario: Search using google
	Given User navigates to google page
	When User looks for 'Objectivity' in google
	Then results should be visible
