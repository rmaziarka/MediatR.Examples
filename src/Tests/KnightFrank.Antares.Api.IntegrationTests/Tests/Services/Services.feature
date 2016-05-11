Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
 # begin: add activity
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| ActivityStatus       | PreAppraisal     |
			| Division             | Residential      |
			| ActivityDocumentType | TermsOfBusiness  |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
 # end: add activity
	When User retrieves url for activity attachment upload for <filename> and <activityDocumentTypeCode>
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode |
	|          | TermsOfBusiness         | BadRequest |
	| test.png | TermsOfBusiness         | OK         |

@Attachment
Scenario Outline: Download attachment for entity with ActivityDocumentType
 # begin: add activity
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| ActivityStatus       | PreAppraisal     |
			| Division             | Residential      |
			| ActivityDocumentType | TermsOfBusiness  |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
 # end: add activity
	When User retrieves url for activity attachment download for <filename> and <activityDocumentTypeCode>
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode |
	|          | TermsOfBusiness         | BadRequest |
	| test.png | TermsOfBusiness         | OK         |