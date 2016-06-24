Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode            | enumTypeItemCode |
		| ActivityDocumentType    | FloorPlan        |
		| PropertyDocumentType    | Brochure         |
		| RequirementDocumentType | TermsOfBusiness  |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Contacts exists in database
			| Title  | FirstName | LastName |
			| Mister | Tomasz    | Bien    |
		And Requirement exists in database
	When User retrieves url for <entity> attachment upload for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode | entity      |
	|          | FloorPlan                | BadRequest | Activity    |
	| test.png | FloorPlan                | OK         | Activity    |
	|          | Brochure                 | BadRequest | Property    |
	| test.png | Brochure                 | OK         | Property    |
	|          | TermsOfBusiness          | BadRequest | Requirement |
	| test.png | TermsOfBusiness          | OK         | Requirement |

@Attachment
Scenario Outline: Download attachment for entity with ActivityDocumentType
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode            | enumTypeItemCode |
		| ActivityDocumentType    | FloorPlan        |
		| PropertyDocumentType    | Brochure         |
		| RequirementDocumentType | TermsOfBusiness  |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Contacts exists in database
			| Title  | FirstName | LastName |
			| Mister | Tomasz    | Bien    |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Requirement exists in database
	When User retrieves url for <entity> attachment download for <filename> and <activityDocumentTypeCode> code
	Then User should get <statusCode> http status code

	Examples: 
	| filename | activityDocumentTypeCode | statusCode | entity      |
	|          | FloorPlan                | BadRequest | Activity    |
	| test.png | FloorPlan                | OK         | Activity    |
	|          | Brochure                 | BadRequest | Property    |
	| test.png | Brochure                 | OK         | Property    |
	|          | TermsOfBusiness          | BadRequest | Requirement |
	| test.png | TermsOfBusiness          | OK         | Requirement |
