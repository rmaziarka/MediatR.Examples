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
		And Following property area breakdown is defined
			| Name  | Size   |
			| area1 | 100    |
			| area2 | 1000   |
			| area3 | 999.99 |
	When User creates defined property area breakdown for latest property
	Then User should get OK http status code
		And Added property area breakdowns should exist in data base

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
		And Following property area breakdown is defined
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
			| enumTypeCode | enumTypeItemCode |
			| Division     | Commercial       |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following property area breakdown is in database
			| Name  | Size   | Order |
			| area1 | 0.1    | 0     |
			| area2 | 1000   | 1     |
			| area3 | 999.99 | 2     |
	When User retrieves property details
	Then User should get OK http status code
		And Returned property area breakdowns should be as expected

@Property
Scenario Outline: Update property area breakdown order
	Given User gets GB address form for Property and country details
        And User gets Office for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Commercial       |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following property area breakdown is in database
			| Name  | Size   | Order |
			| area1 | 0.1    | 0     |
			| area2 | 1000   | 1     |
			| area3 | 999.99 | 2     |
	When User sets <order1> order for area1 property area breakdown for latest property
	Then User should get OK http status code
		And Property area breakdown should be updated
			| Name  | Size   | Order |
			| area1 | 0.1    | <order1> |
			| area2 | 1000   | <order2> |
			| area3 | 999.99 | <order3> |

	Examples: 
		| order1 | order2 | order3 |
		| 2      | 0      | 1      |
		| 0      | 1      | 2      |

	@Property
Scenario Outline: Try to update property area breakdown order with invalid data
	Given User gets GB address form for Property and country details
        And User gets Office for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode | enumTypeItemCode |
			| Division     | Commercial       |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following property area breakdown is in database
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
	| 91AC6B12-020A-11E6-8D22-5E5517507C66 | area1 | 0     | BadRequest |


@Property
Scenario: Update property area breakdown name and size
	Given User gets GB address form for Property and country details
        And User gets Office for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Commercial      |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following property area breakdown is in database
			| Name  | Size   | Order |
			| area1 | 0.1    | 0     |
			| area2 | 1000   | 1     |
			| area3 | 999.99 | 2     |
	When User updates updatedAreaaName name and 1024 size property area breakdown with area1 for latest property
	Then User should get OK http status code
		And Property area breakdown should be updated
			| Name             | Size   | Order |
			| updatedAreaaName | 1024   | 0     |
			| area2            | 1000   | 1     |
			| area3            | 999.99 | 2     |

@Property
Scenario Outline: Try to update property area breakdown name and size with invalida name
	Given User gets GB address form for Property and country details
        And User gets Office for PropertyType
        And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Commercial      |
		And Property with Address and Commercial division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Following property area breakdown is in database
			| Name  | Size   | Order |
			| area1 | 0.1    | 0     |
			| area2 | 1000   | 1     |
			| area3 | 999.99 | 2     |
	When User updates <updatedName> name and <size> size property area breakdown with <name> for <property> property
	Then User should get <responseCode> http status code

Examples: 
	| property                             | name  | size | updatedName | responseCode |
	| latest                               | k     | 1    | updatedName | BadRequest   |
	| 91AC6B12-020A-11E6-8D22-5E5517507C66 | area1 | 0    | updatedName | BadRequest   |
	| latest                               | area1 | 1    |             | BadRequest   |