Feature: Attachments

@Attachment
Scenario: Upload attachment
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| PropertyDocumentType | Brochure         |
	When User uploads property attachment for latest property id for Brochure with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get OK http status code

@Attachment
Scenario Outline: Upload attachment with invalid data
	Given User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| PropertyDocumentType | Brochure         |
			| ActivityDocumentType | Offices          |
		And Property exists in database
			| PropertyType | Division    |
			| House        | Residential |
	When User uploads property attachment for <propertyId> property id for <documentType> with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get <statusCode> http status code

	Examples:
	| documentType | propertyId                           | statusCode |
	| Offices      | latest                               | BadRequest |
	| Brochure     | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 | BadRequest |
	| Brochure     |                                      | NotFound   |

Scenario: Get Activity with attachment
	Given Property exists in database
		| PropertyType | Division    |
		| House        | Residential |
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| PropertyDocumentType | Brochure         |
		And Property attachment for Brochure with following data exists in database
			| FileName | Size | ExternalDocumentId                   |
			| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	When User retrieves property details
	Then User should get OK http status code
		And Retrieved property should have expected attachments 
