Feature: Attachments

@Attachment
Scenario: Upload attachment
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode         | enumTypeItemCode |
		| ActivityDocumentType | TermsOfBusiness  |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
	When User uploads attachment for latest activity id for TermsOfBusiness with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get OK http status code

@Attachment
Scenario Outline: Upload attachment with invalid data
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode           | enumTypeItemCode |
		| ActivityDocumentType   | <documentType>   |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
	When User uploads attachment for <activityId> activity id for <documentType> with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get <statusCode> http status code

	Examples:
	| documentType    | activityId                           | statusCode |
	| Offices         | latest                               | BadRequest |
	| TermsOfBusiness | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 | BadRequest |
	| TermsOfBusiness |                                      | NotFound   |

@Activity
Scenario: Get Activity with attachment
	Given User gets EnumTypeItemId and EnumTypeItem code
		| enumTypeCode         | enumTypeItemCode |
		| ActivityDocumentType | TermsOfBusiness  |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
		And Activity exists in database
			| ActivityStatus | ActivityType  |
			| PreAppraisal   | Freehold Sale |
		And Attachment for TermsOfBusiness with following data exists in database
			| FileName | Size | ExternalDocumentId                   |
			| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	When User gets activity with latest id
	Then User should get OK http status code
		And Retrieved activity should have expected attachments 
