Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| ActivityDocumentType   | TermsOfBusiness  |
			| UserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User retrieves url for activity attachment upload for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode |
	|          | TermsOfBusiness          | BadRequest |
	| test.png | TermsOfBusiness          | OK         |

@Attachment
Scenario Outline: Download attachment for entity with ActivityDocumentType
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| ActivityDocumentType   | TermsOfBusiness  |
			| UserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User retrieves url for activity attachment download for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode |
	|          | TermsOfBusiness          | BadRequest |
	| test.png | TermsOfBusiness          | OK         |
