Feature: Enums

@Enums
Scenario: Get enums by code
	Given There is EnumType
		| Code           |
		| EntityTypeTest |
		And There is EnumTypeItem
			| Code             |
			| EnumTypeItemTest |
		And There is EnumLocalized for given EnumType and EN Locale
			| Value            |
			| EnumTypeItemTest |
	When User retrieves EnumTypes by EntityTypeTest code 
	Then User should get OK http status code
		And Result should contain single element 
		And Single element has Id being set
		And Single element should be equal to
			| Value            |
			| EnumTypeItemTest |


Scenario: Get enums
	When User retrieves Enums
	Then User should get OK http status code
		And Result should get appropriate enums with enums type