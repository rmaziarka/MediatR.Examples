Feature: Property area breakdown

@Property
Scenario: Create property area breakdown
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

@Property
Scenario Outline: Create area breakdown with invalid data
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

@Property
Scenario: Get property with property area breakdown
	Given User gets GB address form for Property and country details
        And User gets Office for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Commercial      |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following propery areas breakdown are defined and put in data base
			| Name  | Size   | Order |
			| area1 | 0.1    | 0     |
			| area2 | 1000   | 1     |
			| area3 | 999.99 | 2     |
	When User retrieves property details
	Then User should get OK http status code
		And Returned property area breakdowns are as expected


@Property
Scenario: Update property area breakdown order
	Given User gets GB address form for Property and country details
        And User gets Office for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Commercial      |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following propery areas breakdown are defined and put in data base
			| Name  | Size   | Order |
			| area1 | 0.1    | 0     |
			| area2 | 1000   | 1     |
			| area3 | 999.99 | 2     |
	When User sets 2 order for area1 property area breakdown for latest property
	Then User should get OK http status code
		And Property area breakdowns should have new order
			| Name  | Size   | Order |
			| area1 | 0.1    | 2     |
			| area2 | 1000   | 0     |
			| area3 | 999.99 | 1     |


	@Property
Scenario Outline: Try to update property area breakdown order with improper data
	Given User gets GB address form for Property and country details
        And User gets Office for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Commercial      |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following propery areas breakdown are defined and put in data base
			| Name  | Size   | Order |
			| area1 | 0.1    | 0     |
			| area2 | 1000   | 1     |
			| area3 | 999.99 | 2     |
	When User sets <order> order for <name> property area breakdown for <property> property
	Then User should get <response> http status code

	Examples: 
	| property                             | name  | order | response   |
	| latest                               | area1 | -1    | BadRequest |
	| latest                               | area  | 1     | BadRequest |
	| latest                               | area1 | 0     | OK         |
	| 91AC6B12-020A-11E6-8D22-5E5517507C66 | area1 | 0     | BadRequest |