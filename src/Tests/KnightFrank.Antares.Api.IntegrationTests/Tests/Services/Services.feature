Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| ActivityDocumentType   | TermsOfBusiness  |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
	When User retrieves url for activity attachment upload for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode |
	|          | TermsOfBusiness          | BadRequest |
	| test.png | TermsOfBusiness          | OK         |

@Attachment
Scenario Outline: Download attachment for entity with ActivityDocumentType
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| ActivityDocumentType   | TermsOfBusiness  |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
	When User retrieves url for activity attachment download for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode |
	|          | TermsOfBusiness          | BadRequest |
	| test.png | TermsOfBusiness          | OK         |
