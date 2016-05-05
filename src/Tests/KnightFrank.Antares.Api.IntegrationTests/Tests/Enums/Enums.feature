Feature: Enums

@Enums
Scenario: Get enums
	When User retrieves Enums
	Then User should get OK http status code
		And Result should get appropriate enums with enums type
