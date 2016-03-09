Feature: GetEnums WebApi method

@mytag
Scenario: Add two numbers
	Given When User creates a contact with following data
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
