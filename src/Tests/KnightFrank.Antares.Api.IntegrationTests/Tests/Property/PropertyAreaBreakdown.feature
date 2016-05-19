Feature: PropertyAreaBreakdown



Scenario: Add property area breakdown
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Commercial       |
        And User gets Office for PropertyType
		And Property with Address and Commercial division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			|              |                |       |       | N1C      |      |        |
		And Following propery areas breakdown are defined
			| Name  | Size   |
			| area1 | 100    |
			| area2 | 1000   |
			| area3 | 999.99 |
	When User creates defined property area breakdown for latest property
	Then User should get OK http status code
		And Added property area breakdowns exists in data base

Scenario Outline: Check Validation for area breakdown
	Given User gets GB address form for Property and country details
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Commercial       |
        And User gets Office for PropertyType
		And Property with Address and Commercial division is in database
			| PropertyName | PropertyNumber | Line2 | Line3 | Postcode | City | County |
			|              |                |       |       | N1C      |      |        |
		And Following propery areas breakdown are defined
			| Name   | Size   |
			| <name> | <size> |
	When User creates defined property area breakdown for <property> property
	Then User should get <response> http status code

	Examples: 
	| name | size | property                             | response   |
	|      | 6    | latest                               | BadRequest |
	| 2    | d    | latest                               | BadRequest |
	| 2    |      | latest                               | BadRequest |
	| abc  | 55   | 91AC6B12-020A-11E6-8D22-5E5517507C66 | BadRequest |

