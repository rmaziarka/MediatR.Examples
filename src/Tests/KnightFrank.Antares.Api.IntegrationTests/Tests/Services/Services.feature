Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| ActivityDocumentType   | TermsOfBusiness  |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| PropertyDocumentType   | Brochure         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User retrieves url for <entity> attachment upload for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode | entity   |
	|          | TermsOfBusiness          | BadRequest | Activity |
	| test.png | TermsOfBusiness          | OK         | Activity |
	|          | Brochure                 | BadRequest | Property |
	| test.png | Brochure                 | OK         | Property |

@Attachment
Scenario Outline: Download attachment for entity with ActivityDocumentType
	Given User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode           | enumTypeItemCode |
			| ActivityStatus         | PreAppraisal     |
			| ActivityDocumentType   | TermsOfBusiness  |
			| ActivityUserType       | LeadNegotiator   |
			| ActivityDepartmentType | Managing         |
			| PropertyDocumentType   | Brochure         |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User retrieves url for <entity> attachment download for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode | entity   |
	|          | TermsOfBusiness          | BadRequest | Activity |
	| test.png | TermsOfBusiness          | OK         | Activity |
	|          | Brochure                 | BadRequest | Property |
	| test.png | Brochure                 | OK         | Property |
